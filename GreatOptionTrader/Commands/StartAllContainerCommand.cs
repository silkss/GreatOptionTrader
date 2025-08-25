using Connectors.IB;
using GreatOptionTrader.ViewModels;
using System.Collections.ObjectModel;

namespace GreatOptionTrader.Commands;
internal class StartAllContainerCommand (InteractiveBroker broker, ObservableCollection<OptionStrategyContainerViewModel> containers) : Base.Command {
    public override bool CanExecute (object? parameter) => broker.IsConnected();
    public override void Execute (object? parameter) {
        foreach( var container in containers) {
            container.Start(broker);
        }
    }
}