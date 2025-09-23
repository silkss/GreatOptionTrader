using System;

namespace Core.Types;

public class Order : Base.ObservableObject{
    private OrderStatus status;

    public bool IsCompleted = false;

    public required TradeDirection Direction { get; init; }
    public required decimal Quantity { get; init; }
    public required decimal LimitPrice { get; set; }
    public required string Account { get; init; }
    public required int BrokerId { get; set; }
    public required DateTime CreatedTime { get; init; }

    public decimal FilledVolume { get; set; }
    public OrderStatus Status { get => status; set => Set(ref status, value); }
    public decimal AverageFilledPrice { get; set; }
    public decimal Commission { get; set; }

    public int InstrumentId { get; set; }

    /// <summary>
    /// Какая то фигня в ордере, которая, вроде как уникальна.
    /// по идеи с ее помощью можно отлавливать ордера, которые были исполнены 
    /// в момент перезапуска приложения.
    /// </summary>
    public int PermId { get; set; }

    public decimal GetPositionPrice () => FilledVolume * AverageFilledPrice;
}
