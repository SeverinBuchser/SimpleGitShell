using SimpleGitShell.Commands.Repo;
using SimpleGitShellrary.Exceptions.Group;
using SimpleGitShellrary.Exceptions.Repo;
using SimpleGitShellrary.Utils.Processes.Git;
using Spectre.Console.Cli;
using Spectre.Console.Testing;
using Tests.SimpleGitShell.TestUtils;

namespace Tests.SimpleGitShell.Commands.Repo;

[Collection("File System Sequential")]
public class RemoveRepoCommandTests : FileSystemTests
{
    private static CommandAppTester App()
    {
        var app = new CommandAppTester();
        app.SetDefaultCommand<RemoveRepoCommand>();
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
    public void RunNonExistingBaseGroupThrowsGroupDoesNotExistException()
    {
        // Given
        var args = new string[] { "repo", "--base-group=group" };

        // When
        var result = App().RunAndCatch<CommandRuntimeException>(args);

        // Then
        Assert.IsType<CommandRuntimeException>(result.Exception);
    }

    [Fact]
    public void RunNonExistingRepoThrowsRepoDoesNotExistException()
    {
        // Given
        var args = new string[] { "repo" };

        // When
        var result = App().RunAndCatch<RepoDoesNotExistException>(args);

        // Then
        Assert.IsType<RepoDoesNotExistException>(result.Exception);
    }

    [Fact]
    public void RunNonExistingRepoExistingBaseGroupThrowsRepoDoesNotExistException()
    {
        // Given
        CreateDirectory("group");
        var args = new string[] { "repo", "--base-group=group" };

        // When
        var result = App().RunAndCatch<RepoDoesNotExistException>(args);

        // Then
        Assert.IsType<RepoDoesNotExistException>(result.Exception);
        DeleteDirectory("group");
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
    public void RunExistingRepoAbortDoesNotRemoveRepo()
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
        Assert.True(Directory.Exists("repo.git"));

        // Finally
        DeleteDirectory("repo.git");
        gitInitBareProcess.Dispose();
    }

    [Fact]
    public void RunExistingRepoConfirmRemovesRepo()
    {
        // Given
        var gitInitBareProcess = new GitInitBareProcess("repo.git");
        gitInitBareProcess.Start();
        SetInput(Path.Combine(".", "repo.git"));
        var args = new string[] { "repo" };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.False(Directory.Exists("repo.git"));

        // Finally
        gitInitBareProcess.Dispose();
    }

    [Fact]
    public void RunExistingRepoInBaseGroupPromptsUserForConfirmation()
    {
        // Given
        CreateDirectory("group");
        var repoPath = Path.Combine("group", "repo.git");
        var gitInitBareProcess = new GitInitBareProcess(repoPath);
        gitInitBareProcess.Start();
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

        SetInput("abort");
        var args = new string[] { "repo", $"--base-group=group" };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(repoPath));

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

        SetInput(repoPath);
        var args = new string[] { "repo", $"--base-group=group" };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.False(Directory.Exists(repoPath));

        // Finally
        DeleteDirectory("group");
        gitInitBareProcess.Dispose();
    }
}
