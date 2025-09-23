using Connectors.IB;
using Core.Commands.Base;
using Core.Types;
using Core.Types.Base;
using Hedger.Exceptions;
using Hedger.Models;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Hedger.ViewModels;
public class CreateContainerViewModel : ObservableObject {
    private readonly int requestId = DateTime.Now.Second;
    private readonly InteractiveBroker broker;

    public CreateContainerViewModel(InteractiveBroker broker) {
        this.broker = broker;
        this.broker.FutureReqeustedEvent += onFutureEvent;

        RequestFuture = new LambdaCommand(onRequesteFuture, canRequestFuture);
        Create = new LambdaCommand(onCreate, canCreate);
    }

    public string? BasisName { get; set; }
    public string? BasisExchange { get; set; }

    private Future? basis;
    public Future? SelectedBasis { get => basis; set => Set(ref basis, value); }

    public string? SelectedAccount { get; set; }
    public IEnumerable<string>? Accounts => broker.Accounts;

    public LambdaCommand RequestFuture { get; }
    private void onRequesteFuture(object? p) {
        if (string.IsNullOrEmpty(BasisName) || string.IsNullOrEmpty(BasisExchange)) { return; }

        broker.RequestFuture(requestId, BasisName, BasisExchange);
    }
    private bool canRequestFuture (object? p) => broker.IsConnected()
        && !string.IsNullOrEmpty(BasisName)
        && !string.IsNullOrEmpty(BasisExchange);

    private void onFutureEvent (int id, Core.Types.Future item) {
        if (id != requestId) { return; }
        SelectedBasis = item;
    }

    public Container CreteContainer () => new () {
        Account = SelectedAccount ?? throw new ContainerCreationError("ошибка с аккаунтом!"),
        Basis = SelectedBasis ?? throw new ContainerCreationError("ошибка с базисом!"),
        Id = Guid.NewGuid(),
    };

    public LambdaCommand Create { get; }
    private void onCreate(object? p) {
        if (p is Window w) {
            w.DialogResult = true;
        }
    }
    private bool canCreate (object? p) => p is Window
        && !string.IsNullOrEmpty(SelectedAccount)
        && SelectedBasis != null;
}
