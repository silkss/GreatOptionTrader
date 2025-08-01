using GreatOptionTrader.Services.Connectors;
using GreatOptionTrader.ViewModels;

namespace GreatOptionTrader.Commands;
public class RequestOptionCommand(InteractiveBroker broker) : Base.Command {
    public override bool CanExecute (object? parameter) => broker.IsConnected()
        && parameter is GroupViewModel gvm
        && !string.IsNullOrEmpty(gvm.RequestedOptionName)
        && !string.IsNullOrEmpty(gvm.RequestedOptionExchange);

    public override void Execute (object? parameter) {
        if (parameter is not GroupViewModel gvm) {
            return;
        }
        if (string.IsNullOrEmpty(gvm.RequestedOptionName) ||
            string.IsNullOrEmpty(gvm.RequestedOptionExchange)) {

            // TODO: оповещение об ошибке.
            return;
        }

        broker.RequestOption(gvm.Group.Id, gvm.RequestedOptionName, gvm.RequestedOptionExchange);
    }
}
