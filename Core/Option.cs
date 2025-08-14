namespace Core;
public class Option : Base.Instrument {
    public required decimal Strike { get; init; }
    public required OptionRight Right { get; init; }
    public required string TradingClass { get; init; }
}
