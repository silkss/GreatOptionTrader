using Connectors.IB;
using Hedger.Services;
using Microsoft.Extensions.Logging;
using System.Windows;

namespace Hedger;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static void ShowErrorMessage(string message) => MessageBox
        .Show(message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);

    private readonly string containerFolder = "containers";
    private readonly InteractiveBroker broker;
    private readonly ILoggerFactory loggerFactory;
    private readonly ContainersJsonLoader loader;
    private readonly ContainersRepository repository;

    public App () {
        loggerFactory = LoggerFactory.Create(builder => builder.AddDebug());
        broker = new InteractiveBroker(loggerFactory.CreateLogger<InteractiveBroker>());
        loader = new ContainersJsonLoader(containerFolder);
        repository = new ContainersRepository(loader, broker, Dispatcher);
    }

    protected override void OnStartup (StartupEventArgs e) {
        MainWindow = new Views.MainView() {
            DataContext = new ViewModels.MainViewModel(broker, repository)
        };

        MainWindow.Show();
        base.OnStartup(e);
    }

    protected override void OnExit (ExitEventArgs e) {
        repository.UpdateAll();
        base.OnExit(e);
    }
}

