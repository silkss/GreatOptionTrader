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
        StartGroupCommand = new StartGroupCommand(broker);

        this.optionStrategiesContainersRepository = optionStrategiesContainersRepository;
        CreateGroup = new CreateGroupCommand(optionStrategiesContainersRepository);
        EditInstrumentGroup = new EditOptionStrategiesContainerCommand(broker);
    }

    public ConnectCommand Connect { get; } 
    public StartGroupCommand StartGroupCommand { get; } 

    public CreateGroupCommand CreateGroup { get; }
    public ObservableCollection<OptionStrategyContainerViewModel> Containers => optionStrategiesContainersRepository.ContainerViewModels;
    public EditOptionStrategiesContainerCommand EditInstrumentGroup { get; }
}
