using Server.GitShell.Lib.Reading;

namespace Tests.Server.GitShell.Utils;

public class TestReader : TestLogger
{
    protected static void _SetInput(string input)
    {
        Reader.SetIn(new StringReader(input));
    }
} 