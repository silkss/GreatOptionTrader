using GreatOptionTrader.Models;

namespace GreatOptionTrader.DTO;

public class InstrumentToGroupDTO {
    public required InstrumentGroup Group { get; init; }
    public required InstrumentRequestDTO InstrumentRequestDTO { get; init; }
}
