using GreatOptionTrader.Models;
using GreatOptionTrader.ViewModels;
using GreatOptionTrader.Services.Connectors;
using Microsoft.Extensions.Logging;
using System.Windows;
using GreatOptionTrader.Services.Repositories;
using System.Threading.Tasks;

namespace GreatOptionTrader;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application {
    private readonly InteractiveBroker broker;
    private readonly InstrumentGroupRepository instrumentGroupRepository;
    private readonly InstrumentRepository instrumentRepository;
    private readonly ILoggerFactory loggerFactory;

    public App () {
        loggerFactory = LoggerFactory.Create(b => b.AddDebug());
        instrumentRepository = new();

        broker = new InteractiveBroker(
            loggerFactory.CreateLogger<InteractiveBroker>());

        instrumentGroupRepository = new(Dispatcher, broker, instrumentRepository);

    }

    protected override void OnStartup (StartupEventArgs e) {
        var mainViewModel = new MainViewModel(broker, instrumentGroupRepository);
        MainWindow = new Views.MainView() {
            DataContext = mainViewModel
        };

        MainWindow.Show();
        base.OnStartup(e);
    }

    protected override void OnExit (ExitEventArgs e) {
        //broker.Disconnect();
        base.OnExit(e);
    }
}

