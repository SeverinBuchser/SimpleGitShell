using ConsoleTables;
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
    public void Execute_ValidGitDirs_OnlyListsGitDirectories()
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
            Columns = new string[] {"Repository", "Creation Time"}
        });
        foreach (var gitDir in _ValidRepos) {
            table.AddRow(gitDir, _CreationTime(gitDir));
        }
        table.Write(Format.Alternative);

        Assert.Equal($"[INFO] Available repositories in group \"root\":\n[INFO] \n{ writer }", _CaptureWriter.ToString());

        // Finally
        foreach (var gitDir in _ValidRepos) _DeleteDirectory(gitDir);
        _CreateDirectory("git-shell-commands");
        _DeleteDirectory(_ValidGroup);
    }

    [Fact]
    public void Execute_ValidGitDirsInValidGroup_OnlyListsGitDirectoriesInValidGroup()
    {
        // Given
        _CreateDirectory("git-shell-commands");
        _CreateDirectory(_ValidGroup);
        _CreateDirectory("root.git");
        foreach (var gitDir in _ValidRepos) _CreateDirectory(Path.Combine(_ValidGroup, gitDir));
        var args = new string[]{$"--group={_ValidGroup}"};

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);

        var writer = new StringWriter();
        var table = new ConsoleTable(new ConsoleTableOptions {
            OutputTo = writer,
            Columns = new string[] {"Repository", "Creation Time"}
        });
        foreach (var gitDir in _ValidRepos) {
            table.AddRow(gitDir, _CreationTime(Path.Combine(_ValidGroup, gitDir)));
        }
        table.Write(Format.Alternative);

        Assert.Equal($"[INFO] Available repositories in group \"{ _ValidGroup }\":\n[INFO] \n{ writer }", _CaptureWriter.ToString());

        // Finally
        _CreateDirectory("git-shell-commands");
        _DeleteDirectory(_ValidGroup);
        _DeleteDirectory("root.git");
    }
} 