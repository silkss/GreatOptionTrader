using Connectors.IB;
using Core.Commands.Base;
using GreatOptionTrader.ViewModels;
using GreatOptionTrader.Views;

namespace GreatOptionTrader.Commands;
public class EditHedgeCommand(InteractiveBroker broker) : Command {

    public override bool CanExecute (object? parameter) => broker.IsConnected()
        && parameter is OptionStrategyContainerViewModel;

    public override void Execute (object? parameter) {
        if (parameter is not OptionStrategyContainerViewModel container) {
            return;
        }

        var dialog = new EditHedgeView(broker, container);
        if (dialog.ShowDialog() == true) { }
    }
}
