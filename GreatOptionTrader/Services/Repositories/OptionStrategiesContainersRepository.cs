using Connectors.IB;
using GreatOptionTrader.Models;
using GreatOptionTrader.Services.Loaders;
using GreatOptionTrader.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;

namespace GreatOptionTrader.Services.Repositories;
public class OptionStrategiesContainersRepository {
    private readonly Dispatcher dispatcher;
    private readonly InteractiveBroker broker;
    private readonly ILoader<OptionStrategiesContainer> loader;

    private OptionStrategyContainerViewModel makeViewModel (OptionStrategiesContainer container) {
        return new OptionStrategyContainerViewModel(
            container,
            broker,
            dispatcher);
    }

    public OptionStrategiesContainersRepository(
        InteractiveBroker broker, 
        ILoader<OptionStrategiesContainer> loader,
        Dispatcher dispatcher) {
        this.broker = broker;
        this.loader = loader;
        this.dispatcher = dispatcher;
        ContainerViewModels = new ObservableCollection<OptionStrategyContainerViewModel>(
            loader.LoadAll().Select(makeViewModel).ToList());
    }

    public void Create(OptionStrategiesContainer container) {
        loader.Save(container);
        dispatcher.Invoke(() => {
            ContainerViewModels.Add(makeViewModel(container));
        });
    }

    public void UpdateAll() {
        foreach (var containerViewModel in ContainerViewModels) {
            loader.Save(containerViewModel.Container);
        }
    }

    public ObservableCollection<OptionStrategyContainerViewModel> ContainerViewModels { get; }

}

