using Connectors.IB;
using GreatOptionTrader.ViewModels;

namespace GreatOptionTrader.Commands;
public class RequestOptionCommand(InteractiveBroker broker) : Base.Command {
    public override bool CanExecute (object? parameter) => broker.IsConnected()
        && parameter is OptionStrategyContainerViewModel gvm
        && !string.IsNullOrEmpty(gvm.RequestedOptionName)
        && !string.IsNullOrEmpty(gvm.RequestedOptionExchange);

    public override void Execute (object? parameter) {
        if (parameter is not OptionStrategyContainerViewModel gvm) {
            return;
        }
        if (string.IsNullOrEmpty(gvm.RequestedOptionName) ||
            string.IsNullOrEmpty(gvm.RequestedOptionExchange)) {

            // TODO: оповещение об ошибке.
            return;
        }

        broker.RequestOption(gvm.GetHashCode() , gvm.RequestedOptionName, gvm.RequestedOptionExchange);
    }
}
