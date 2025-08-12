using GreatOptionTrader.Models;
using GreatOptionTrader.ViewModels;
using GreatOptionTrader.Services.Connectors;
using Microsoft.Extensions.Logging;
using System.Windows;
using GreatOptionTrader.Services.Repositories;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace GreatOptionTrader;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application {
    private readonly InteractiveBroker broker;
    private readonly InstrumentGroupRepository instrumentGroupRepository;
    private readonly InstrumentRepository instrumentRepository;
    private readonly OrdersRepository ordersRepository;
    private readonly ILoggerFactory loggerFactory;
    private readonly ObservableCollection<string> accounts;

    public App () {
        accounts = [];
        loggerFactory = LoggerFactory.Create(b => b.AddDebug());
        instrumentRepository = new();
        ordersRepository = new();

        broker = new InteractiveBroker(
            loggerFactory.CreateLogger<InteractiveBroker>(),
            accounts,
            Dispatcher);

        instrumentGroupRepository = new(
            Dispatcher, 
            broker, 
            instrumentRepository,
            ordersRepository);

    }

    protected override void OnStartup (StartupEventArgs e) {
        var mainViewModel = new MainViewModel(broker, instrumentGroupRepository, accounts);
        MainWindow = new Views.MainView() {
            DataContext = mainViewModel
        };

        MainWindow.Show();
        base.OnStartup(e);
    }

    protected override void OnExit (ExitEventArgs e) {
        instrumentGroupRepository.UpdateAll();
        base.OnExit(e);
    }
}

