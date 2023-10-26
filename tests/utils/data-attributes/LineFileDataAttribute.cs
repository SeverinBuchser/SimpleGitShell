using System.Reflection;

namespace Tests.SimpleGitShell.Utils.DataAttributes;

public sealed class LineFileDataAttribute : FileDataAttribute
{
    private readonly string _line;
    public int LineIndex { get; }
    public LineFileDataAttribute(string filename, int lineIndex = 0) : base(filename)
    {
        LineIndex = lineIndex;
        if (int.IsNegative(lineIndex))
        {
            throw new ArgumentException("The line number cannot be negative.");
        }

        if (string.IsNullOrEmpty(FileData))
        {
            throw new ArgumentException("The file is empty.");
        }

        var lines = FileData.Split("\n");
        if (lines.Length <= lineIndex)
        {
            throw new ArgumentException($"The file does not have this many lines. Found {lines.Length} lines, requested line {lineIndex}.");
        }

        _line = lines[lineIndex];
    }

    public override IEnumerable<string[]> GetData(MethodInfo testMethod)
    {
        return new string[1][] { new string[] { _line } };
    }

}
