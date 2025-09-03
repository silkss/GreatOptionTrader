using System;
using System.Collections.Generic;

namespace GreatOptionTrader.Models;

public class OptionStrategiesContainer : OptionStrategy {
    public required Guid Guid { get; init; }
    public required string Name { get; set; }
    public required List<OptionStrategy> Strategies { get; init; }
    public required string Account { get; init; }
}
