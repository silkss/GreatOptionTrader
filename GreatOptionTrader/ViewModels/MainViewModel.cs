using GreatOptionTrader.Commands;
using GreatOptionTrader.Models;
using GreatOptionTrader.Services.Connectors;
using GreatOptionTrader.Services.Repositories;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GreatOptionTrader.ViewModels;
public class MainViewModel (
    InteractiveBroker broker,
    InstrumentGroupRepository instrumentGroupsRepository,
    ObservableCollection<string> accounts) {
    public ConnectCommand Connect { get; } = new ConnectCommand(broker);
    public StartGroupCommand StartGroupCommand { get; } = new StartGroupCommand(broker);
    public CreateGroupCommand CreateGroup { get; } = new CreateGroupCommand(instrumentGroupsRepository);
    public SendOrderCommand SendOrderCommand { get; } = new SendOrderCommand(broker);
    public RequestOptionCommand RequestOptionCommand { get; } = new RequestOptionCommand(broker);
    public ObservableCollection<GroupViewModel> Groups => instrumentGroupsRepository.Items;
    public ObservableCollection<string> Accounts => accounts;
}
