using Connectors.IB;
using Core;
using GreatOptionTrader.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace GreatOptionTrader.ViewModels;

public class OptionStrategyContainerViewModel : Base.ObservableObject {
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

    private OptionStrategyViewModel createInstrumentViewModel(OptionStrategy strategy) {
        return new OptionStrategyViewModel(
            strategy: strategy, 
            broker: broker);
    }

    public OptionStrategyContainerViewModel (
        OptionStrategiesContainer container,
        InteractiveBroker broker,
        Dispatcher dispatcher) {
        this.dispatcher = dispatcher;
        Container = container;
        this.broker = broker;
        this.Instruments = new ObservableCollection<OptionStrategyViewModel>(container.Strategies.Select(createInstrumentViewModel));
        this.broker.OptionRequestedEvent += OnInstrumentUpdated;
    }

    public bool IsStarted { get; set; }
    public decimal CurrencyOpenPnL { get; private set; }
    public decimal FixedPnL { get; private set; }

    public ObservableCollection<OptionStrategyViewModel> Instruments { get; }

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

    public void Start (InteractiveBroker broker) {
        if (IsStarted) { return; }

        foreach (var instrumentViewModel in Instruments) {
            broker.SubscribeOnMarketData(instrumentViewModel);
        }

        IsStarted = true;
        RaisePropertyChanged(nameof(IsStarted));
        pnlUpdaterWorker = makePnLUpdaterWorker();
    }

    public void UpdatePnL () {
        decimal currencyOpenPnl = 0m;
        decimal fixedPnL = 0m;
        foreach (var ivm in Instruments) {
            currencyOpenPnl += ivm.Position.CurrencyOpenPnL;
            fixedPnL += ivm.Position.FixedPnL;
        }

        if (CurrencyOpenPnL != currencyOpenPnl) {
            CurrencyOpenPnL = currencyOpenPnl;
            RaisePropertyChanged(nameof(CurrencyOpenPnL));
        }

        if (FixedPnL != fixedPnL ) {
            FixedPnL = fixedPnL;
            RaisePropertyChanged(nameof(FixedPnL));
        }
    }

    public void OnInstrumentUpdated(int requestId, Option option) {
        if (GetHashCode() != requestId) return;
        if (Instruments.Any(instrument => instrument.Instrument.Id == option.Id)) return;

        var optionStrategy = new OptionStrategy() { Instrument = option, Orders = [] };
        Container.Strategies.Add(optionStrategy);

        var optionVM = createInstrumentViewModel(optionStrategy);
        dispatcher.Invoke(() => Instruments.Add(optionVM));

        if (IsStarted) {
            broker.SubscribeOnMarketData(optionVM);
        }
    }
}
