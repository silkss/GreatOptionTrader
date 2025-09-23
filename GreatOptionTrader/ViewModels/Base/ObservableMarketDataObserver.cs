using Connectors;
using Core.Types;
using Core.Types.Base;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GreatOptionTrader.ViewModels.Base;
public abstract class ObservableMarketDataObserver<TInstrument> : MarketDataObserver<TInstrument>
    where TInstrument : Instrument {

    public decimal BidPrice { get; private set; }
    public decimal AskPrice { get; private set; }
    public decimal LastPrice { get; private set; }

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
}
