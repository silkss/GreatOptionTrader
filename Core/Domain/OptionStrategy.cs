namespace Core.Domain;

internal class OptionStrategy {
    public OptionStrategy (
        Instrument instrument,
        string account
    ) {
        Instrument = instrument;
        Account = account;
    }

    public Instrument Instrument { get; }
    public string Account { get; }
}
