using System.Windows;

namespace Robin;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application {
    protected override void OnStartup (StartupEventArgs e) {
        MainWindow = new Views.MainView();
        MainWindow.Show();
        base.OnStartup(e);
    }
}
