using Core.Types;
using System.Collections.Generic;

namespace GreatOptionTrader.Models;
public class OptionStrategy {
    public required Option Instrument { get; init; }
    public required List<Order> Orders { get; init; }
}
