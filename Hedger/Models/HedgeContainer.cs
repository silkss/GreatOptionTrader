using Core.Types;
using System.Linq;
using System.Collections.Generic;

namespace Hedger.Models;

public class HedgeContainer {
    private List<HedgeLevel> buyLevels = [];
    private List<HedgeLevel> sellLevels = [];

    public required Future Basis { get; init; }
    public required List<HedgeLevel> BuyLevels {
        get => buyLevels;
        set {
            buyLevels = [.. value.OrderByDescending(level => level.ActivatePrice)];
        }
    }

    public required List<HedgeLevel> SellLevels {
        get => sellLevels;
        set {
            sellLevels = [.. value.OrderByDescending(level => level.ActivatePrice)];
        }
    }
}
