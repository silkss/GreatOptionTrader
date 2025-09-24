using Core.Commands.Base;
using Hedger.Services;

namespace Hedger.Commands;
public class SaveAllContainersCommand (ContainersRepository repository) : Command {
    public override bool CanExecute (object? parameter) => true;
    public override void Execute (object? parameter) => repository.UpdateAll();
}
