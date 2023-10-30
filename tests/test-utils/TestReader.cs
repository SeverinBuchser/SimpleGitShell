using SimpleGitShellrary.Reading;

namespace Tests.SimpleGitShell.TestUtils;

public class TestReader : TestLogger, IDisposable
{
    protected static void SetInput(string input)
    {
        Reader.SetIn(new StringReader(input));
    }

    public virtual void Dispose()
    {
        Reader.SetIn(Console.In);
    }
}
