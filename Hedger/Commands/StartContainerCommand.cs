using Connectors.IB;
using Core.Commands.Base;
using Hedger.ViewModels;

namespace Hedger.Commands;
public class StartContainerCommand (InteractiveBroker broker) : Command {
    public override bool CanExecute (object? parameter) => broker.IsConnected()
        && parameter is ContainerViewModel;

    public override void Execute (object? parameter) {
        if (parameter is not ContainerViewModel container) {
            return;
        }

        container.Start();
    }
}
