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

    private static string Time()
    {
        return string.Format("{0:dd/mm/yyyy HH:mm:ss}", DateTime.Now);
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
        var text = _CaptureWriter.ToString();
        foreach (var gitDir in _ValidRepos) 
        {
            Assert.Contains(gitDir, text);
        }
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
        var text = _CaptureWriter.ToString();
        foreach (var gitDir in _ValidRepos) 
        {
            Assert.Contains(gitDir, text);
        }
    }
} 