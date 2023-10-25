using System.Diagnostics;
using Server.GitShell.Lib.Logging;

namespace Server.GitShell.Lib.Utils.Processes;

public class LoggerProcess : Process
{
    protected List<ILogger> _loggers = new();

    public void Attach(ILogger logger) 
    {
        _loggers.Add(logger);
    }
}