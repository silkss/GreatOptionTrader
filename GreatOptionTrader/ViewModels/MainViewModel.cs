using Connectors.IB;
using GreatOptionTrader.Commands;
using GreatOptionTrader.Services.Repositories;
using System.Collections.ObjectModel;

namespace GreatOptionTrader.ViewModels;
public class MainViewModel {
    private readonly OptionStrategiesContainersRepository optionStrategiesContainersRepository;

    public MainViewModel (
        InteractiveBroker broker,
        OptionStrategiesContainersRepository optionStrategiesContainersRepository) {

        Connect = new ConnectCommand(broker);
        StartContainerCommand = new StartOptionStrategiesContainerCommand(broker);

        this.optionStrategiesContainersRepository = optionStrategiesContainersRepository;
        CreateGroup = new CreateGroupCommand(broker, optionStrategiesContainersRepository);
        StartAllContainers = new StartAllContainerCommand(broker, optionStrategiesContainersRepository.ContainerViewModels);
        OpenMasterPositionCommand = new OpenMasterPositionCommand(broker);
        CloseContainerCommand = new CloseContainerCommand(broker);
        EditHedgeCommand = new EditHedgeCommand(broker);
    }

    public ConnectCommand Connect { get; } 
    public StartOptionStrategiesContainerCommand StartContainerCommand{ get; } 
    public StartAllContainerCommand StartAllContainers { get; }
    public CreateGroupCommand CreateGroup { get; }
    public OpenMasterPositionCommand OpenMasterPositionCommand { get; }
    public CloseContainerCommand CloseContainerCommand { get; }
    public EditHedgeCommand EditHedgeCommand { get; }
    public ObservableCollection<OptionStrategyContainerViewModel> Containers => optionStrategiesContainersRepository.ContainerViewModels;
}
