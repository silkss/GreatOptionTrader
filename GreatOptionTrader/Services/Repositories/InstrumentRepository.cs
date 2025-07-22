using GreatOptionTrader.Models;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace GreatOptionTrader.Services.Repositories;
public class InstrumentRepository {
    private readonly Dispatcher dispatcher;

    public InstrumentRepository(Dispatcher dispatcher) {
        Items = new();
        this.dispatcher = dispatcher;
    }

    public ObservableCollection<Instrument> Items { get; }
    public int Create(Instrument instrument) {
        int tickerId = Items.Count;
        dispatcher.Invoke(() => {
            Items.Add(instrument);
        });
        return tickerId;
    }
}
