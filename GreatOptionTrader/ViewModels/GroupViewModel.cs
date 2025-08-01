﻿using GreatOptionTrader.Commands.Base;
using GreatOptionTrader.Models;
using GreatOptionTrader.Services.Connectors;
using GreatOptionTrader.Services.Repositories;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;

namespace GreatOptionTrader.ViewModels;

public class GroupViewModel : Base.ObservableObject {
    private readonly InteractiveBroker broker;
    private readonly InstrumentRepository instrumentRepository;
    private readonly OrdersRepository ordersRepository;
    private readonly Dispatcher dispatcher;
    private string? requestedOptionName;
    private string? requestedOptionExchange;

    private InstrumentViewModel createInstrumentViewModel(Instrument instrument) {
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
        this.Instruments = new ObservableCollection<InstrumentViewModel>(group.Instruments.Select(createInstrumentViewModel));
        this.RequestOptionCommand = new LambdaCommand(onRequestOptionCommand, canRequestOptionCommand);
        this.broker.ContractUpdated += OnInstrumentUpdated;
    }
    public bool IsStarted { get; set; }
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
    
    public void OnInstrumentUpdated(int requestId, Instrument option) {
        if (requestId != Group.Id) {
            return;
        }

        if (Instruments.Any(instrument => instrument.Instrument.Id == option.Id)) {
            return;
        }

        option.InstrumentGroupId = Group.Id;

        instrumentRepository.Add(option);

        dispatcher.Invoke(() => Instruments.Add(createInstrumentViewModel(option)));
    }

    #region RequestOptionCommand
    public LambdaCommand RequestOptionCommand { get; }
    private void onRequestOptionCommand(object? p) {
        if (string.IsNullOrEmpty(RequestedOptionName) || 
            string.IsNullOrEmpty(RequestedOptionExchange)) {
            return;
        }

        broker.RequestOption(Group.Id, RequestedOptionName, RequestedOptionExchange);
    }
    private bool canRequestOptionCommand (object? p) => broker.IsConnected()
        && !string.IsNullOrEmpty(RequestedOptionName)
        && !string.IsNullOrEmpty(RequestedOptionExchange);
    #endregion
}
