using System.Collections.Generic;

namespace GreatOptionTrader.Models;

public class OptionModel : Core.Option {
    public int InstrumentGroupId { get; set; }
    public List<Core.Order> Orders { get; set; } = [];
}
