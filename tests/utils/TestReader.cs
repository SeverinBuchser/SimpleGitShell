using SimpleGitShell.Library.Reading;

namespace Tests.SimpleGitShell.Utils;

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
