namespace Tests.Server.GitShell.Utils;

public class FileSystemCommandTests : TestConsole, IDisposable
{
    protected static string _CWD = "test_dir";

    public FileSystemCommandTests() : base()
    {
        _CreateDirectory(_CWD);
        Directory.SetCurrentDirectory(_CWD);
    }

    public void Dispose()
    {
        Directory.SetCurrentDirectory(Directory.GetParent(Directory.GetCurrentDirectory())!.FullName);
        Directory.Delete(_CWD, true);
    }

    protected static void _CreateDirectory(string directory)
    {
        Directory.CreateDirectory(directory);
    }

    protected static void _CreateNonEmptyDirectory(string directory, string subdir)
    {
        Directory.CreateDirectory(directory);
        Directory.CreateDirectory(Path.Combine(directory, subdir));
    }

    protected static void _DeleteDirectory(string directory)
    {
        Directory.Delete(directory, true);
    }

    protected static string _CreationTime(string directory)
    {
        return Directory.GetCreationTime(directory).ToString();
    }
} 