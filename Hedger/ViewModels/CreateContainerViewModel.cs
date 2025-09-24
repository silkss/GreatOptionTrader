using Connectors.IB;
using Core.Types;
using Core.Types.Base;
using Core.Commands.Base;
using Hedger.Models;
using Hedger.Exceptions;
using System;
using System.Windows;
using System.Collections.Generic;

namespace Hedger.ViewModels;
public class CreateContainerViewModel : ObservableObject {
    private static int currentRequestId = 1;

    private int basisRequestId;
    private int hedgeBasisRequestId; 

    private readonly InteractiveBroker broker;

    public CreateContainerViewModel(InteractiveBroker broker) {
        this.broker = broker;
        this.broker.FutureReqeustedEvent += onFutureEvent;

        RequestBasis = new LambdaCommand(onRequestBasis, canRequestBasis);
        RequestHedgeBasis = new LambdaCommand(onRequestHedgeBasis, canRequestHedgeBasis);

        Create = new LambdaCommand(onCreate, canCreate);
    }

    public string? BasisName { get; set; }
    public string? BasisExchange { get; set; }

    private Future? basis;
    public Future? SelectedBasis { get => basis; set => Set(ref basis, value); }

    public string? HedgeBasisName { get; set; }
    public string? HedgeBasisExchange { get; set; }

    private Future? hedgeBasis;
    public Future? SelectedHedgeBasis { get => hedgeBasis; set => Set(ref hedgeBasis, value); }

    public string? SelectedAccount { get; set; }
    public IEnumerable<string>? Accounts => broker.Accounts;

    public LambdaCommand RequestBasis { get; }
    private void onRequestBasis (object? p) {
        if (string.IsNullOrEmpty(BasisName) || string.IsNullOrEmpty(BasisExchange)) { return; }
        basisRequestId = currentRequestId++;
        broker.RequestFuture(basisRequestId, BasisName, BasisExchange);
    }
    private bool canRequestBasis (object? p) => broker.IsConnected()
        && !string.IsNullOrEmpty(BasisName)
        && !string.IsNullOrEmpty(BasisExchange);
    
    public LambdaCommand RequestHedgeBasis { get; }
    private void onRequestHedgeBasis(object? p) {
        if (string.IsNullOrEmpty(HedgeBasisName) || string.IsNullOrEmpty(HedgeBasisExchange)) { return; }
        hedgeBasisRequestId = currentRequestId++;
        broker.RequestFuture(hedgeBasisRequestId, HedgeBasisName, HedgeBasisExchange);
    }
    private bool canRequestHedgeBasis (object? p) => broker.IsConnected() 
        && !string.IsNullOrEmpty(HedgeBasisName)
        && !string.IsNullOrEmpty(HedgeBasisExchange);

    private void onFutureEvent (int id, Future item) {
        if (id == basisRequestId) {
            SelectedBasis = item;
            SelectedHedgeBasis ??= item;
        }
        else if (id == hedgeBasisRequestId) {
            SelectedHedgeBasis = item;
        }
    }
    public LambdaCommand Create { get; }
    private void onCreate (object? p) {
        if (p is Window w) {
            w.DialogResult = true;
        }
    }
    private bool canCreate (object? p) => p is Window
        && !string.IsNullOrEmpty(SelectedAccount)
        && SelectedBasis != null
        && SelectedHedgeBasis != null;

    public Container CreteContainer () => new () {
        Account = SelectedAccount ?? throw new ContainerCreationError("ошибка с аккаунтом!"),
        Basis = SelectedBasis ?? throw new ContainerCreationError("ошибка с базисом!"),
        Id = Guid.NewGuid(),
        Hedge = new HedgeContainer() {
            Basis = SelectedHedgeBasis ?? throw new ContainerCreationError("не выбран базис для хеджа!"),
            BuyLevels = [],
            SellLevels = []
        }
    };
}
