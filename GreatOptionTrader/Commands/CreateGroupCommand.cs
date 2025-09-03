using Connectors.IB;
using GreatOptionTrader.Services.Repositories;
using GreatOptionTrader.Views;

namespace GreatOptionTrader.Commands;
public class CreateGroupCommand (InteractiveBroker broker, OptionStrategiesContainersRepository repository) : Base.Command {
    public override bool CanExecute (object? parameter) => broker.IsConnected();

    public override void Execute (object? parameter) {
        var dialog = new CreateStrategyContainerView(broker, repository);
        dialog.ShowDialog();
    }
}
