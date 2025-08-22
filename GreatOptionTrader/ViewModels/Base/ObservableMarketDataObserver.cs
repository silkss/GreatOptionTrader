using Connectors;
using Core.Base;
using System.ComponentModel;

namespace GreatOptionTrader.ViewModels.Base;
public abstract class ObservableMarketDataObserver<TInstrument> : MarketDataObserver<TInstrument>, INotifyPropertyChanged
    where TInstrument : Instrument {
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void RaisePropertyChanged (string propertyName) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
