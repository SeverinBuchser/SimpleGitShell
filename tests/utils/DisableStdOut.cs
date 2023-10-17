using Server.GitShell.Lib.Logging;

namespace Tests.Server.GitShell.Utils;

public class TestConsole
{
    protected StringWriter _CaptureWriter = new();

    public TestConsole()
    {
        if (Environment.GetEnvironmentVariable("VERBOSE") != "true") 
        {
            Logger.SetOut(_CaptureWriter);
        }
    }

} 