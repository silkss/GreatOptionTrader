using System;
using System.ComponentModel;

namespace GreatOptionTrader.Models;

public class Instrument : INotifyPropertyChanged {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Symbol { get; init; }
    public required string Exchange { get; init; }
    public required DateTime ExpirationDate { get; init; }
    public required int Multiplier { get; init; }
    public double Strike { get; init; }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void RaisePropertyChanged(string propertyName) {
        PropertyChanged?.Invoke(this, new(propertyName));
    }

    private double bid;
    private double ask;

    public double Bid {
        get => bid;
        set {
            if (value != bid) {
                bid = value;
                RaisePropertyChanged(nameof(Bid));
            }
        }
    }
    public double Ask {
        get => ask;
        set {
            if (value != ask) {
                ask = value;
                RaisePropertyChanged(nameof(Ask));
            }
        }
    }
}
