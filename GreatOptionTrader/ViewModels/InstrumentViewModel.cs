using Core;
using GreatOptionTrader.Abstractions;
using GreatOptionTrader.EventArguments;
using GreatOptionTrader.Models;
using GreatOptionTrader.Services.Connectors;
using GreatOptionTrader.Services.Repositories;
using System.Collections.Generic;

namespace GreatOptionTrader.ViewModels;

public class InstrumentViewModel : Base.ObservableObject, IPriceable<double> {
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
        Instrument instrument, 
        OrdersRepository ordersRepository, 
        InteractiveBroker broker) {
        Instrument = instrument;
        this.ordersRepository = ordersRepository;
        Position = new PositionViewModel(instrument.Orders);
        OpenOrder = checkIfHaveOpenOrder(instrument.Orders);
        SubscribeOwnEvents(broker);
    }

    public PositionViewModel Position { get; }
    public Instrument Instrument { get; }
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

    public void UpdateAskPrice (double price) {
        if (price <= 0) {
            return;
        }
        
        if (price == double.MaxValue) {
            return;
        }

        var newPrice = (decimal)price;
        if (AskPrice != newPrice) {
            AskPrice = newPrice;
            RaisePropertyChanged(nameof(AskPrice));
        }

        if (Position.CurrentVolume < 0m) {
            Position.OpenPnL = PositionViewModel.CalculatePnL(AskPrice, Position.AverageFilledPrice);
        }
    }

    public void UpdateBidPrice (double price) {
        if (price <= 0) {
            return;
        }

        if (price == double.MaxValue) {
            return;
        }

        var newPrice = (decimal)price;
        if (BidPrice != newPrice) {
            BidPrice = newPrice;
            RaisePropertyChanged(nameof(BidPrice));
        }

        if (Position.CurrentVolume > 0m) {
            Position.OpenPnL = PositionViewModel.CalculatePnL(Position.AverageFilledPrice, BidPrice);
        }
    }

    public void UpdateLastPrice (double price) {
        if (price <= 0) {
            return;
        }

        if (price == double.MaxValue) {
            return;
        }

        var newPrice = (decimal)price;
        if (LastPrice != newPrice) {
            LastPrice = newPrice;
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
        if (OpenOrder.OrderId != id) return;

        OpenOrder.Status = arg.Status;
        OpenOrder.AverageFilledPrice = arg.AverageFilledPrice;
        OpenOrder.FilledVolume = arg.FilledVolume;

        if (OpenOrder.Status == OrderStatus.Filled) {
            if (OpenOrder.IsCompleted) {
                Position.ProcessOrder(OpenOrder);
                ordersRepository.Update(OpenOrder);
                OpenOrder = null;
                return;
            }
            OpenOrder.IsCompleted = true;
        }
        if (OpenOrder.Status == OrderStatus.Cancelled) OpenOrder = null;
    }

    public void UpdateOrderCommission (int orderId, CommissionUpdateEventArgs args) {
        if (OpenOrder == null) return;
        if (OpenOrder.OrderId != orderId) return;

        OpenOrder.PermId = args.PermId;
        OpenOrder.Commission = args.Commission;
    }
}