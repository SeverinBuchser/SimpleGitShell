namespace Tests.SimpleGitShell.Utils;

public class FileSystemTests : TestReader
{
    protected static readonly string CWD = "test_dir";

    public FileSystemTests() : base()
    {
        CreateDirectory(CWD);
        Directory.SetCurrentDirectory(CWD);
    }

    public override void Dispose()
    {
        base.Dispose();
        Directory.SetCurrentDirectory(Directory.GetParent(Directory.GetCurrentDirectory())!.FullName);
        Directory.Delete(CWD, true);
    }

    protected static void CreateDirectory(string directory)
    {
        Directory.CreateDirectory(directory);
    }

    protected static void CreateNonEmptyDirectory(string directory, string subdir)
    {
        Directory.CreateDirectory(Path.Combine(directory, subdir));
    }

    protected static void DeleteDirectory(string directory)
    {
        Directory.Delete(directory, true);
    }

    protected static string CreationTime(string directory)
    {
        return Directory.GetCreationTime(directory).ToString();
    }

    protected static void CreateFile(string filename, string content)
    {
        var writer = File.CreateText(filename);
        writer.Write(content);
        writer.Close();
    }

    protected static void DeleteFile(string filename)
    {
        File.Delete(filename);
    }
}
