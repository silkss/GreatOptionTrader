using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core;
public class MarketRule {
    public required int Id { get; init; }
    public required List<PriceIncrement> PriceIncrements { get; init; }
}
