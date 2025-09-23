using Connectors;
using Connectors.IB;
using Core.Types;
using Hedger.Models;

namespace Hedger.ViewModels;
public class ContainerViewModel : MarketDataObserver<Future> {
    private readonly InteractiveBroker broker;
    private readonly Container container;

    public ContainerViewModel(Container container, InteractiveBroker broker) {
        this.container = container;
        this.broker = broker;
    }

    public decimal LastPrice { get; private set; }
    public decimal AskPrice { get; private set; }
    public decimal BidPrice { get; private set; }

    public override Future Instrument => container.Basis;
    public Container Container => container;

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
                }
                break;
        }

    }
    public void Start() {
        broker.SubscribeOnMarketData(this);
    }
}
