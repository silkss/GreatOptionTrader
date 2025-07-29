using GreatOptionTrader.Abstractions;
using GreatOptionTrader.DTO;
using GreatOptionTrader.Models;
using GreatOptionTrader.Services.Repositories;
using IBApi;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using GreatOptionTrader.Services.Converters;

namespace GreatOptionTrader.Services.Connectors;

public delegate void ItemRequestedEventHandler<TItem> (int requestId, TItem item);

public class InteractiveBroker {
    
    private readonly IbWrapper wrapper;
    private readonly EReaderMonitorSignal monitor;
    private readonly EClientSocket socket;

    private readonly List<IPriceable<double>> instrumentCache;

    public InteractiveBroker(
        ILogger<InteractiveBroker> logger) {
        instrumentCache = new(100);
        wrapper = new IbWrapper(instrumentCache, logger);
        monitor = new EReaderMonitorSignal();
        socket = new EClientSocket(wrapper, monitor);

    }

    public event ItemRequestedEventHandler<Instrument> OnContractDetailsRequested {
        add => wrapper.OnContractDetailsRequested += value;
        remove => wrapper.OnContractDetailsRequested -= value;
    }

    public IEnumerable<string> Accounts => wrapper.Accounts ?? [];
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
            instrument.Instrument.ToIbContract(),
            string.Empty, false, false, null);
    }

    public int GetValidOrderId () => wrapper.ValidOrderId++;

    public void PlaceOrder(Instrument instrument, Models.Order order) {
        socket.placeOrder(order.OrderId, instrument.ToIbContract(), order.ToIbOrder());
    }
}
