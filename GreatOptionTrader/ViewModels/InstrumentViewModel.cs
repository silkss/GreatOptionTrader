using GreatOptionTrader.Abstractions;
using GreatOptionTrader.Models;
using GreatOptionTrader.Types;

namespace GreatOptionTrader.ViewModels;

public class InstrumentViewModel : Base.ObservableObject, IPriceable<double> {
    public InstrumentViewModel(Instrument instrument) {
        Instrument = instrument;
    }

    public Instrument Instrument { get; }

    public double AskPrice { get; private set; }
    public double BidPrice { get; private set; }
    public double LastPrice { get; private set; }

    public decimal WantedVolume { get; set; } = 1m;
    private double wantedPrice;
    public double WantedPrice {
        get => wantedPrice == 0.0 ? (AskPrice + BidPrice) / 2.0 : wantedPrice;
        set => wantedPrice = value;
    }
    public TradeDirection WantedDirection { get; set; }

    public void UpdateAskPrice (double price) {
        if (AskPrice != price) {
            AskPrice = price;
            RaisePropertyChanged(nameof(AskPrice));
        }
    }

    public void UpdateBidPrice (double price) {
        if (BidPrice != price) {
            BidPrice = price;
            RaisePropertyChanged(nameof(BidPrice));
        }
    }

    public void UpdateLastPrice (double price) {
        if (LastPrice != price) {
            LastPrice = price;
            RaisePropertyChanged(nameof(LastPrice));
        }
    }
}
