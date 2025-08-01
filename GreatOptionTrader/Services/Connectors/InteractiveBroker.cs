using GreatOptionTrader.Abstractions;
using GreatOptionTrader.DTO;
using GreatOptionTrader.EventArguments;
using GreatOptionTrader.Models;
using GreatOptionTrader.Services.Converters;
using IBApi;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Threading;

namespace GreatOptionTrader.Services.Connectors;

public class InteractiveBroker {
    private readonly IbWrapper wrapper;
    private readonly EReaderMonitorSignal monitor;
    private readonly EClientSocket socket;

    private readonly List<IPriceable<double>> instrumentCache;

    public InteractiveBroker(
        ILogger<InteractiveBroker> logger,
        ObservableCollection<string> accounts,
        Dispatcher dispatcher) {
        instrumentCache = new(100);
        wrapper = new IbWrapper(instrumentCache, logger, accounts, dispatcher);
        monitor = new EReaderMonitorSignal();
        socket = new EClientSocket(wrapper, monitor);
    }

    public event ItemUpdatedEvent<Instrument> ContractUpdated {
        add => wrapper.ContractUpdated += value;
        remove => wrapper.ContractUpdated -= value;
    }

    public event ItemUpdatedEvent<OrderStatusEventArgument> OrderStatusUpdated {
        add => wrapper.OrderStatusUpdated += value;
        remove => wrapper.OrderStatusUpdated -= value;
    }

    public event ItemUpdatedEvent<CommissionUpdateEventArgs> CommissionUpdated {
        add => wrapper.CommissionUpdated += value;
        remove => wrapper.CommissionUpdated -= value;
    }

    public bool IsConnected () => socket.IsConnected();

    public void Connect (int clientId, string host = "127.0.0.1", int port = 7497) {
        socket.eConnect(host, port, clientId);
        var reader = new EReader(socket, monitor);
        reader.Start();

        new Thread(() => {
            while (socket.IsConnected()) {
                monitor.waitForSignal();
                reader.processMsgs();
            }
        }) {
            IsBackground = true
        }
        .Start();

        Thread.Sleep(500);
        if (socket.IsConnected()) {
            socket.reqMarketDataType(3);
        }
    }

    public void RequestOption(int requestId, string optionName, string otpionExchange) {
        var contract = new Contract() {
            LocalSymbol = optionName.Trim().ToUpper(),
            Exchange = otpionExchange.Trim().ToUpper(),
            SecType = "FOP"
        };

        socket.reqContractDetails(requestId, contract);
    }

    public void RequestOptions(int requestId, InstrumentRequestDTO request) {
        if (string.IsNullOrEmpty(request.InstrumentName) ||
            string.IsNullOrEmpty(request.InstrumentExchange)) {
            return;
        }

        var contract = new Contract() {
            LocalSymbol = request.InstrumentName.Trim().ToUpper(),
            Exchange = request.InstrumentExchange.Trim().ToUpper(),
            SecType = "FOP"
        };

        socket.reqContractDetails(requestId, contract);
    }

    public void Disconnect() {
        if (socket.IsConnected()) {
            socket.eDisconnect();
        }
    }

    public void AddInstrumentToCache (IPriceable<double> instrument) {
        int ticker = instrumentCache.Count;
        if (instrumentCache.Any(i => i.Instrument.Id == instrument.Instrument.Id)) {
            return;
        }

        instrumentCache.Add(instrument);
        
        socket.reqMktData(
            ticker, 
            instrument.Instrument.ToIBContract(),
            string.Empty, false, false, null);
    }

    public int GetValidOrderId () => wrapper.ValidOrderId++;

    public void PlaceOrder(Instrument instrument, Core.Order order) {
        socket.placeOrder(order.OrderId, instrument.ToIBContract(), order.ToIBLimitOrder());
    }
}
