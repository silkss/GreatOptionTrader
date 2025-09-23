using Connectors.IB;
using Core.Commands.Base;
using GreatOptionTrader.Services.Repositories;
using GreatOptionTrader.Views;

namespace GreatOptionTrader.Commands;
public class CreateGroupCommand (InteractiveBroker broker, OptionStrategiesContainersRepository repository) : Command {
    public override bool CanExecute (object? parameter) => broker.IsConnected();

    public override void Execute (object? parameter) {
        var dialog = new CreateStrategyContainerView(broker, repository);
        dialog.ShowDialog();
    }
}
