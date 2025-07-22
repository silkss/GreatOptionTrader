using GreatOptionTrader.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;

namespace GreatOptionTrader.Services.Repositories;
public class InstrumentGroupRepo {
    private readonly Dispatcher dispatcher;

    public InstrumentGroupRepo(Dispatcher dispatcher) {
        List<InstrumentGroup> items;

        using (var db = new GOTContext()) {
            items = db.InstrumentGroups?.ToList() ?? [];
        }

        Items = new ObservableCollection<InstrumentGroup>(items);
        this.dispatcher = dispatcher;
    }

    public ObservableCollection<InstrumentGroup> Items { get; }

    public void Create(InstrumentGroup item) {
        using (var db = new GOTContext()) {
            if (db.InstrumentGroups == null) {
                return;
            }
            db.InstrumentGroups.Add(item);
            db.SaveChanges();
        }

        dispatcher.Invoke(() => Items.Add(item));
    }
    public void CreateByName(string name) {
        InstrumentGroup group = new InstrumentGroup() { Name = name };
        Create(group);
    }
}
