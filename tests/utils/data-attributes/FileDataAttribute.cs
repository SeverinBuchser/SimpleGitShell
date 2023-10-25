using Xunit.Sdk;

namespace Tests.SimpleGitShell.Utils.DataAttributes;

public abstract class FileDataAttribute : DataAttribute
{
    private readonly string _filename;

    protected string _fileData 
    {
        get {
            if (!File.Exists(_filename)) throw new ArgumentException("The file does not exit");
            return File.ReadAllText(_filename);
        }
    }

    public FileDataAttribute(string filename)
    {
        _filename = filename;
    }
}