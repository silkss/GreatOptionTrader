using Core;
using GreatOptionTrader.ViewModels.Base;
using System;
using System.Collections.Generic;

namespace GreatOptionTrader.ViewModels;
public class PositionViewModel : ObservableObject {
    public static decimal CalculatePnL (decimal buyPrice, decimal sellPrice) => sellPrice - buyPrice;
    private decimal previousOpeningPrice;
    private decimal openPnL;

    public decimal CurrentVolume { get; private set; }
    public decimal AbsoluteCurrentVolume => Math.Abs(CurrentVolume);
    public decimal PreviousOpeningPrice {
        get => previousOpeningPrice;
        private set => Set(ref previousOpeningPrice, value);
    }
    public decimal AverageFilledPrice { get; private set; }
    public decimal CommissionCurrency { get; private set; }
    public decimal FixedPnL { get; private set; }
    public decimal OpenPnL { 
        get => openPnL;
        set => Set(ref openPnL, value);
    }

    public decimal GetFixedCurrencyPnL (int multiplier) => FixedPnL * multiplier - CommissionCurrency;

    public PositionViewModel (IEnumerable<Order> orders) {
        foreach (var order in orders) ProcessOrder(order);
    }

    private decimal positionPrice () => AbsoluteCurrentVolume * AverageFilledPrice;

    public void ProcessOrder (Order order) {
        if (order.Commission != 0m) CommissionCurrency += order.Commission;

        if (order.Direction == TradeDirection.Buy) {
            if (CurrentVolume == 0m) {
                CurrentVolume = order.FilledVolume;
                AverageFilledPrice = order.AverageFilledPrice;
                RaisePropertyChanged(nameof(AverageFilledPrice));
                RaisePropertyChanged(nameof(CurrentVolume));
                return;
            }
            if (CurrentVolume > 0m) {
                var volume = CurrentVolume + order.FilledVolume;
                AverageFilledPrice = (positionPrice() + order.GetPositionPrice()) / volume;
                CurrentVolume = volume;
                RaisePropertyChanged(nameof(AverageFilledPrice));
                RaisePropertyChanged(nameof(CurrentVolume));
                return;
            }
            if (CurrentVolume < 0m) {
                var volume = CurrentVolume + order.FilledVolume;
                if (volume == 0m) {
                    FixedPnL += CalculatePnL(order.AverageFilledPrice, AverageFilledPrice) * order.FilledVolume;
                    PreviousOpeningPrice = AverageFilledPrice;
                    AverageFilledPrice = 0m;
                    RaisePropertyChanged(nameof(AverageFilledPrice));
                }
                else if (volume < 0m) {
                    FixedPnL += CalculatePnL(order.AverageFilledPrice, AverageFilledPrice) * order.FilledVolume;
                }
                else if (volume > 0m) {
                    FixedPnL += CalculatePnL(order.AverageFilledPrice, AverageFilledPrice) * Math.Abs(CurrentVolume);
                    AverageFilledPrice = order.AverageFilledPrice;
                    RaisePropertyChanged(nameof(AverageFilledPrice));
                }
                CurrentVolume = volume;
                RaisePropertyChanged(nameof(CurrentVolume));
                return;
            }
        }
        if (order.Direction == TradeDirection.Sell) {
            if (CurrentVolume == 0m) {
                CurrentVolume -= order.FilledVolume;
                AverageFilledPrice = order.AverageFilledPrice;
                RaisePropertyChanged(nameof(AverageFilledPrice));
                RaisePropertyChanged(nameof(CurrentVolume));
                return;
            }
            if (CurrentVolume < 0m) {
                var volume = CurrentVolume - order.FilledVolume;
                AverageFilledPrice = (positionPrice() + order.GetPositionPrice()) / Math.Abs(volume);
                CurrentVolume = volume;
                return;
            }
            if (CurrentVolume > 0m) {
                decimal volume = CurrentVolume - order.FilledVolume;
                if (volume == 0m) {
                    FixedPnL += CalculatePnL(AverageFilledPrice, order.AverageFilledPrice) * order.FilledVolume;
                    PreviousOpeningPrice = AverageFilledPrice;
                    AverageFilledPrice = 0m;
                    RaisePropertyChanged(nameof(AverageFilledPrice));
                }
                else if (volume < 0m) {
                    FixedPnL += CalculatePnL(AverageFilledPrice, order.AverageFilledPrice) * order.FilledVolume;
                }
                else if (volume > 0m) {
                    FixedPnL += CalculatePnL(AverageFilledPrice, order.AverageFilledPrice) * Math.Abs(CurrentVolume);
                    AverageFilledPrice = order.AverageFilledPrice;
                    RaisePropertyChanged(nameof(AverageFilledPrice));
                }
                CurrentVolume = volume;
                RaisePropertyChanged(nameof(CurrentVolume));
            }
        }
    }
}
