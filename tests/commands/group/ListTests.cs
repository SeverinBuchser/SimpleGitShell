using Server.GitShell.Commands.Group;
using Spectre.Console.Testing;

namespace Tests.Server.GitShell.Commands.Group;

[Collection("File System Sequential")]
public class ListGroupCommandTests : BaseGroupCommandTests
{
    private static CommandAppTester App()
    {
        var app = new CommandAppTester();
        app.SetDefaultCommand<ListGroupCommand>();
        return app;
    }

    [Fact]
    public void Execute_ValidGroupname_DoesNotListGitDirectories()
    {
        // Given
        _CreateDirectory("git1.git");
        _CreateDirectory("git2.git");
        _CreateDirectory("git3.git");
        _CreateDirectory("git4.git");
        _CreateDirectory("git-shell-commands");
        _CreateDirectory(_ValidGroupname);

        // When
        var result = App().Run();

        // Then
        Assert.Equal(0, result.ExitCode);
        // TODO Test output

        // Finally
        _DeleteDirectory(_ValidGroupname);
        _DeleteDirectory("git1.git");
        _DeleteDirectory("git2.git");
        _DeleteDirectory("git3.git");
        _DeleteDirectory("git4.git");
        _CreateDirectory("git-shell-commands");
    }
} 