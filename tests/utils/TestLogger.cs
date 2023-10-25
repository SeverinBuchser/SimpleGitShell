using SimpleGitShell.Lib.Logging;

namespace Tests.SimpleGitShell.Utils;

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