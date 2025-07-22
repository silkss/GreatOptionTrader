using GreatOptionTrader.DTO;
using GreatOptionTrader.Services.Connectors;

namespace GreatOptionTrader.Commands;
public class RequestOptionCommand (InteractiveBroker broker) : Base.Command {
    public override bool CanExecute (object? parameter) =>
        broker.IsConnected() &&
        parameter is InstrumentRequestDTO;
    

    public override void Execute (object? parameter) {
        if (parameter is not InstrumentRequestDTO request) {
            return;
        }
        broker.RequestOptions(request);
    }
}
