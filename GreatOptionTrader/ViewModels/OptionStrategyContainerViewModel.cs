using Connectors.IB;
using Core;
using GreatOptionTrader.Models;
using GreatOptionTrader.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace GreatOptionTrader.ViewModels;

public class OptionStrategyContainerViewModel : BaseOptionStrategyViewModel {
    private readonly InteractiveBroker broker;
    private readonly Dispatcher dispatcher;
    private string? requestedOptionName;
    private string? requestedOptionExchange;
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
        Dispatcher dispatcher) : base(container.Orders) {
        this.dispatcher = dispatcher;
        Container = container;
        this.broker = broker;
        this.OptionStrategies = new ObservableCollection<OptionStrategyViewModel>(container.Strategies.Select(createOptionStrategyViewModel));
    }

    public bool IsStarted { get; set; }
    public decimal CurrencyOpenPnL { get; private set; }
    public decimal CurrencyFixedPnL { get; private set; }
    public decimal CurrencyTotalPnL { get; private set; }

    public ObservableCollection<OptionStrategyViewModel> OptionStrategies { get; }

    public string? RequestedOptionName {
        get => requestedOptionName;
        set {
            if (requestedOptionName != value) {
                requestedOptionName = value;
                RaisePropertyChanged(nameof(RequestedOptionName));
            }
        }
    }
    public string? RequestedOptionExchange {
        get => requestedOptionExchange;
        set {
            if (requestedOptionExchange != value) {
                requestedOptionExchange = value;
                RaisePropertyChanged(nameof(RequestedOptionExchange));
            }
        }
    }

    public OptionStrategiesContainer Container { get; }

    public override Option Instrument => Container.Instrument;

    public void Start (InteractiveBroker broker) {
        if (IsStarted) { return; }
        broker.SubscribeOnMarketData(this);

        foreach (var optionStrategy in OptionStrategies) {
            broker.SubscribeOnMarketData(optionStrategy);
        }

        IsStarted = true;
        RaisePropertyChanged(nameof(IsStarted));
        pnlUpdaterWorker = makePnLUpdaterWorker();
    }
    public void AddStrategy(OptionStrategy strategy) {
        Container.Strategies.Add(strategy);
        var vm = createOptionStrategyViewModel(strategy);
        dispatcher.Invoke(() => {
            OptionStrategies.Add(vm);
        });
    }
    public void UpdatePnL () {
        decimal currencyOpenPnl = 0m, currencyFixedPnL = 0m, currencyTotalPnL;

        foreach (var ivm in OptionStrategies) {
            currencyOpenPnl += ivm.Position.CurrencyOpenPnL;
            currencyFixedPnL += ivm.Position.CurrencyFixedPnL;
        }

        if (CurrencyOpenPnL != currencyOpenPnl) {
            CurrencyOpenPnL = currencyOpenPnl;
            RaisePropertyChanged(nameof(CurrencyOpenPnL));
        }

        if (CurrencyFixedPnL != currencyFixedPnL) {
            CurrencyFixedPnL = currencyFixedPnL;
            RaisePropertyChanged(nameof(CurrencyFixedPnL));
        }

        currencyTotalPnL = CurrencyOpenPnL + CurrencyFixedPnL;
        if (CurrencyTotalPnL != currencyTotalPnL) {
            CurrencyTotalPnL = currencyTotalPnL;
            RaisePropertyChanged(nameof(CurrencyTotalPnL));
        }
    }
}
