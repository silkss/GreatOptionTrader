using GreatOptionTrader.DTO;
using GreatOptionTrader.Services.Connectors;

namespace GreatOptionTrader.Commands;
public class AddInstrumentToGroupCommand (InteractiveBroker broker) : Base.Command {
    public override bool CanExecute (object? parameter) {
        return parameter is InstrumentToGroupDTO
            && broker.IsConnected();
    }

    public override void Execute (object? parameter) {
        if (parameter is not InstrumentToGroupDTO instrumentToGroupDTO) {
            return;
        }

        broker.RequestOptions(instrumentToGroupDTO.InstrumentRequestDTO);
    }
}
