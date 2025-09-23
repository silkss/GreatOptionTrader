using IBApi;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using Core.Types;
using Core.Types.Base;

namespace Connectors.IB;

public class InteractiveBroker {
    private readonly IbWrapper wrapper;
    private readonly EReaderMonitorSignal monitor;
    private readonly EClientSocket socket;

    private readonly Dictionary<int, List<Core.Types.PriceIncrement>> marketRules;

    public InteractiveBroker(ILogger<InteractiveBroker> logger) { 
        marketRules = [];

        wrapper = new IbWrapper(marketRules, logger);
        monitor = new EReaderMonitorSignal();
        socket = new EClientSocket(wrapper, monitor);
    }

    public event ItemUpdatedEvent<Option> OptionRequestedEvent {
        add => wrapper.OptionRequestedEvent += value;
        remove => wrapper.OptionRequestedEvent -= value;
    }

    public event ItemUpdatedEvent<Future> FutureReqeustedEvent {
        add => wrapper.FutureReqeustedEvent += value;
        remove => wrapper.FutureReqeustedEvent -= value;
    }

    public event ItemUpdatedEvent<OrderStatusEventArgument> OrderStatusUpdated {
        add => wrapper.OrderStatusUpdated += value;
        remove => wrapper.OrderStatusUpdated -= value;
    }

    public event ItemUpdatedEvent<CommissionUpdateEventArgs> CommissionUpdated {
        add => wrapper.CommissionUpdated += value;
        remove => wrapper.CommissionUpdated -= value;
    }

    public event ItemUpdatedEvent<CompletedOrderEventArgument> CompletedOrderUpdated {
        add => wrapper.CompletedOrderUpdated += value;
        remove => wrapper.CompletedOrderUpdated -= value;
    }

    public string[]? Accounts => wrapper.Accounts;

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
            socket.reqCompletedOrders(apiOnly: true);
        }
    }

    public void RequestFuture(int requestId, string futureName, string futureExchange) {
        var future = new Contract() {
            LocalSymbol = futureName.Trim().ToUpper(),
            Exchange = futureExchange.Trim().ToUpper(),
            SecType = "FUT"
        };

        socket.reqContractDetails(requestId, contract: future);
    }

    public void RequestOption(int requestId, string optionName, string otpionExchange) {
        var contract = new Contract() {
            LocalSymbol = optionName.Trim().ToUpper(),
            Exchange = otpionExchange.Trim().ToUpper(),
            SecType = "FOP"
        };

        socket.reqContractDetails(requestId, contract);
    }

    public void SubscribeOnMarketData<TInstrument> (MarketDataObserver<TInstrument> observer) 
        where TInstrument : Instrument {
        if (wrapper.tickObservers.TryGetValue(observer.Instrument.Id, out var observers)) {
            observers.Add(observer);
            return;
        }

        wrapper.tickObservers.Add(observer.Instrument.Id, [observer]);

        socket.reqMktData(
            observer.Instrument.Id,
            observer.Instrument.ToIBContract(),
            string.Empty, false, false, null);
    }

    public int GetValidOrderId () => wrapper.ValidOrderId++;

    public void PlaceOrder(Instrument instrument, Core.Types.Order order) {
        var price = order.LimitPrice;

        decimal increment = 0m;
        if (marketRules.TryGetValue(instrument.MarketRulesId.First(), out var increments)) {
            increment = increments.First(i => i.LowEdge < price).Increment;
        }
        if (increment != 0m) order.LimitPrice = price - (price % increment);
        socket.placeOrder(order.BrokerId, instrument.ToIBContract(), order.ToIBLimitOrder());
    }

    public void CancelOrder (int orderId) => socket.cancelOrder(orderId, new());
}
