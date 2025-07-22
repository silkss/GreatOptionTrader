using GreatOptionTrader.Services.Repositories;
using GreatOptionTrader.Views;

namespace GreatOptionTrader.Commands;
public class CreateGroupCommand (InstrumentGroupRepo repository) : Base.Command {
    public override bool CanExecute (object? parameter) => true;

    public override void Execute (object? parameter) {
        var dialog = new CreateInstrumentGroupView(repository);
        dialog.ShowDialog();
    }
}
