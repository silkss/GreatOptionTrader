using System;
using System.Collections.Generic;

namespace Connectors;
internal class Unsubscriber(IObserver<TickEventArg> observer, List<IObserver<TickEventArg>> observers) : IDisposable {
    private readonly IObserver<TickEventArg> observer = observer;
    private readonly List<IObserver<TickEventArg>>  observers = observers;
    private bool isDisposed = false;

    public void Dispose () {
        if( !isDisposed) {
            observers.Remove(observer);
            isDisposed = true;
        }
    }
}
