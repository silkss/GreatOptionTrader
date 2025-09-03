using Connectors.IB;
using GreatOptionTrader.ViewModels;

namespace GreatOptionTrader.Commands;
public class StartOptionStrategiesContainerCommand (InteractiveBroker broker) : Base.Command {
    public override bool CanExecute (object? parameter) => broker.IsConnected()
        && parameter is OptionStrategyContainerViewModel group
        && !group.IsStarted;

    public override void Execute (object? parameter) {
        if (parameter is not OptionStrategyContainerViewModel container) {
            return;
        }
        container.Start(broker);
    }
}
