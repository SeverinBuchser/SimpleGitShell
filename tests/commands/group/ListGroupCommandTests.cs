using ConsoleTables;
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
    public void Execute_ValidGroups_OnlyListsGroupDirectories()
    {
        // Given
        foreach (var gitDir in _ValidRepos) _CreateDirectory(gitDir);
        _CreateDirectory("git-shell-commands");
        _CreateDirectory(_ValidGroup);
        

        // When
        var result = App().Run();

        // Then
        Assert.Equal(0, result.ExitCode);

        var writer = new StringWriter();
        var table = new ConsoleTable(new ConsoleTableOptions {
            OutputTo = writer,
            Columns = new string[] {"Group", "Creation Time"}
        });
        table.AddRow(_ValidGroup, _CreationTime(_ValidGroup));
        table.Write(Format.Alternative);

        Assert.Equal($"[INFO] Available groups:\n[INFO] \n{ writer }", _CaptureWriter.ToString());
    }
} 