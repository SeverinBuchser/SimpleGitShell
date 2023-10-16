using Server.GitShell.Commands.Repo;
using Spectre.Console.Testing;

namespace Tests.Server.GitShell.Commands.Repo;

[Collection("File System Sequential")]
public class RemoveRepoCommandTests : BaseRepoCommandTests
{
    private static CommandAppTester App()
    {
        var app = new CommandAppTester();
        app.SetDefaultCommand<RemoveRepoCommand>();
        return app;
    }
} 