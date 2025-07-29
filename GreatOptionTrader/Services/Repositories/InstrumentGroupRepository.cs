using GreatOptionTrader.Models;
using GreatOptionTrader.Services.Connectors;
using GreatOptionTrader.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;

namespace GreatOptionTrader.Services.Repositories;
public class InstrumentGroupRepository {
    private readonly Dispatcher dispatcher;
    private readonly InteractiveBroker broker;
    private readonly InstrumentRepository instrumentRepository;

    public InstrumentGroupRepository(Dispatcher dispatcher, InteractiveBroker broker, InstrumentRepository instrumentRepository) {
        this.dispatcher = dispatcher;
        this.broker = broker;
        this.instrumentRepository = instrumentRepository;
        List<InstrumentGroup> items;

        using (var db = new GOTContext()) {
            items = db
                .InstrumentGroups? 
                .Include(group => group.Instruments)
                .ToList() ?? [];
        }

        Items = new ObservableCollection<GroupViewModel>(
            items.Select(item => new GroupViewModel(item, broker, instrumentRepository, dispatcher)));
    }

    public ObservableCollection<GroupViewModel> Items { get; }

    public void Create(InstrumentGroup item) {
        using (var db = new GOTContext()) {
            if (db.InstrumentGroups == null) {
                return;
            }
            db.InstrumentGroups.Add(item);
            db.SaveChanges();
        }

        dispatcher.Invoke(() => Items.Add(new GroupViewModel(item, broker, instrumentRepository, dispatcher)));
    }

    public void CreateByName(string name) {
        InstrumentGroup group = new InstrumentGroup() { Name = name };
        Create(group);
    }
}
