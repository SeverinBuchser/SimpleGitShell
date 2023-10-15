using Server.GitShell.Commands.Group;
using Spectre.Console.Cli;
using Tests.Server.GitShell.Utils;

namespace Tests.Server.GitShell.Commands.Group;

[Collection("File System Sequential")]
public class ListGroupCommandTests : DisableConsole
{
    private static string _ValidGroupname = "groupname";
    private static string _InvalidGroupname = "";
    private readonly IRemainingArguments _remainingArgs = new Mock<IRemainingArguments>().Object;

} 