using GreatOptionTrader.Types;
using System;

namespace GreatOptionTrader.Models;
public class Order {
    
    public DateTime CreatedDateTime { get; init; } = DateTime.Now;

    public required int OrderId { get; init; }
    public required double LimitPrice{ get; set; }
    public required decimal Volume { get; init; }
    public required TradeDirection Direction { get; init; }

    public double AverageFilledPrice { get; set; }
    public double FilledVolume { get; set; }
}
