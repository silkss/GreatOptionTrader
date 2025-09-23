using Core.Types;

namespace GreatOptionTrader.ViewModels;
public class OrderParamsViewModel {
    public decimal Volume { get; set; }
    public decimal Price { get; set; }
    public TradeDirection Direction { get; set; }
}
