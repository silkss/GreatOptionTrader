using GreatOptionTrader.Views;
using GreatOptionTrader.ViewModels;
using System.Collections.ObjectModel;

namespace GreatOptionTrader.Commands;
public class EditInstrumentGroupCommand (
    ObservableCollection<string> accounts,
    SendOrderCommand sendOrderCommand,
    CancelOrderCommand cancelOrderCommand,
    RequestOptionCommand requestOptionCommand) : Base.Command {
    public override bool CanExecute (object? parameter) =>
        parameter is GroupViewModel;

    public override void Execute (object? parameter) {
        if (parameter is not GroupViewModel viewModel) {
            return;
        }

        var view = new EditInstrumentGroupView(
            viewModel, accounts, sendOrderCommand, 
            cancelOrderCommand,
            requestOptionCommand);

        view.ShowDialog();
    }
}
