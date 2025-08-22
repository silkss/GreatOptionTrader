using Connectors.IB;
using GreatOptionTrader.ViewModels;
using System;

namespace GreatOptionTrader.Commands;
public class SendOrderCommand (InteractiveBroker broker) : Base.Command {
    public override bool CanExecute (object? parameter) => broker.IsConnected()
        && parameter is OptionStrategyViewModel ivm
        && ivm.WantedPrice > 0m
        && ivm.WantedVolume > 0m
        && ivm.OpenOrder == null;

    public override void Execute (object? parameter) {
        if (parameter is not OptionStrategyViewModel ivm) {
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
            BrokerId = broker.GetValidOrderId(),
            InstrumentId = ivm.Instrument.Id
        };

        ivm.AddOrder(order);
        
        broker.PlaceOrder(ivm.Instrument, order);
    }
}
