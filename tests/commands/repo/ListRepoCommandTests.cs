using Server.GitShell.Commands.Repo;
using Spectre.Console.Testing;
using Tests.Server.GitShell.Commands.Group;

namespace Tests.Server.GitShell.Commands.Repo;

[Collection("File System Sequential")]
public class ListRepoCommandTests : BaseGroupCommandTests
{
    private static CommandAppTester App()
    {
        var app = new CommandAppTester();
        app.SetDefaultCommand<ListRepoCommand>();
        return app;
    }

    [Fact]
    public void Execute_ValidGroup_DoesNotListGitDirectories()
    {
        // Given
        _CreateDirectory("git1.git");
        _CreateDirectory("git2.git");
        _CreateDirectory("git3.git");
        _CreateDirectory("git4.git");
        _CreateDirectory("git-shell-commands");
        _CreateDirectory(_ValidGroup);

        // When
        var result = App().Run();

        // Then
        Assert.Equal(0, result.ExitCode);
        // TODO Test output

        // Finally
        _DeleteDirectory(_ValidGroup);
        _DeleteDirectory("git1.git");
        _DeleteDirectory("git2.git");
        _DeleteDirectory("git3.git");
        _DeleteDirectory("git4.git");
        _CreateDirectory("git-shell-commands");
    }
} 