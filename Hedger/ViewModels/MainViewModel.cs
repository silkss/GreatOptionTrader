using Connectors.IB;
using Hedger.Commands;
using Hedger.Services;
using System.Collections.ObjectModel;

namespace Hedger.ViewModels;
internal class MainViewModel(InteractiveBroker broker, ContainersRepository repository) {
    public ConnectCommand Connect { get; } = new ConnectCommand(broker);
    public CreateContainerCommand CreateContainer { get; } = new CreateContainerCommand(broker, repository);
    public StartContainerCommand StartContainer { get; } = new StartContainerCommand(broker);
    public SaveAllContainersCommand SaveContainers { get; } = new SaveAllContainersCommand(repository);
    public EditHedgeCommand EditHedgeCommand { get; } = new EditHedgeCommand();

    public ObservableCollection<ContainerViewModel> Containers => repository.Containers;
}
