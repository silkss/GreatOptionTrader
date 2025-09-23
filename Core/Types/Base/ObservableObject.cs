using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Core.Types.Base;
public abstract class ObservableObject : INotifyPropertyChanged {
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void RaisePropertyChanged (string propertyName) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected virtual bool Set<T> (ref T field, T value, [CallerMemberName] string? propertyName = null) {
        ArgumentNullException.ThrowIfNull(propertyName);
        if (Equals(field, value)) return false;
        field = value;
        RaisePropertyChanged(propertyName);
        return true;
    }
}
