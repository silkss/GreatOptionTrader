using GreatOptionTrader.Views;
using GreatOptionTrader.ViewModels;
using System.Collections.ObjectModel;
using Connectors.IB;

namespace GreatOptionTrader.Commands;
public class EditOptionStrategiesContainerCommand (InteractiveBroker broker) : Base.Command {
    public override bool CanExecute (object? parameter) =>
        broker.IsConnected() &&
        parameter is OptionStrategyContainerViewModel;

    public override void Execute (object? parameter) {
        if (parameter is not OptionStrategyContainerViewModel viewModel) {
            return;
        }

        var view = new EditInstrumentGroupView(
            viewModel, broker);

        view.ShowDialog();
    }
}
