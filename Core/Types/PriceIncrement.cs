using System.Dynamic;

namespace Core.Types;
public class PriceIncrement {
    public required decimal LowEdge { get; init; }
    public required decimal Increment { get; init; }
}
