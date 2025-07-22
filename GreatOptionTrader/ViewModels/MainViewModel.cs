using GreatOptionTrader.Commands;
using GreatOptionTrader.Models;
using GreatOptionTrader.Services.Connectors;
using GreatOptionTrader.Services.Repositories;
using System.Collections.ObjectModel;

namespace GreatOptionTrader.ViewModels;
public class MainViewModel (InteractiveBroker broker, InstrumentGroupRepo instrumentGroupsRepository ) {
    public ConnectCommand Connect { get; } = new ConnectCommand(broker);
    public ManageInstrumentsCommand ManageInstruments { get; } = new(broker);

    public AddInstrumentToGroupCommand AddInstrumentToGroup { get; } = new(broker);
    public CreateGroupCommand CreateGroup { get; } = new CreateGroupCommand(instrumentGroupsRepository);
    public ObservableCollection<InstrumentGroup> Groups => instrumentGroupsRepository.Items;
}
