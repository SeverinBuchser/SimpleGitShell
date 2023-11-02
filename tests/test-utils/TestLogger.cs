using SimpleGitShell.Logging;

namespace Tests.SimpleGitShell.TestUtils;

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
