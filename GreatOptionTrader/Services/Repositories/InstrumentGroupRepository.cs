using GreatOptionTrader.Models;
using GreatOptionTrader.Services.Connectors;
using GreatOptionTrader.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;

namespace GreatOptionTrader.Services.Repositories;
public class InstrumentGroupRepository {
    private readonly Dispatcher dispatcher;
    private readonly InteractiveBroker broker;
    private readonly InstrumentRepository instrumentRepository;
    private readonly OrdersRepository ordersRepository;

    private GroupViewModel createGroupView(InstrumentGroup group) {
        return new GroupViewModel(
            group: group,
            broker: broker,
            instrumentRepository: instrumentRepository,
            ordersRepository: ordersRepository,
            dispatcher: dispatcher);
    }

    public InstrumentGroupRepository(
        Dispatcher dispatcher, 
        InteractiveBroker broker,
        InstrumentRepository instrumentRepository,
        OrdersRepository ordersRepository) {
        this.dispatcher = dispatcher;
        this.broker = broker;
        this.instrumentRepository = instrumentRepository;
        this.ordersRepository = ordersRepository;

        List<InstrumentGroup> items;

        using (var db = new GOTContext()) {
            items = db
                .InstrumentGroups? 
                .Include(group => group.Options)
                .ThenInclude(sec => sec.Orders)
                .ToList() ?? [];
        }

        Items = new ObservableCollection<GroupViewModel>(items.Select(createGroupView));
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

        dispatcher.Invoke(() => Items.Add(createGroupView(item)));
    }

    public void CreateByName(string name) {
        InstrumentGroup group = new InstrumentGroup() { Name = name };
        Create(group);
    }

    public void UpdateAll () { 
        using (var db = new GOTContext()) {
            foreach (var item in Items) {
                db.InstrumentGroups?.Update(item.Group);
            }

            db.SaveChanges();
        }
    }

}


