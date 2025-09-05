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
        AddOptionStrategyCommand = new AddOptionStrategyCommand(broker);
        StartAllContainers = new StartAllContainerCommand(broker, optionStrategiesContainersRepository.ContainerViewModels);
        OpenMasterPositionCommand = new OpenMasterPositionCommand(broker);
    }

    public ConnectCommand Connect { get; } 
    public StartOptionStrategiesContainerCommand StartContainerCommand{ get; } 
    public StartAllContainerCommand StartAllContainers { get; }
    public AddOptionStrategyCommand AddOptionStrategyCommand { get; }
    public CreateGroupCommand CreateGroup { get; }
    public OpenMasterPositionCommand OpenMasterPositionCommand { get; }
    public ObservableCollection<OptionStrategyContainerViewModel> Containers => optionStrategiesContainersRepository.ContainerViewModels;
}
