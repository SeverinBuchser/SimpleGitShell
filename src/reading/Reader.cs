namespace SimpleGitShell.Reading;

public class Reader : IReader
{
    private static TextReader _reader = Console.In;

    public static Reader Instance { get; } = new();

    static Reader()
    {
    }

    private Reader() { }

    public string? ReadLine()
    {
        return _reader.ReadLine();
    }

    public static void SetIn(TextReader reader)
    {
        _reader = reader;
    }
}
