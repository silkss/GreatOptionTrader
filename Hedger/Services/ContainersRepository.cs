using Connectors.IB;
using Hedger.Models;
using Hedger.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Threading;

namespace Hedger.Services;
public class ContainersRepository {
    private readonly ContainersJsonLoader loader;
    private readonly InteractiveBroker broker;
    private readonly Dispatcher dispatcher;

    private ContainerViewModel toViewModel (Container container) => new(container, broker);

    public ContainersRepository(ContainersJsonLoader loader, InteractiveBroker broker, Dispatcher dispatcher) {
        this.loader = loader;
        this.broker = broker;
        this.dispatcher = dispatcher;

        Containers = [.. loader.LoadAll().Select(toViewModel)];
    }

    public ObservableCollection<ContainerViewModel> Containers { get; }

    public void Create(Container container) {
        loader.Save(container);
        dispatcher.Invoke(() => {
            Containers.Add(toViewModel(container));
        });
    }

    public void UpdateAll () {
        foreach (var container in Containers) {
            loader.Save(container.Container);
        }
    }
}
