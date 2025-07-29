using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatOptionTrader.Commands.Base;
public class LambdaCommand (Action<object?> action, Predicate<object?>? predicate) : Command {
    public override bool CanExecute (object? parameter) => predicate?.Invoke(parameter) ?? true;
    public override void Execute (object? parameter) => action.Invoke(parameter);
}
