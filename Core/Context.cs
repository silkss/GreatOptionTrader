using Core.Connectors.InteractiveBrokers;
using Microsoft.Extensions.Logging;

namespace Core;

public class Context {
    private readonly ILoggerFactory _loggerFactory;
    private readonly FoldersHolder _folders;

    public Connector Connector { get; }

    public Context (string applicationName) {
        _folders = new FoldersHolder(applicationName);
        _loggerFactory = LoggerFactory.Create(b => b.AddDebug());

        Connector = new(_loggerFactory.CreateLogger<Connector>());
    }
}
