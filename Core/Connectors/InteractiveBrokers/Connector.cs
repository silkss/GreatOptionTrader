using IBApi;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace Core.Connectors.InteractiveBrokers;

public class Connector {
    readonly Wrapper _wrapper;
    readonly IBApi.EReaderMonitorSignal _monitor;
    readonly IBApi.EClientSocket _socket;

    public Connector (ILogger<Connector> logger) {
        _wrapper = new(logger);
        _monitor = new();
        _socket = new(_wrapper, _monitor);
    }

    public bool IsConnected () => _socket.IsConnected();
    
    public void Connect(
        string host,
        int port,
        int clientId
    ) {
        _socket.eConnect(host, port, clientId);
        EReader reader = new(_socket, _monitor);
        reader.Start();

        new Thread(() => {
            while (_socket.IsConnected()) {
                _monitor.waitForSignal();
                reader.processMsgs();
            }
        }) {
            IsBackground = true
        }.Start();
    }
}
