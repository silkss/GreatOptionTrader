using System.Collections.Generic;

namespace Core;
public class MarketRule {
    public required int Id { get; init; }
    public required List<PriceIncrement> PriceIncrements { get; init; }
}
