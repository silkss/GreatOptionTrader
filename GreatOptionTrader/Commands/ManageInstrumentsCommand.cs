using GreatOptionTrader.Services.Connectors;
using GreatOptionTrader.ViewModels;
using GreatOptionTrader.Views;

namespace GreatOptionTrader.Commands;
public class ManageInstrumentsCommand (InteractiveBroker broker) : Base.Command {
    public override bool CanExecute (object? parameter) => broker.IsConnected();
    public override void Execute (object? parameter) {

        var viewModel = new ManageInstrumentsViewModel(broker);
        var dialog = new InstrumentManagerView() {
            DataContext = viewModel
        };

        dialog.ShowDialog();
    }
}
