using Core;
using GreatOptionTrader.Abstractions;
using GreatOptionTrader.EventArguments;
using GreatOptionTrader.Models;
using GreatOptionTrader.Services.Connectors;
using GreatOptionTrader.Services.Repositories;
using System.Collections.Generic;

namespace GreatOptionTrader.ViewModels;

public class InstrumentViewModel : Base.ObservableObject, IPriceable<decimal> {
    private readonly OrdersRepository ordersRepository;

    private static Order? checkIfHaveOpenOrder(IEnumerable<Order> orders) {
        Order? openOrder = null;
        foreach (var order in orders) {
            if (order.Status == OrderStatus.Submitted) {
                openOrder = order;
            }
        }

        return openOrder;
    }

    public InstrumentViewModel (
        OptionModel instrument, 
        OrdersRepository ordersRepository, 
        InteractiveBroker broker) {
        Instrument = instrument;
        this.ordersRepository = ordersRepository;
        Position = new PositionViewModel(instrument.Orders);
        OpenOrder = checkIfHaveOpenOrder(instrument.Orders);
        SubscribeOwnEvents(broker);
    }

    public PositionViewModel Position { get; }
    public OptionModel Instrument { get; }
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

    public void UpdateAskPrice (decimal price) {
        if (AskPrice != price) {
            AskPrice = price;
            RaisePropertyChanged(nameof(AskPrice));
        }

        if (Position.CurrentVolume < 0m) {
            Position.CurrencyOpenPnL = Position.CalculateShortPnL(AskPrice, Instrument.Multiplier);
        }
    }

    public void UpdateBidPrice (decimal price) {
        if (BidPrice != price) {
            BidPrice = price;
            RaisePropertyChanged(nameof(BidPrice));
        }

        if (Position.CurrentVolume > 0m) {
            Position.CurrencyOpenPnL = Position.CalculateLongPnL(BidPrice, Instrument.Multiplier);
        }
    }

    public void UpdateLastPrice (decimal price) {
        if (LastPrice != price) {
            LastPrice = price;
            RaisePropertyChanged(nameof(LastPrice));
        }
    }

    public void AddOrder(Order order) {
        if (OpenOrder == null) {
            OpenOrder = order;
            Instrument.Orders.Add(order);
            ordersRepository.Add(order);
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
        ordersRepository.Update(OpenOrder);
        OpenOrder = null;
    }

    public void UpdateOrderStatus(int id, OrderStatusEventArgument arg) {
        if (OpenOrder == null) return;
        if (OpenOrder.BrokerId != id) return;

        OpenOrder.Status = arg.Status;
        OpenOrder.AverageFilledPrice = arg.AverageFilledPrice;
        OpenOrder.FilledVolume = arg.FilledVolume;
        
        ordersRepository.Update(OpenOrder);

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
}