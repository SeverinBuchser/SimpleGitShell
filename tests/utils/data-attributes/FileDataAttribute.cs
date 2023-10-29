using Xunit.Sdk;

namespace Tests.SimpleGitShell.Utils.DataAttributes;

public abstract class FileDataAttribute : DataAttribute
{
    protected string FileData
    {
        get
        {
#pragma warning disable CA1065
            if (!File.Exists(Filename))
            {
                throw new ArgumentException("The file does not exit");
            }
#pragma warning restore CA1065
            return File.ReadAllText(Filename);
        }
    }

    protected FileDataAttribute(string filename)
    {
        Filename = filename;
    }

    public string Filename { get; }
}
