using Spectre.Console.Cli;
using Tests.Server.GitShell.Utils;

namespace Tests.Server.GitShell.Commands.Group;

public class BaseGroupCommandTests : TestConsole
{
    protected static string _CWD = "test_dir";
    protected static string _ValidGroup = "Group";
    protected static string _SubDir = "subdir";
    protected static string _InvalidGroup = "";
    protected readonly IRemainingArguments _remainingArgs = new Mock<IRemainingArguments>().Object;

    public BaseGroupCommandTests() 
    {
        _CreateDirectory(_CWD);
        Directory.SetCurrentDirectory(_CWD);
    }

    public override void Dispose()
    {
        base.Dispose();
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
} 