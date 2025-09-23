using IBApi;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Collections.Generic;
using Core.Types;

namespace Connectors.IB;

internal class IbWrapper (
        Dictionary<int, List<Core.Types.PriceIncrement>> marketRules,
        ILogger<InteractiveBroker> logger) : DefaultEWrapper  {

    public int ValidOrderId;
    public string[]? Accounts { get; private set; }    
    public readonly Dictionary<int, List<IObserver<TickEventArg>>> tickObservers = [];

    public event ItemUpdatedEvent<Option> OptionRequestedEvent = delegate { };
    public event ItemUpdatedEvent<Future> FutureReqeustedEvent = delegate { };
    public event ItemUpdatedEvent<OrderStatusEventArgument> OrderStatusUpdated = delegate { };
    public event ItemUpdatedEvent<CommissionUpdateEventArgs> CommissionUpdated = delegate { };
    public event ItemUpdatedEvent<CompletedOrderEventArgument> CompletedOrderUpdated = delegate { };

    public override void error (int id, int errorCode, string errorMsg, string advancedOrderRejectJson) {
        logger.LogError("{id}: {errorCode}: {message}", id, errorCode, errorMsg);
            switch (errorCode) {
                case 110: // The price does not conform to the minimum price variation for this contract.
                var arg = new OrderStatusEventArgument() {
                    AverageFilledPrice = 0m,
                    Status = OrderStatus.Cancelled,
                    FilledVolume = 0m
                };
                OrderStatusUpdated.Invoke(id, arg);
                break;
            }
    }

    public override void error (string str) => logger.LogCritical("{message}", str);
    
    public override void error (Exception e) => logger.LogCritical("{message}", e.Message);
    
    public override void contractDetails (int reqId, ContractDetails contractDetails) {
        if (contractDetails.Contract.SecType == "FOP") {
            OptionRequestedEvent(reqId, contractDetails.ToOption());
        }
        else if (contractDetails.Contract.SecType == "FUT") {
            FutureReqeustedEvent.Invoke(reqId, contractDetails.ToFuture());
        }
    }

    public override void managedAccounts (string accountsList) => Accounts = accountsList.Split(",");

    public override void nextValidId (int orderId) => ValidOrderId = orderId;

    private void notifyObservers(int tickerId, Core.Types.TickType tickType, double price) {
        if (price == double.MaxValue) return;
        if (price <= 0.0) return;
        if (tickObservers.TryGetValue(tickerId, out var observers)) {
            var arg = new TickEventArg(tickType, (decimal)price);
            foreach( var observer in observers) {
                observer.OnNext(arg);
            }
        }
    }

    public override void tickPrice (int tickerId, int field, double price, TickAttrib attribs) {
        switch (field) {
            case IBApi.TickType.ASK:
            case IBApi.TickType.DELAYED_ASK:
                notifyObservers(tickerId, Core.Types.TickType.Ask, price);
                break;
            case IBApi.TickType.BID:
            case IBApi.TickType.DELAYED_BID:
                notifyObservers(tickerId, Core.Types.TickType.Bid, price);
                break;
            case IBApi.TickType.LAST:
            case IBApi.TickType.DELAYED_LAST:
                notifyObservers(tickerId, Core.Types.TickType.LastPrice, price);
                break;
            default:
                break;
        }
    }

    public override void orderStatus (
        int orderId, 
        string status, 
        decimal filled, 
        decimal remaining,
        double avgFillPrice,
        int permId,
        int parentId,
        double lastFillPrice,
        int clientId,
        string whyHeld, 
        double mktCapPrice) {
        var arg = new OrderStatusEventArgument() {
            AverageFilledPrice = avgFillPrice == double.MaxValue ? 0m : (decimal)avgFillPrice,
            Status = status.ToOrderStatus(),
            FilledVolume = filled
        };

        OrderStatusUpdated.Invoke(orderId, arg);
    }

    public override void openOrder (
        int orderId, 
        Contract contract, 
        IBApi.Order order, 
        OrderState orderState) {
        CommissionUpdateEventArgs args;
        if (orderState.Commission is double.MaxValue or <= 0.0) {
            args = new CommissionUpdateEventArgs() {
                Commission = 0.0m,
                PermId = order.PermId
            };
            CommissionUpdated.Invoke(orderId, args);
            return;
        }

        args = new CommissionUpdateEventArgs() {
            Commission = (decimal)orderState.Commission,
            PermId = order.PermId
        };
        CommissionUpdated.Invoke(orderId, args);
    }

    public override void completedOrder (Contract contract,  IBApi.Order order, OrderState orderState) {
        CompletedOrderEventArgument arg = new CompletedOrderEventArgument() {
            FilledVolume = order.FilledQuantity,
            Status = orderState.Status.ToOrderStatus()
        };

        CompletedOrderUpdated.Invoke(order.PermId, arg);
    }

    public override void marketRule (int marketRuleId, IBApi.PriceIncrement[] priceIncrements) {
        if (marketRules.ContainsKey(marketRuleId)) return;
        marketRules[marketRuleId] = priceIncrements
            .Select(Converter.ToPriceIncrement)
            .OrderByDescending(p => p.LowEdge)
            .ToList();
    }
}