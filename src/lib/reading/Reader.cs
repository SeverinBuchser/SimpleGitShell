namespace Server.GitShell.Lib.Reading;

public class Reader : IReader
{
    private static TextReader _reader = Console.In;

    private static readonly Reader _instance = new();

    public static Reader Instance
    {
        get => _instance;
    }

    static Reader() {
    }

    private Reader() {}

    public string? ReadLine()
    {   
        return _reader.ReadLine();
    }

    public static void SetIn(TextReader reader)
    {
        _reader = reader;
    }
}