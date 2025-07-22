using GreatOptionTrader.Models;
using GreatOptionTrader.Services.Connectors;

namespace GreatOptionTrader.Commands;
public class CacheOptionCommand ( InteractiveBroker broker) : Base.Command {
    public override bool CanExecute (object? parameter) =>
        broker.IsConnected() &&
        parameter is Instrument;

    public override void Execute (object? parameter) {
        if (parameter is not Instrument instrument) {
            return;
        }

        broker.AddInstrumentToCache(instrument);
    }
}
