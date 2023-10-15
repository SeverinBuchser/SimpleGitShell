using Spectre.Console.Cli;
using Tests.Server.GitShell.Utils;

namespace Tests.Server.GitShell.Commands.Group;

public class BaseGroupCommandTests : DisableConsole
{
    protected static string _ValidGroupname = "groupname";
    protected static string _SubDir = "subdir";
    protected static string _InvalidGroupname = "";
    protected readonly IRemainingArguments _remainingArgs = new Mock<IRemainingArguments>().Object;

    public override void Dispose()
    {
        base.Dispose();
        if (Directory.Exists(_ValidGroupname)) Directory.Delete(_ValidGroupname, true);
    }

    protected static void _CreateGroupDirectory(string directory)
    {
        Directory.CreateDirectory(directory);
    }

    protected static void _CreateNonEmptyGroupDirectory(string directory, string subdir)
    {
        Directory.CreateDirectory(directory);
        Directory.CreateDirectory(Path.Combine(directory, subdir));
    }
} 