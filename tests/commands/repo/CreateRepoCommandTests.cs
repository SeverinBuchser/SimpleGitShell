using SimpleGitShell.Commands.Repo;
using SimpleGitShellrary.Exceptions.Group;
using SimpleGitShellrary.Exceptions.Repo;
using SimpleGitShellrary.Utils.Processes.Git;
using Spectre.Console.Cli;
using Spectre.Console.Testing;
using Tests.SimpleGitShell.TestUtils;

namespace Tests.SimpleGitShell.Commands.Repo;

[Collection("File System Sequential")]
public class CreateRepoCommandTests : FileSystemTests
{
    private static CommandAppTester App()
    {
        var app = new CommandAppTester();
        app.SetDefaultCommand<CreateRepoCommand>();
        return app;
    }

    [Fact]
    public void RunEmptyRepoThrowsEmptyRepoNameException()
    {
        // Given
        var args = new string[] { "" };

        // When
        var result = App().RunAndCatch<EmptyRepoNameException>(args);

        // Then
        Assert.IsType<EmptyRepoNameException>(result.Exception);
    }

    [Theory]
    [InlineData("$")]
    [InlineData("#")]
    [InlineData("\\")]
    [InlineData("(")]
    [InlineData("`")]
    [InlineData("_")]
    public void RunInvalidRepoThrowsRepoNameNotValidException(string repo)
    {
        // Given
        var args = new string[] { repo };

        // When
        var result = App().RunAndCatch<RepoNameNotValidException>(args);

        // Then
        Assert.IsType<RepoNameNotValidException>(result.Exception);
    }

    [Theory]
    [InlineData("$")]
    [InlineData("#")]
    [InlineData("\\")]
    [InlineData("(")]
    [InlineData("`")]
    [InlineData("_")]
    public void RunInvalidBaseGroupThrowsGroupNameNotValidException(string baseGroup)
    {
        // Given
        var args = new string[] { "repo", $"--base-group={baseGroup}" };

        // When
        var result = App().RunAndCatch<GroupNameNotValidException>(args);

        // Then
        Assert.IsType<GroupNameNotValidException>(result.Exception);
    }


    [Fact]
    public void RunValidRepoNonExistingCreatesRepo()
    {
        // Given
        var args = new string[] { "repo" };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists("repo.git"));
        DeleteDirectory("repo.git");
    }

    [Fact]
    public void RunExistingRepoPromptsUserForConfirmation()
    {
        // Given
        var gitInitBareProcess = new GitInitBareProcess("repo.git");
        gitInitBareProcess.Start();

        SetInput("abort");
        var args = new string[] { "repo" };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.Contains("confirm", CaptureWriter.ToString());

        // Finally
        DeleteDirectory("repo.git");
        gitInitBareProcess.Dispose();
    }

    [Fact]
    public void RunExistingRepoAbortDoesNotOverrideRepo()
    {
        // Given
        var gitInitBareProcess = new GitInitBareProcess("repo.git");
        gitInitBareProcess.Start();
        var subDirPath = Path.Combine("repo.git", "subdir");
        CreateDirectory(subDirPath);
        SetInput("abort");
        var args = new string[] { "repo" };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(subDirPath));

        // Finally
        DeleteDirectory("repo.git");
        gitInitBareProcess.Dispose();
    }

    [Fact]
    public void RunExistingRepoConfirmOverridesRepo()
    {
        // Given
        var gitInitBareProcess = new GitInitBareProcess("repo.git");
        gitInitBareProcess.Start();
        var subDirPath = Path.Combine("repo.git", "subdir");
        CreateDirectory(subDirPath);
        SetInput(Path.Combine(".", "repo.git"));
        var args = new string[] { "repo" };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.False(Directory.Exists(subDirPath));

        // Finally
        DeleteDirectory("repo.git");
        gitInitBareProcess.Dispose();
    }

    [Fact]
    public void RunNonExistingBaseGroupThrowsGroupDoesNotExistException()
    {
        // Given
        var args = new string[] { "repo", $"--base-group=group" };

        // When
        var result = App().RunAndCatch<CommandRuntimeException>(args);

        // Then
        Assert.IsType<CommandRuntimeException>(result.Exception);
    }

    [Fact]
    public void RunExistingBaseGroupCreatesRepoInBaseGroup()
    {
        // Given
        CreateDirectory("group");
        var args = new string[] { "repo", $"--base-group=group" };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(Path.Combine("group", "repo.git")));
    }

    [Fact]
    public void RunExistingRepoInBaseGroupPromptsUserForConfirmation()
    {
        // Given
        CreateDirectory("group");
        var repoPath = Path.Combine("group", "repo.git");
        var gitInitBareProcess = new GitInitBareProcess(repoPath);
        gitInitBareProcess.Start();
        CreateNonEmptyDirectory(repoPath, "subdir");
        SetInput("abort");
        var args = new string[] { "repo", $"--base-group=group" };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.Contains("confirm", CaptureWriter.ToString());

        // Finally
        DeleteDirectory("group");
        gitInitBareProcess.Dispose();
    }

    [Fact]
    public void RunExistingRepoInBaseGroupAbortDoesNotOverrideRepo()
    {
        // Given
        CreateDirectory("group");
        var repoPath = Path.Combine("group", "repo.git");
        var gitInitBareProcess = new GitInitBareProcess(repoPath);
        gitInitBareProcess.Start();

        var subDirPath = Path.Combine(repoPath, "subdir");
        CreateDirectory(subDirPath);

        SetInput("abort");
        var args = new string[] { "repo", $"--base-group=group" };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(subDirPath));

        // Finally
        DeleteDirectory("group");
        gitInitBareProcess.Dispose();
    }

    [Fact]
    public void RunExistingRepoInBaseGroupConfirmOverridesRepo()
    {
        // Given
        CreateDirectory("group");
        var repoPath = Path.Combine("group", "repo.git");
        var gitInitBareProcess = new GitInitBareProcess(repoPath);
        gitInitBareProcess.Start();

        var subDirPath = Path.Combine(repoPath, "subdir");
        CreateDirectory(subDirPath);
        SetInput(repoPath);
        var args = new string[] { "repo", $"--base-group=group" };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.False(Directory.Exists(subDirPath));

        // Finally
        DeleteDirectory("group");
        gitInitBareProcess.Dispose();
    }
}
