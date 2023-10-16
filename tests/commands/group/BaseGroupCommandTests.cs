using Spectre.Console.Cli;
using Tests.Server.GitShell.Utils;

namespace Tests.Server.GitShell.Commands.Group;

public class BaseGroupCommandTests : FileSystemCommandTests
{
    protected static string _ValidGroup = "Group";
    protected static string _SubDir = "subdir";
    protected static string _InvalidGroup = "";
    protected readonly IRemainingArguments _remainingArgs = new Mock<IRemainingArguments>().Object;
}