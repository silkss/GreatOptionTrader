using Connectors;
using Connectors.IB;
using Core;
using GreatOptionTrader.Models;
using GreatOptionTrader.ViewModels.Base;
using System.Collections.Generic;

namespace GreatOptionTrader.ViewModels;

public class OptionStrategyViewModel : ObservableMarketDataObserver<Option> {
    private static Order? checkIfHaveOpenOrder(IEnumerable<Order> orders) {
        Order? openOrder = null;
        foreach (var order in orders) {
            if (order.Status == OrderStatus.Submitted) {
                openOrder = order;
            }
        }
        return openOrder;
    }

    public OptionStrategyViewModel (
        OptionStrategy strategy, 
        InteractiveBroker broker) {
        Strategy = strategy;
        Position = new PositionViewModel(Strategy.Orders);
        OpenOrder = checkIfHaveOpenOrder(Strategy.Orders);
        SubscribeOwnEvents(broker);
    }

    public override Option Instrument => Strategy.Instrument;

    public PositionViewModel Position { get; }
    public Order? OpenOrder { get; set; }

    public decimal AskPrice { get; private set; }
    public decimal BidPrice { get; private set; }
    public decimal LastPrice { get; private set; }

    public decimal WantedVolume { get; set; } = 1m;

    private decimal wantedPrice;
    public decimal WantedPrice {
        get => wantedPrice == 0m ? (AskPrice + BidPrice) / 2m : wantedPrice;
        set => wantedPrice = value;
    }
    
    public TradeDirection WantedDirection { get; set; }
    public string? WantedAccount { get; set; }
    public OptionStrategy Strategy { get; }

    public void AddOrder(Order order) {
        if (OpenOrder == null) {
            OpenOrder = order;
            Strategy.Orders.Add(order);
        }
    }

    public void SubscribeOwnEvents (InteractiveBroker broker) {
        broker.OrderStatusUpdated += UpdateOrderStatus;
        broker.CommissionUpdated += UpdateOrderCommission;
        broker.CompletedOrderUpdated += OnCompletedOrderUpdated;
    }

    public void OnCompletedOrderUpdated(int permId, CompletedOrderEventArgument arg) {
        if (OpenOrder == null) return;
        if (OpenOrder.PermId != permId) return;

        OpenOrder.FilledVolume = arg.FilledVolume;
        OpenOrder.Status = arg.Status;

        if (OpenOrder.AverageFilledPrice == 0m) {
            OpenOrder.AverageFilledPrice = OpenOrder.LimitPrice;
        }

        Position.ProcessOrder(OpenOrder);
        OpenOrder = null;
    }

    public void UpdateOrderStatus(int id, OrderStatusEventArgument arg) {
        if (OpenOrder == null) return;
        if (OpenOrder.BrokerId != id) return;

        OpenOrder.Status = arg.Status;
        OpenOrder.AverageFilledPrice = arg.AverageFilledPrice;
        OpenOrder.FilledVolume = arg.FilledVolume;
        
        if (OpenOrder.Status == OrderStatus.Filled) {
            if (OpenOrder.IsCompleted) {
                Position.ProcessOrder(OpenOrder);
                OpenOrder = null;
                return;
            }
            OpenOrder.IsCompleted = true;
        }
        if (OpenOrder.Status == OrderStatus.Cancelled) OpenOrder = null;
    }

    public void UpdateOrderCommission (int orderId, CommissionUpdateEventArgs args) {
        if (OpenOrder == null) return;
        if (OpenOrder.BrokerId != orderId) return;

        OpenOrder.PermId = args.PermId;
        OpenOrder.Commission = args.Commission;
    }

    public override void OnNext (TickEventArg arg) {
        switch (arg.TickType) {
            case TickType.Bid:
                if (BidPrice != arg.Price) {
                    BidPrice = arg.Price;
                    RaisePropertyChanged(nameof(BidPrice));
                    if (Position.CurrentVolume > 0m) {
                        Position.CurrencyOpenPnL = Position.CalculateLongPnL(BidPrice, Instrument.Multiplier);
                    }
                }
                break;
            case TickType.Ask:
                if (AskPrice != arg.Price) {
                    AskPrice = arg.Price;
                    RaisePropertyChanged(nameof(AskPrice));

                    if (Position.CurrentVolume < 0m) {
                        Position.CurrencyOpenPnL = Position.CalculateShortPnL(AskPrice, Instrument.Multiplier);
                    }
                }
                break;
            case TickType.LastPrice:
                if (LastPrice != arg.Price) {
                    LastPrice = arg.Price;
                    RaisePropertyChanged(nameof(LastPrice));
                }
                break;
        }
    }
}