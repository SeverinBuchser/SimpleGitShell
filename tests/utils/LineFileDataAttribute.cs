using System.Reflection;

namespace Tests.Server.GitShell.Utils;

public class LineFileDataAttribute : FileDataAttribute
{
    private string _line;
    public LineFileDataAttribute(string filename, int lineIndex = 0) : base(filename)
    {   
        if (int.IsNegative(lineIndex)) throw new ArgumentException("The line number cannot be negative.");
        if (string.IsNullOrEmpty(_fileData)) throw new ArgumentException("The file is empty.");
        var lines = _fileData.Split("\n");
        if (lines.Length <= lineIndex) throw new ArgumentException($"The file does not have this many lines. Found { lines.Length } lines, requested line { lineIndex }.");
        _line = lines[lineIndex];
    }

    public override IEnumerable<string[]> GetData(MethodInfo testMethod)
    {
        return new string[1][]{ new string[] {_line} };
    }
}