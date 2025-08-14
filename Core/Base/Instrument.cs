using System;

namespace Core.Base;
public abstract class Instrument {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Symbol { get; init; }
    public required string Exchange { get; init; }
    public required DateTime ExpirationDate { get; init; }
    public required int Multiplier { get; init; }
    public required int PriceMagnifier { get; init; }
}
