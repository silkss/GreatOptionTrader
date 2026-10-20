using System;

namespace Core.Domain;

internal class OptionStrategy {
    public OptionStrategy (
        Guid id,
        Instrument instrument,
        string account
    ) {
        Id = id;
        Instrument = instrument;
        Account = account;
    }

    public Guid Id { get; }
    public Instrument Instrument { get; }
    public string Account { get; }
}
