using GreatOptionTrader.Services.Connectors;
using GreatOptionTrader.ViewModels;
using System;

namespace GreatOptionTrader.Commands;
public class SendOrderCommand (InteractiveBroker broker) : Base.Command {
    public override bool CanExecute (object? parameter) => broker.IsConnected()
        && parameter is InstrumentViewModel ivm
        && ivm.WantedPrice > 0m
        && ivm.WantedVolume > 0m
        && ivm.OpenOrder == null;

    public override void Execute (object? parameter) {
        if (parameter is not InstrumentViewModel ivm) {
            return;
        }

        if (ivm.WantedAccount == null) {
            return;
        }

        if (ivm.OpenOrder != null) {
            return;
        }

        var order = new Core.Order() {
            CreatedTime = DateTime.Now,
            Direction = ivm.WantedDirection,
            LimitPrice = ivm.WantedPrice,
            Account = ivm.WantedAccount,
            Quantity = ivm.WantedVolume,
            OrderId = broker.GetValidOrderId(),
            InstrumentId = ivm.Instrument.Id
        };

        ivm.AddOrder(order);
        
        broker.PlaceOrder(ivm.Instrument, order);
    }
}
