using SimpleGitShell.Commands.Group;
using Spectre.Console.Cli;
using Spectre.Console.Testing;
using Tests.SimpleGitShell.TestUtils;

namespace Tests.SimpleGitShell.Commands.Group;

[Collection("File System Sequential")]
public class RemoveGroupCommandTests : FileSystemTests
{
    private static CommandAppTester App()
    {
        var app = new CommandAppTester();
        app.SetDefaultCommand<RemoveGroupCommand>();
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
    public void RunInvalidGroupThrowsCommandRuntimeException(string group)
    {
        // Given
        var args = new string[] { group };

        // When
        var result = App().RunAndCatch<CommandRuntimeException>(args);

        // Then
        Assert.IsType<CommandRuntimeException>(result.Exception);
        Assert.Contains("group name", result.Exception.Message);
    }

    [Theory]
    [InlineData(" ")]
    [InlineData("$")]
    [InlineData("#")]
    [InlineData("\\")]
    [InlineData("(")]
    [InlineData("`")]
    [InlineData("_")]
    public void RunInvalidBaseGroupThrowsCommandRuntimeException(string basegroup)
    {
        // Given
        var args = new string[] { "group", $"--base-group={basegroup}" };

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
        var args = new string[] { "group", "--base-group=basegroup" };

        // When
        var result = App().RunAndCatch<CommandRuntimeException>(args);

        // Then
        Assert.IsType<CommandRuntimeException>(result.Exception);
        Assert.Contains("directory", result.Exception.Message);
        Assert.Contains("basegroup", result.Exception.Message);
    }

    [Fact]
    public void RunNonExistingGroupThrowsCommandRuntimeException()
    {
        // Given
        var args = new string[] { "group" };

        // When
        var result = App().RunAndCatch<CommandRuntimeException>(args);

        // Then
        Assert.IsType<CommandRuntimeException>(result.Exception);
        Assert.Contains("directory", result.Exception.Message);
        Assert.Contains("group", result.Exception.Message);
    }

    [Fact]
    public void RunNonExistingGroupInExistingBaseGroupThrowsCommandRuntimeException()
    {
        // Given
        CreateDirectory("basegroup");
        var args = new string[] { "group", "--base-group=basegroup" };

        // When
        var result = App().RunAndCatch<CommandRuntimeException>(args);

        // Then
        Assert.IsType<CommandRuntimeException>(result.Exception);
        Assert.Contains("directory", result.Exception.Message);
        Assert.Contains("group", result.Exception.Message);
        DeleteDirectory("basegroup");
    }

    [Fact]
    public void RunExistingGroupPromptsUserForConfirmation()
    {
        // Given
        CreateDirectory("group");
        SetInput("abort");
        var args = new string[] { "group" };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.Contains("confirm", CaptureWriter.ToString());

        // Finally
        DeleteDirectory("group");
    }

    [Fact]
    public void RunExistingGroupAbortDoesNotRemoveGroup()
    {
        // Given
        CreateDirectory("group");
        SetInput("abort");
        var args = new string[] { "group" };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists("group"));

        // Finally
        DeleteDirectory("group");
    }

    [Fact]
    public void RunExistingGroupConfirmRemovesGroup()
    {
        // Given
        CreateDirectory("group");
        SetInput(Path.Combine(".", "group"));
        var args = new string[] { "group" };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.False(Directory.Exists("group"));
    }

    [Fact]
    public void RunExistingGroupInGroupPromptsUserForConfirmation()
    {
        // Given
        CreateNonEmptyDirectory("basegroup", "group");
        SetInput("abort");
        var args = new string[] { "group", $"--base-group=basegroup" };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.Contains("confirm", CaptureWriter.ToString());

        // Finally
        DeleteDirectory("basegroup");
    }

    [Fact]
    public void RunExistingGroupInGroupAbortDoesNotOverrideGroup()
    {
        // Given
        var groupPath = Path.Combine("basegroup", "group");
        CreateDirectory(groupPath);

        SetInput("abort");
        var args = new string[] { "group", $"--base-group=basegroup" };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(groupPath));

        // Finally
        DeleteDirectory("basegroup");
    }

    [Fact]
    public void RunExistingGroupInGroupConfirmOverridesGroup()
    {
        // Given
        var groupPath = Path.Combine("basegroup", "group");
        CreateDirectory(groupPath);

        SetInput(groupPath);
        var args = new string[] { "group", $"--base-group=basegroup" };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.False(Directory.Exists(groupPath));

        // Finally
        DeleteDirectory("basegroup");
    }
}
