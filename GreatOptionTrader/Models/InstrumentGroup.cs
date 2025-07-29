using System.Collections.Generic;

namespace GreatOptionTrader.Models;

public class InstrumentGroup {
    public int Id { get; set; }
    public required string Name { get; set; }

    public List<Instrument> Instruments { get; } = [];
}
