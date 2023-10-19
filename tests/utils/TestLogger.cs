using Server.GitShell.Lib.Logging;

namespace Tests.Server.GitShell.Utils;

public class TestLogger
{
    protected StringWriter _CaptureWriter = new();

    public TestLogger()
    {
        if (Environment.GetEnvironmentVariable("VERBOSE") != "true") 
        {
            Logger.SetOut(_CaptureWriter);
        } 
        else
        {
            Logger.AddOut(_CaptureWriter);
        }
    }

} 