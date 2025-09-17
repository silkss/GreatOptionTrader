using Connectors.IB;
using Core;
using GreatOptionTrader.Models;
using GreatOptionTrader.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace GreatOptionTrader.ViewModels;

public class OptionStrategyContainerViewModel : BaseOptionStrategyViewModel {
    private readonly InteractiveBroker broker;
    private readonly Dispatcher dispatcher;
    private Task? pnlUpdaterWorker;

    private async Task makePnLUpdaterWorker() {
        while (IsStarted) {

            if (Container.State == Types.ContainerState.Created)
            {
                UpdatePnL();
                if (CurrencyOpenPnL >= ContainerSettings.CurrencyTargetPnL)
                {
                    Close(broker);
                    Container.State = Types.ContainerState.Closing;
                }
            }
            else if (Container.State == Types.ContainerState.Closing)
            {
                bool isClosed = false;

                isClosed = Position.CurrentVolume == 0m;
                foreach (var strategy in OptionStrategies)
                {
                    isClosed = isClosed && (strategy.Position.CurrentVolume == 0m);
                }

                if (isClosed)
                {
                    Container.State = Types.ContainerState.Closed;
                    IsStarted = false;
                    RaisePropertyChanged(nameof(IsStarted));
                }
            }
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
        OptionStrategies = [.. container.Strategies.Select(createOptionStrategyViewModel)];
        UpdatePnL();
    }

    public bool IsStarted { get; set; }
    public ContainerSettings ContainerSettings => Container.Settings;
    public override ICollection<Order> Orders => Container.Orders;

    public ObservableCollection<OptionStrategyViewModel> OptionStrategies { get; }

    public OptionStrategiesContainer Container { get; }

    public override Option Instrument => Container.Instrument;

    public bool CanStarted() => !IsStarted && Container.State != Types.ContainerState.Closed;
    
    public void Start (InteractiveBroker broker) {
        if (IsStarted) return;
        if (Container.State == Types.ContainerState.Closed) return;
        
        broker.SubscribeOnMarketData(this);

        foreach (var optionStrategy in OptionStrategies)
        {
            broker.SubscribeOnMarketData(optionStrategy);
        }

        IsStarted = true;
        RaisePropertyChanged(nameof(IsStarted));
        pnlUpdaterWorker = makePnLUpdaterWorker();
    }
    
    public void AddStrategy(OptionStrategy strategy) {
        Container.Strategies.Add(strategy);
        var vm = createOptionStrategyViewModel(strategy);

        if (IsStarted)
        {
            broker.SubscribeOnMarketData(vm);
        }
        
        dispatcher.Invoke(() => {
            OptionStrategies.Add(vm);
        });
    }

    public override void UpdatePnL () {
        base.UpdatePnL();
        decimal hedgeOpenPnL = 0m;
        decimal hedgeTotalPnL = 0m;

        foreach (var strategy in OptionStrategies) {
            strategy.UpdatePnL();
            hedgeOpenPnL += strategy.CurrencyOpenPnL;
            hedgeTotalPnL += strategy.CurrencyTotalPnL;
            strategy.RaisePropertyChange();
        }

        CurrencyOpenPnL += hedgeOpenPnL;
        CurrencyTotalPnL += hedgeTotalPnL;
        RaisePropertyChange();
    }
    
    public void Close(InteractiveBroker broker) {
        if (Container.State is Types.ContainerState.Closing or Types.ContainerState.Closed)
        {
            return;
        }

        Close(broker, Container.Account);

        foreach (var strategy in OptionStrategies) {
            strategy.Close(broker, Container.Account);
        }
    }
}
