using GreatOptionTrader.Services.Connectors;
using GreatOptionTrader.ViewModels;

namespace GreatOptionTrader.Commands;
public class StartGroupCommand (InteractiveBroker broker) : Base.Command {
    public override bool CanExecute (object? parameter) => broker.IsConnected()
        && parameter is GroupViewModel group
        && !group.IsStarted;

    public override void Execute (object? parameter) {
        if (parameter is not GroupViewModel group) {
            return;
        }
        if (group.IsStarted) { return; }

        foreach (var instrumentViewModel in group.Instruments) {
            broker.AddInstrumentToCache(instrumentViewModel);
        }

        group.IsStarted = true;
    }
}
