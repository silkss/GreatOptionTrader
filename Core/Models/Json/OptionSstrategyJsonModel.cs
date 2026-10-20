using Core.Abstractions;
using System;

namespace Core.Models.Json;

public record OptionSstrategyJsonModel (
    Guid Id,
    InstrumentJsonModel Instrument,
    string Account
) : IJsonModel;
