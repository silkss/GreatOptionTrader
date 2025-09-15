using Connectors.IB;
using Core;
using GreatOptionTrader.Models;
using GreatOptionTrader.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace GreatOptionTrader.ViewModels;

public class OptionStrategyContainerViewModel : BaseOptionStrategyViewModel {
    private readonly InteractiveBroker broker;
    private readonly Dispatcher dispatcher;
    private Task? pnlUpdaterWorker;

    private async Task makePnLUpdaterWorker() {
        while (IsStarted) {
            UpdatePnL();
            await Task.Delay(2000);
        }
    }

    private OptionStrategyViewModel createOptionStrategyViewModel(OptionStrategy strategy) {
        return new OptionStrategyViewModel(
            strategy: strategy, 
            broker: broker);
    }

    public OptionStrategyContainerViewModel (
        OptionStrategiesContainer container,
        InteractiveBroker broker,
        Dispatcher dispatcher) : base(broker, container.Orders) {
        this.dispatcher = dispatcher;
        Container = container;
        this.broker = broker;
        OptionStrategies = new ObservableCollection<OptionStrategyViewModel>(container.Strategies.Select(createOptionStrategyViewModel));
    }

    public bool IsStarted { get; set; }
    public decimal CurrencyFixedPnL { get; private set; }
    public decimal CurrencyTotalPnL { get; private set; }
    public ContainerSettings ContainerSettings => Container.Settings;
    public override ICollection<Order> Orders => Container.Orders;

    public ObservableCollection<OptionStrategyViewModel> OptionStrategies { get; }

    public OptionStrategiesContainer Container { get; }

    public override Option Instrument => Container.Instrument;

    public void Start (InteractiveBroker broker) {
        if (IsStarted) return;         broker.SubscribeOnMarketData(this);

        foreach (var optionStrategy in OptionStrategies)             broker.SubscribeOnMarketData(optionStrategy);

        IsStarted = true;
        RaisePropertyChanged(nameof(IsStarted));
        pnlUpdaterWorker = makePnLUpdaterWorker();
    }
    
    public void AddStrategy(OptionStrategy strategy) {
        Container.Strategies.Add(strategy);
        var vm = createOptionStrategyViewModel(strategy);
        
        if (IsStarted)             broker.SubscribeOnMarketData(vm);
        
        dispatcher.Invoke(() => {
            OptionStrategies.Add(vm);
        });
    }

    public override void UpdatePnL () {
        base.UpdatePnL();
        decimal hedgeOpenPnL = 0m;
        decimal totalPnL = 0m;

        foreach (var strategy in OptionStrategies) {
            strategy.UpdatePnL();
            hedgeOpenPnL += strategy.CurrencyOpenPnL;
        }

        totalPnL = CurrencyOpenPnL + hedgeOpenPnL;

        if (CurrencyTotalPnL != totalPnL) {
            CurrencyTotalPnL = totalPnL;
            RaisePropertyChanged(nameof(CurrencyTotalPnL));
        }
    }
    
    public void Close(InteractiveBroker broker) {
        Close(broker, Container.Account);

        foreach (var strategy in OptionStrategies) {
            strategy.Close(broker, Container.Account);
        }
    }
}
