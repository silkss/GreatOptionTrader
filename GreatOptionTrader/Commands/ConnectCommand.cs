using GreatOptionTrader.Services.Connectors;

namespace GreatOptionTrader.Commands;
public class ConnectCommand (InteractiveBroker broker) : Base.Command {
    public override bool CanExecute (object? parameter) => !broker.IsConnected();
    public override void Execute (object? parameter) {
        broker.Connect(2);
    }
}
