using Core.Types;

namespace Connectors;
public class OrderStatusEventArgument {
    public required OrderStatus Status { get; init; }
    public required decimal AverageFilledPrice { get; init; }
    public required decimal FilledVolume { get; init; }
}
