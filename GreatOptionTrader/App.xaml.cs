using GreatOptionTrader.Models;
using GreatOptionTrader.ViewModels;
using GreatOptionTrader.Services.Connectors;
using Microsoft.Extensions.Logging;
using System.Windows;
using GreatOptionTrader.Services.Repositories;

namespace GreatOptionTrader;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application {
    private readonly InteractiveBroker broker;
    private readonly InstrumentGroupRepo instrumentGroupRepository;
    private readonly InstrumentRepository instrumentRepository;
    private readonly ILoggerFactory loggerFactory;

    public App () {
        loggerFactory = LoggerFactory.Create(b => b.AddDebug());

        instrumentGroupRepository = new(Dispatcher);
        instrumentRepository = new(Dispatcher);

        broker = new InteractiveBroker(
            instrumentRepository,
            loggerFactory.CreateLogger<InteractiveBroker>());
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
        broker.Disconnect();
        base.OnExit(e);
    }
}

