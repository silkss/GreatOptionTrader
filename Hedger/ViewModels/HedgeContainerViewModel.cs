using Connectors;
using Connectors.IB;
using Core.Types;
using Hedger.Models;

namespace Hedger.ViewModels;
public class HedgeContainerViewModel : MarketDataObserver<Future> {
    private readonly InteractiveBroker broker;

    public HedgeContainerViewModel(HedgeContainer hedge, InteractiveBroker broker) {
        Hedge = hedge;
        this.broker = broker;
    }
    
    private void tryBuy(decimal price) {
        decimal tradeVolume = 0m;

        foreach (var level in Hedge.BuyLevels) {
            if (level.State == HedgeLevelState.Observe) {
                if (price >= level.ActivatePrice) {
                    tradeVolume += level.Volume;
                }
            }
        }
    }

    public HedgeContainer Hedge { get; }
    public decimal AskPrice { get; private set; }
    public decimal BidPrice { get; private set; }
    public decimal LastPrice { get; private set; }

    public override Future Instrument => Hedge.Basis;

    public override void OnNext (TickEventArg arg) {
        switch (arg.TickType) {
            case TickType.Bid:
                if (BidPrice != arg.Price) {
                    BidPrice = arg.Price;
                    RaisePropertyChanged(nameof(BidPrice));
                }
                break;
            case TickType.Ask:
                if (AskPrice != arg.Price) {
                    AskPrice = arg.Price;
                    RaisePropertyChanged(nameof(AskPrice));
                }
                break;
            case TickType.LastPrice:
                if (LastPrice != arg.Price) {
                    LastPrice = arg.Price;
                    RaisePropertyChanged(nameof(LastPrice));
                    trade();
                }
                break;
        }
    }
}
