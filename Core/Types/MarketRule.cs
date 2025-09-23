using System.Collections.Generic;

namespace Core.Types;
public class MarketRule {
    public required int Id { get; init; }
    public required List<PriceIncrement> PriceIncrements { get; init; }
}
