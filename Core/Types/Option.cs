using Core.Types.Base;

namespace Core.Types;
public class Option : Instrument {
    public required decimal Strike { get; init; }
    public required OptionRight Right { get; init; }
    public required string TradingClass { get; init; }
}
