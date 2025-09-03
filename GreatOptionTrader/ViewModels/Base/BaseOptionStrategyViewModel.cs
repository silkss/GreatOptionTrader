using Connectors;
using Core;
using System.Collections.Generic;

namespace GreatOptionTrader.ViewModels.Base;
public abstract class BaseOptionStrategyViewModel : ObservableMarketDataObserver<Option> {
    private static Order? checkIfHaveOpenOrder (IEnumerable<Order> orders) {
        Order? openOrder = null;
        foreach (var order in orders) {
            if (order.Status == OrderStatus.Submitted) {
                openOrder = order;
            }
        }
        return openOrder;
    }

    protected BaseOptionStrategyViewModel (IEnumerable<Order> orders) {
        Position = new PositionViewModel(orders);
        OpenOrder = checkIfHaveOpenOrder(orders);
    }

    public PositionViewModel Position { get; }
    public Order? OpenOrder { get; set; }

    public void OnCompletedOrderUpdated (int permId, CompletedOrderEventArgument arg) {
        if (OpenOrder == null) return;
        if (OpenOrder.PermId != permId) return;

        OpenOrder.FilledVolume = arg.FilledVolume;
        OpenOrder.Status = arg.Status;

        if (OpenOrder.AverageFilledPrice == 0m) {
            OpenOrder.AverageFilledPrice = OpenOrder.LimitPrice;
        }

        Position.ProcessOrder(OpenOrder);
        OpenOrder = null;
    }

    public void UpdateOrderStatus (int id, OrderStatusEventArgument arg) {
        if (OpenOrder == null) return;
        if (OpenOrder.BrokerId != id) return;

        OpenOrder.Status = arg.Status;
        OpenOrder.AverageFilledPrice = arg.AverageFilledPrice;
        OpenOrder.FilledVolume = arg.FilledVolume;

        if (OpenOrder.Status == OrderStatus.Filled) {
            if (OpenOrder.IsCompleted) {
                Position.ProcessOrder(OpenOrder);
                OpenOrder = null;
                return;
            }
            OpenOrder.IsCompleted = true;
        }
        if (OpenOrder.Status == OrderStatus.Cancelled) OpenOrder = null;
    }

    public void UpdateOrderCommission (int orderId, CommissionUpdateEventArgs args) {
        if (OpenOrder == null) return;
        if (OpenOrder.BrokerId != orderId) return;

        OpenOrder.PermId = args.PermId;
        OpenOrder.Commission = args.Commission;
    }
}
