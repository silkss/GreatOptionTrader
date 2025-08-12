using Core;

namespace GreatOptionTrader.EventArguments;

public class CompletedOrderEventArgument 
{
    public required OrderStatus Status { get; init; }
    public required decimal FilledVolume { get; init; }
}
