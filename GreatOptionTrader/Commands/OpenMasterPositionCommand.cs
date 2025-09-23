using Connectors.IB;
using Core.Commands.Base;
using GreatOptionTrader.ViewModels;
using GreatOptionTrader.ViewModels.Base;
using GreatOptionTrader.Views;

namespace GreatOptionTrader.Commands;
public class OpenMasterPositionCommand (InteractiveBroker broker) : Command {
    public override bool CanExecute (object? parameter) => broker.IsConnected()
        && parameter is BaseOptionStrategyViewModel;
        

    public override void Execute (object? parameter) {
        if (parameter is not OptionStrategyContainerViewModel container) {
            return;
        }

        var view = new OpenMasterPositionVIew(broker, container);
        if (view.ShowDialog() == true) {

        }
    }
}
