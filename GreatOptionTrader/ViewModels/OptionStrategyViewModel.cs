using Connectors.IB;
using Core.Types;
using GreatOptionTrader.Models;
using GreatOptionTrader.ViewModels.Base;
using System.Collections.Generic;

namespace GreatOptionTrader.ViewModels;

public class OptionStrategyViewModel : BaseOptionStrategyViewModel{
    public OptionStrategyViewModel (
        OptionStrategy strategy, 
        InteractiveBroker broker) : base(broker, strategy.Orders) {
        Strategy = strategy;
    }

    public override Option Instrument => Strategy.Instrument;
    public override ICollection<Order> Orders => Strategy.Orders;

    public OptionStrategy Strategy { get; }
}