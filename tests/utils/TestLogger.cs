using SimpleGitShell.Library.Logging;

namespace Tests.SimpleGitShell.Utils;

#pragma warning disable CA1001
public class TestLogger
#pragma warning restore CA1001
{
    public StringWriter CaptureWriter { get; } = new();

    public TestLogger()
    {
        if (Environment.GetEnvironmentVariable("VERBOSE") != "true")
        {
            Logger.SetOut(CaptureWriter);
        }
        else
        {
            Logger.AddOut(CaptureWriter);
        }
    }
}
