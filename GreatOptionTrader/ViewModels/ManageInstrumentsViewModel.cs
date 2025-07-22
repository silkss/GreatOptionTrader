using GreatOptionTrader.Commands;
using GreatOptionTrader.DTO;
using GreatOptionTrader.Models;
using GreatOptionTrader.Services.Connectors;
using System.ComponentModel;

namespace GreatOptionTrader.ViewModels;
public class ManageInstrumentsViewModel : INotifyPropertyChanged {
    public event PropertyChangedEventHandler? PropertyChanged;

    private readonly InteractiveBroker broker;

    public ManageInstrumentsViewModel (InteractiveBroker broker) {
        this.broker = broker;
        this.broker.OnContractDetailsRequested += onInstrumentRequested;
        InstrumentRequestDTO = new();
        RequestOption = new RequestOptionCommand(broker);
        CacheInstrument = new CacheOptionCommand(broker);
    }

    private void onInstrumentRequested (Instrument obj) {
        RequestedInstrument = obj;
        PropertyChanged?.Invoke(this, new(nameof(RequestedInstrument)));
    }

    public InstrumentRequestDTO InstrumentRequestDTO { get;  } 
    public RequestOptionCommand RequestOption { get; } 
    public CacheOptionCommand CacheInstrument { get; }
    public Instrument? RequestedInstrument { get; set; }
}
