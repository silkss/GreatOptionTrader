using Connectors.IB;
using Core;
using GreatOptionTrader.Models;
using GreatOptionTrader.ViewModels.Base;

namespace GreatOptionTrader.ViewModels;

public class OptionStrategyViewModel : BaseOptionStrategyViewModel{
    public OptionStrategyViewModel (
        OptionStrategy strategy, 
        InteractiveBroker broker) : base(strategy.Orders) {
        Strategy = strategy;
        SubscribeOwnEvents(broker);
    }

    public override Option Instrument => Strategy.Instrument;

    public decimal WantedVolume { get; set; } = 1m;

    private decimal wantedPrice;
    public decimal WantedPrice {
        get => wantedPrice == 0m ? (AskPrice + BidPrice) / 2m : wantedPrice;
        set => wantedPrice = value;
    }
    
    public TradeDirection WantedDirection { get; set; }
    public string? WantedAccount { get; set; }
    public OptionStrategy Strategy { get; }
    
    public void AddOrder (Order order) {
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
}