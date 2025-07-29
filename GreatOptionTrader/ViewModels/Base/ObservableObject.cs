using System.ComponentModel;

namespace GreatOptionTrader.ViewModels.Base;
public abstract class ObservableObject : INotifyPropertyChanged {
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void RaisePropertyChanged (string propertyName) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
