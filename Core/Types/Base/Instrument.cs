using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Core.Types.Base;
public abstract class Instrument {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Symbol { get; init; }
    public required string Exchange { get; init; }
    public required DateTime ExpirationDate { get; init; }
    public required int Multiplier { get; init; }
    public required int PriceMagnifier { get; init; }
    public required List<int> MarketRulesId { get; init; }

    [JsonIgnore]
    public int DaysToExpiration => (ExpirationDate - DateTime.Now).Days;
}
