using GreatOptionTrader.ViewModels;
using Microsoft.Extensions.Logging;
using System.Windows;
using GreatOptionTrader.Services.Repositories;
using System.Collections.ObjectModel;
using Connectors.IB;
using GreatOptionTrader.Services;

namespace GreatOptionTrader;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application {
    const string FolderName = "GreatOptionTrader";

    private readonly InteractiveBroker broker;
    private readonly JsonOptionStrategiesContainerLoader loader;
    private readonly OptionStrategiesContainersRepository optionStrategiesContainersRepository;
    private readonly ILoggerFactory loggerFactory;
    private readonly ObservableCollection<string> accounts;

    public App () {
        accounts = [];
        loggerFactory = LoggerFactory.Create(b => b.AddDebug());
        broker = new InteractiveBroker(
            loggerFactory.CreateLogger<InteractiveBroker>());
        loader = new JsonOptionStrategiesContainerLoader(FolderName);
        optionStrategiesContainersRepository = new OptionStrategiesContainersRepository(
            broker,
            loader,
            Dispatcher);
    }

    protected override void OnStartup (StartupEventArgs e) {
        var mainViewModel = new MainViewModel(broker, optionStrategiesContainersRepository);
        MainWindow = new Views.MainView() {
            DataContext = mainViewModel
        };

        MainWindow.Show();
        base.OnStartup(e);
    }

    protected override void OnExit (ExitEventArgs e) {
        optionStrategiesContainersRepository.UpdateAll();
        base.OnExit(e);
    }
}

