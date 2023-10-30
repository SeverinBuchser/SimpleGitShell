using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Tests.SimpleGitShell.TestUtils.DataAttributes;

public sealed class LinesFileDataAttribute : FileDataAttribute
{
    private readonly string[] _lines;
    public int[] LineIndices { get; }
    public LinesFileDataAttribute(string filename, [NotNull] params int[] lineIndices) : base(filename)
    {
        LineIndices = lineIndices;
        if (string.IsNullOrEmpty(FileData))
        {
            throw new ArgumentException("The file is empty.");
        }

        var lines = FileData.Split("\n");
        var selectedLines = new List<string>();
        foreach (var line in lineIndices)
        {
            if (int.IsNegative(line))
            {
                throw new ArgumentException("The line number cannot be negative.");
            }

            if (lines.Length <= line)
            {
                throw new ArgumentException($"The file does not have this many lines. Found {lines.Length} lines, requested line {line}.");
            }

            selectedLines.Add(lines[line]);
        }
        _lines = selectedLines.ToArray();
    }

    public override IEnumerable<string[]> GetData(MethodInfo testMethod)
    {
        return new string[1][] { _lines };
    }
}
