using Connectors.IB;
using Core.Commands.Base;
using Hedger.Exceptions;
using Hedger.Models;
using Hedger.Services;
using System;
using System.Windows;

namespace Hedger.Commands;
public class CreateContainerCommand (InteractiveBroker broker, ContainersRepository repository) : Command {
    public override bool CanExecute (object? parameter) => broker.IsConnected();
    public override void Execute (object? parameter) {
        var viewModel = new ViewModels.CreateContainerViewModel(broker);
        var view = new Views.CreateContainerView() {
            DataContext = viewModel
        };

        if (view.ShowDialog() == true) {
            try {
                repository.Create(viewModel.CreteContainer());
            }
            catch (ContainerCreationError error) {
                App.ShowErrorMessage(error.Message);
            }
        }
    }
}
