using System.ComponentModel;
using System.Windows;

namespace GreatOptionTrader.DTO;

public class InstrumentRequestDTO : INotifyPropertyChanged {
    private string? name;
    private string? exchange;
    public string? InstrumentName {
        get => name;
        set {
            if (value != name) {
                name = value;
                PropertyChanged?.Invoke(this, new(nameof(InstrumentName)));
            }
        }
    }

    public string? InstrumentExchange {
        get => exchange;
        set {
            if (value != exchange) {
                exchange = value;
                PropertyChanged?.Invoke(this, new(nameof(InstrumentExchange)));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
