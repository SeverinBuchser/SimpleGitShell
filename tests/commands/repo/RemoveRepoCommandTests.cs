using SimpleGitShell.Commands.Repo;
using SimpleGitShell.Utils.Processes.Git;
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

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("$")]
    [InlineData("#")]
    [InlineData("\\")]
    [InlineData("(")]
    [InlineData("`")]
    [InlineData("_")]
    public void RunInvalidRepoThrowsCommandRuntimeException(string repo)
    {
        // Given
        var args = new string[] { repo };

        // When
        var result = App().RunAndCatch<CommandRuntimeException>(args);

        // Then
        Assert.IsType<CommandRuntimeException>(result.Exception);
        Assert.Contains("repository name", result.Exception.Message);
    }

    [Theory]
    [InlineData(" ")]
    [InlineData("$")]
    [InlineData("#")]
    [InlineData("\\")]
    [InlineData("(")]
    [InlineData("`")]
    [InlineData("_")]
    public void RunInvalidBaseGroupThrowsCommandRuntimeException(string baseGroup)
    {
        // Given
        var args = new string[] { "repo", $"--base-group={baseGroup}" };

        // When
        var result = App().RunAndCatch<CommandRuntimeException>(args);

        // Then
        Assert.IsType<CommandRuntimeException>(result.Exception);
        Assert.Contains("base group name", result.Exception.Message);
    }

    [Fact]
    public void RunNonExistingBaseGroupThrowsCommandRuntimeException()
    {
        // Given
        var args = new string[] { "repo", "--base-group=basegroup" };

        // When
        var result = App().RunAndCatch<CommandRuntimeException>(args);

        // Then
        Assert.IsType<CommandRuntimeException>(result.Exception);
        Assert.Contains("directory", result.Exception.Message);
        Assert.Contains("basegroup", result.Exception.Message);
    }

    [Fact]
    public void RunNonExistingRepoThrowsCommandRuntimeException()
    {
        // Given
        var args = new string[] { "repo" };

        // When
        var result = App().RunAndCatch<CommandRuntimeException>(args);

        // Then
        Assert.IsType<CommandRuntimeException>(result.Exception);
        Assert.Contains("directory", result.Exception.Message);
        Assert.Contains("repo", result.Exception.Message);
    }

    [Fact]
    public void RunNonExistingRepoExistingBaseGroupThrowsCommandRuntimeException()
    {
        // Given
        CreateDirectory("basegroup");
        var args = new string[] { "repo", "--base-group=basegroup" };

        // When
        var result = App().RunAndCatch<CommandRuntimeException>(args);

        // Then
        Assert.IsType<CommandRuntimeException>(result.Exception);
        Assert.Contains("directory", result.Exception.Message);
        Assert.Contains("repo", result.Exception.Message);
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
        CreateDirectory("basegroup");
        var repoPath = Path.Combine("basegroup", "repo.git");
        var gitInitBareProcess = new GitInitBareProcess(repoPath);
        gitInitBareProcess.Start();
        SetInput("abort");
        var args = new string[] { "repo", $"--base-group=basegroup" };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.Contains("confirm", CaptureWriter.ToString());

        // Finally
        DeleteDirectory("basegroup");
        gitInitBareProcess.Dispose();
    }

    [Fact]
    public void RunExistingRepoInBaseGroupAbortDoesNotOverrideRepo()
    {
        // Given
        CreateDirectory("basegroup");
        var repoPath = Path.Combine("basegroup", "repo.git");
        var gitInitBareProcess = new GitInitBareProcess(repoPath);
        gitInitBareProcess.Start();

        SetInput("abort");
        var args = new string[] { "repo", $"--base-group=basegroup" };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(repoPath));

        // Finally
        DeleteDirectory("basegroup");
        gitInitBareProcess.Dispose();
    }

    [Fact]
    public void RunExistingRepoInBaseGroupConfirmOverridesRepo()
    {
        // Given
        CreateDirectory("basegroup");
        var repoPath = Path.Combine("basegroup", "repo.git");
        var gitInitBareProcess = new GitInitBareProcess(repoPath);
        gitInitBareProcess.Start();

        SetInput(repoPath);
        var args = new string[] { "repo", $"--base-group=basegroup" };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.False(Directory.Exists(repoPath));

        // Finally
        DeleteDirectory("basegroup");
        gitInitBareProcess.Dispose();
    }
}
