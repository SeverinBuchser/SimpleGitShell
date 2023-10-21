using System.Reflection;

namespace Tests.Server.GitShell.Utils;

public class LinesFileDataAttribute : FileDataAttribute
{
    private string[] _lines;
    public LinesFileDataAttribute(string filename, params int[] lineIndices) : base(filename)
    {   
        if (string.IsNullOrEmpty(_fileData)) throw new ArgumentException("The file is empty.");
        var lines = _fileData.Split("\n");
        var selectedLines = new List<string>();
        foreach (var line in lineIndices) {
            if (int.IsNegative(line)) throw new ArgumentException("The line number cannot be negative.");
            if (lines.Length <= line) throw new ArgumentException($"The file does not have this many lines. Found { lines.Length } lines, requested line { line }.");
            selectedLines.Add(lines[line]);
        }
        _lines = selectedLines.ToArray();
    }

    public override IEnumerable<string[]> GetData(MethodInfo testMethod)
    {
        return new string[1][]{ _lines };
    }
}