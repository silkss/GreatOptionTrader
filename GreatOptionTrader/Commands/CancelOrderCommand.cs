using Connectors.IB;
using GreatOptionTrader.ViewModels;

namespace GreatOptionTrader.Commands;
public class CancelOrderCommand (InteractiveBroker broker) : Base.Command {
    public override bool CanExecute (object? parameter) => broker.IsConnected()
        && parameter is OptionStrategyViewModel ivm
        && ivm.OpenOrder != null;

    public override void Execute (object? parameter) {
        if (parameter is not OptionStrategyViewModel ivm) {
            return;
        }
        var order = ivm.OpenOrder;

        if (order == null) {
            return;
        }

        broker.CancelOrder(order.BrokerId);
    }
}
