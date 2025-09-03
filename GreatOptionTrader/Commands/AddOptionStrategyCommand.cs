using Connectors.IB;
using GreatOptionTrader.Models;
using GreatOptionTrader.ViewModels;
using GreatOptionTrader.Views;

namespace GreatOptionTrader.Commands;
public class AddOptionStrategyCommand (InteractiveBroker broker) : Base.Command {
    public override bool CanExecute (object? parameter) => broker.IsConnected()
        && parameter is OptionStrategyContainerViewModel;

    public override void Execute (object? parameter) {
        if (parameter is not OptionStrategyContainerViewModel container) {
            return;
        }

        var dialog = new RequestOptionView(broker);

        if (dialog.ShowDialog() == true && dialog.SelectedOption != null) {
            var strategy = new OptionStrategy() {
                Instrument = dialog.SelectedOption,
                Orders = []
            };

            container.AddStrategy(strategy);
        }
    }
}
