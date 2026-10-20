namespace GreatOptionTrader.Commands;

internal class ConnectCommand (
    Core.Connectors.InteractiveBrokers.Connector connector
) : Base.Command {
    private readonly Core.Connectors.InteractiveBrokers.Connector _connector = connector;

    public override bool CanExecute (object? parameter) => !_connector.IsConnected();
    public override void Execute (object? parameter) {
        _connector.Connect("127.0.0.1", 7497, 1000);
    }
}
