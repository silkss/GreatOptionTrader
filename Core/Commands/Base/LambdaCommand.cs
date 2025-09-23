using System;

namespace Core.Commands.Base;
public class LambdaCommand (Action<object?> action, Predicate<object?>? predicate) : Command {
    public override bool CanExecute (object? parameter) => predicate?.Invoke(parameter) ?? true;
    public override void Execute (object? parameter) => action.Invoke(parameter);
}
