using System.Diagnostics;
using SimpleGitShell.Lib.Logging;

namespace SimpleGitShell.Lib.Utils.Processes;

public class LoggerProcess : Process
{
    protected List<ILogger> _loggers = new();

    public void Attach(ILogger logger) 
    {
        _loggers.Add(logger);
    }
}