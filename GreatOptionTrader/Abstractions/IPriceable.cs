using GreatOptionTrader.Models;

namespace GreatOptionTrader.Abstractions;

public interface IPriceable<TPrice> {
    void UpdateAskPrice (TPrice price);
    void UpdateBidPrice (TPrice price);
    void UpdateLastPrice (TPrice price);
    Instrument Instrument { get; }
}
