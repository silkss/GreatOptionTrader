using GreatOptionTrader.Converters;
using GreatOptionTrader.DTO;
using GreatOptionTrader.Models;
using GreatOptionTrader.Services.Repositories;
using IBApi;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.Threading;

namespace GreatOptionTrader.Services.Connectors;
public class InteractiveBroker {
    private class IbWrapper (
        ObservableCollection<Instrument> cache, 
        ILogger<InteractiveBroker> logger) : DefaultEWrapper {

        public event Action<Instrument> OnContractDetailsRequested = delegate { };

        public override void error (int id, int errorCode, string errorMsg, string advancedOrderRejectJson) {
            logger.LogError("{id}: {errorCode}: {message}", id, errorCode, errorMsg);
        }
        public override void error (string str) => logger.LogCritical("{message}", str);
        public override void error (Exception e) => logger.LogCritical("{message}", e.Message);
        public override void contractDetails (int reqId, ContractDetails contractDetails) {
            OnContractDetailsRequested.Invoke(contractDetails.ToInstrument());
        }
        public override void tickPrice (int tickerId, int field, double price, TickAttrib attribs) {
            switch (field) {
                case TickType.ASK:
                case TickType.DELAYED_ASK:
                    cache[tickerId].Ask = price;
                    break;
                case TickType.BID:
                case TickType.DELAYED_BID:
                    cache[tickerId].Bid = price;
                    break;
            }
        }
    }

    private readonly IbWrapper wrapper;
    private readonly EReaderMonitorSignal monitor;
    private readonly EClientSocket socket;

    public InstrumentRepository InstrumentRepository { get; }

    public InteractiveBroker(
        InstrumentRepository instrumentRepository,
        ILogger<InteractiveBroker> logger) {
        wrapper = new IbWrapper(instrumentRepository.Items, logger);
        monitor = new EReaderMonitorSignal();
        socket = new EClientSocket(wrapper, monitor);

        InstrumentRepository = instrumentRepository;
    }

    public event Action<Instrument> OnContractDetailsRequested {
        add => wrapper.OnContractDetailsRequested += value;
        remove => wrapper.OnContractDetailsRequested -= value;
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

    public void AddInstrumentToCache (Instrument instrument) {
        var tickerId = InstrumentRepository.Create(instrument);
        socket.reqMktData(tickerId, instrument.ToIbContract(), string.Empty, false, false, null);
    }
}
