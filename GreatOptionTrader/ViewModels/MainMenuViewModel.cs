using Core.Connectors.InteractiveBrokers;
using System.Windows.Input;

namespace GreatOptionTrader.ViewModels;

internal class MainMenuViewModel {
    public ICommand Connect { get; }
    
    public MainMenuViewModel (Connector connector) {
        Connect = new Commands.ConnectCommand(connector);
    }
}
