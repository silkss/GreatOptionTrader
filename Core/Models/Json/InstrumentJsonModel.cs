using System;

namespace Core.Models.Json;

public record InstrumentJsonModel (
    InstrumentRightJsonModel Right,
    InstrumentTypeJsonModel Type,
    int Id,
    string Name,
    string Symbol,
    string Exchange,
    DateTime ExpirationAt
);
