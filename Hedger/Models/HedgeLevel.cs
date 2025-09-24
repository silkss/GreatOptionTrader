namespace Hedger.Models;
public class HedgeLevel {
    public HedgeLevelState State { get; set; }

    public decimal ActivatePrice { get; set; }
    public decimal Volume { get; set; } = 1m;
}
