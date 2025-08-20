using GreatOptionTrader.Commands;
using GreatOptionTrader.Models;
using GreatOptionTrader.Services.Connectors;
using GreatOptionTrader.Services.Repositories;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GreatOptionTrader.ViewModels;
public class MainViewModel {
    private readonly InstrumentGroupRepository instrumentGroupsRepository;

    public MainViewModel (
        InteractiveBroker broker,
        InstrumentGroupRepository instrumentGroupsRepository,
        ObservableCollection<string> accounts) {

        Connect = new ConnectCommand(broker);
        StartGroupCommand = new StartGroupCommand(broker);
        SendOrderCommand = new SendOrderCommand(broker);
        CancelOrderCommand = new CancelOrderCommand(broker);

        RequestOptionCommand = new RequestOptionCommand(broker);
        CreateGroup = new CreateGroupCommand(instrumentGroupsRepository);
        this.instrumentGroupsRepository = instrumentGroupsRepository;
        EditInstrumentGroup = new EditInstrumentGroupCommand(
            accounts, 
            SendOrderCommand, 
            CancelOrderCommand,
            RequestOptionCommand);
    }

    public ConnectCommand Connect { get; } 
    public StartGroupCommand StartGroupCommand { get; } 
    public SendOrderCommand SendOrderCommand { get; } 
    public CancelOrderCommand CancelOrderCommand { get; }
    public RequestOptionCommand RequestOptionCommand { get; } 

    public CreateGroupCommand CreateGroup { get; }
    public ObservableCollection<GroupViewModel> Groups => instrumentGroupsRepository.Items;
    public EditInstrumentGroupCommand EditInstrumentGroup { get; }
}
