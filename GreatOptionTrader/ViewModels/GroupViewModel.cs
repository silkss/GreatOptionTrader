using GreatOptionTrader.Models;
using GreatOptionTrader.Services.Connectors;
using GreatOptionTrader.Services.Repositories;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace GreatOptionTrader.ViewModels;

public class GroupViewModel : Base.ObservableObject {
    private readonly InteractiveBroker broker;
    private readonly InstrumentRepository instrumentRepository;
    private readonly OrdersRepository ordersRepository;
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

    private InstrumentViewModel createInstrumentViewModel(OptionModel instrument) {
        return new InstrumentViewModel(
            instrument: instrument,
            ordersRepository: ordersRepository,
            broker: broker);
    }

    public GroupViewModel (
        InstrumentGroup group,
        InteractiveBroker broker,
        InstrumentRepository instrumentRepository,
        OrdersRepository ordersRepository,
        Dispatcher dispatcher) {
        this.dispatcher = dispatcher;
        this.Group = group;
        this.broker = broker;
        this.instrumentRepository = instrumentRepository;
        this.ordersRepository = ordersRepository;
        this.Instruments = new ObservableCollection<InstrumentViewModel>(group.Options.Select(createInstrumentViewModel));
        this.broker.ContractUpdated += OnInstrumentUpdated;
    }

    public bool IsStarted { get; set; }
    public decimal OpenPnL { get; private set; }
    public decimal FixedPnL { get; private set; }

    public InstrumentGroup Group { get; }
    public ObservableCollection<InstrumentViewModel> Instruments { get; }

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
    
    public void Start (InteractiveBroker broker) {
        if (IsStarted) { return; }

        foreach (var instrumentViewModel in Instruments) {
            broker.AddInstrumentToCache(instrumentViewModel);
        }

        IsStarted = true;
        RaisePropertyChanged(nameof(IsStarted));
        pnlUpdaterWorker = makePnLUpdaterWorker();
    }

    public void UpdatePnL () {
        decimal openPnl = 0m;
        decimal fixedPnL = 0m;
        foreach (var ivm in Instruments) {
            openPnl += ivm.Position.OpenPnL;
            fixedPnL += ivm.Position.FixedPnL;
        }

        if (OpenPnL != openPnl) {
            OpenPnL = openPnl;
            RaisePropertyChanged(nameof(OpenPnL));
        }

        if (FixedPnL != fixedPnL ) {
            FixedPnL = fixedPnL;
            RaisePropertyChanged(nameof(FixedPnL));
        }
    }

    public void OnInstrumentUpdated(int requestId, OptionModel option) {
        if (requestId != Group.Id) return;
        if (Instruments.Any(instrument => instrument.Instrument.Id == option.Id)) return;

        option.InstrumentGroupId = Group.Id;

        instrumentRepository.Add(option);

        var optionVM = createInstrumentViewModel(option);
        dispatcher.Invoke(() => Instruments.Add(optionVM));

        if (IsStarted) {
            broker.AddInstrumentToCache(optionVM);
        }
    }
}
