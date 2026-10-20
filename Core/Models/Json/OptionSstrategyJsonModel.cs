using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models.Json;

public record OptionSstrategyJsonModel (
    InstrumentJsonModel Instrument,
    string Account
);
