using Connectors.IB;
using GreatOptionTrader.ViewModels;

namespace GreatOptionTrader.Commands;
public class CloseContainerCommand (InteractiveBroker broker) : Base.Command {
    public override bool CanExecute (object? parameter) => broker.IsConnected()
        && parameter is OptionStrategyContainerViewModel container
        && container.IsStarted;

    public override void Execute (object? parameter) {
        if (parameter is not OptionStrategyContainerViewModel container) {
            return;
        }

        container.Close(broker);
    }
}
