using Core.Base;
using System;

namespace Connectors;

public abstract class MarketDataObserver<TInstrument> : IObserver<TickEventArg> 
    where TInstrument : Instrument {

    public abstract TInstrument Instrument { get; }

    public void OnCompleted () {
        throw new NotImplementedException();
    }

    public void OnError (Exception error) {
        throw new NotImplementedException();
    }

    public abstract void OnNext (TickEventArg arg);
}
