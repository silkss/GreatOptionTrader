using GreatOptionTrader.Models;
using GreatOptionTrader.Services.Connectors;
using GreatOptionTrader.ViewModels;

namespace GreatOptionTrader.Commands;
public class SendOrderCommand (InteractiveBroker broker) : Base.Command {
    public override bool CanExecute (object? parameter) => broker.IsConnected()
        && parameter is InstrumentViewModel ivm
        && ivm.WantedPrice > 0.0
        && ivm.WantedVolume > 0m;

    public override void Execute (object? parameter) {
        if (parameter is not InstrumentViewModel ivm) {
            return;
        }

        var order = new Order() {
            Direction = ivm.WantedDirection,
            LimitPrice = ivm.WantedPrice,
            Volume = ivm.WantedVolume,
            OrderId = broker.GetValidOrderId()
        };

        broker.PlaceOrder(ivm.Instrument, order);
    }
}
