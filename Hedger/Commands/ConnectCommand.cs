using Connectors.IB;
using Core.Commands.Base;

namespace Hedger.Commands;

public class ConnectCommand (InteractiveBroker broker) : Command {
    public override bool CanExecute (object? parameter) => !broker.IsConnected();
    public override void Execute (object? parameter) {
        broker.Connect(112);
    }
}
