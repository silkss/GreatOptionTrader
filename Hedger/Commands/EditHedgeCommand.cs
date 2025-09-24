using Core.Commands.Base;
using Hedger.ViewModels;
using System;

namespace Hedger.Commands;
public class EditHedgeCommand : Command {
    public override bool CanExecute (object? parameter) => parameter is ContainerViewModel;
    public override void Execute (object? parameter) {
        if (parameter is not ContainerViewModel container) { return; }
        var view = new Views.EditHedgeContainerView() {
            DataContext = container.Hedge,
        };

        if (view.ShowDialog() == true) { }
    }
}
