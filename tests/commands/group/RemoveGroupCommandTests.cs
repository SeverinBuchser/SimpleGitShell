using SimpleGitShell.Commands.Group;
using SimpleGitShell.Library.Exceptions.Group;
using Spectre.Console.Testing;
using Tests.SimpleGitShell.Utils;

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

    [Fact]
    public void RunEmptyGroupThrowsEmptyGroupNameException()
    {
        // Given
        var args = new string[] { "" };

        // When
        var result = App().RunAndCatch<EmptyGroupNameException>(args);

        // Then
        Assert.IsType<EmptyGroupNameException>(result.Exception);
    }

    [Theory]
    [InlineData("$")]
    [InlineData("#")]
    [InlineData("\\")]
    [InlineData("(")]
    [InlineData("`")]
    [InlineData("_")]
    public void RunInvalidGroupThrowsGroupNameNotValidException(string group)
    {
        // Given
        var args = new string[] { group };

        // When
        var result = App().RunAndCatch<GroupNameNotValidException>(args);

        // Then
        Assert.IsType<GroupNameNotValidException>(result.Exception);
    }

    [Theory]
    [InlineData("$")]
    [InlineData("#")]
    [InlineData("\\")]
    [InlineData("(")]
    [InlineData("`")]
    [InlineData("_")]
    public void RunInvalidBaseGroupThrowsGroupNameNotValidException(string basegroup)
    {
        // Given
        var args = new string[] { "group", $"--base-group={basegroup}" };

        // When
        var result = App().RunAndCatch<GroupNameNotValidException>(args);

        // Then
        Assert.IsType<GroupNameNotValidException>(result.Exception);
    }

    [Fact]
    public void RunNonExistingBaseGroupThrowsGroupDoesNotExistException()
    {
        // Given
        var args = new string[] { "group", "--base-group=basegroup" };

        // When
        var result = App().RunAndCatch<GroupDoesNotExistException>(args);

        // Then
        Assert.IsType<GroupDoesNotExistException>(result.Exception);
    }

    [Fact]
    public void RunNonExistingGroupThrowsGroupDoesNotExistException()
    {
        // Given
        var args = new string[] { "group" };

        // When
        var result = App().RunAndCatch<GroupDoesNotExistException>(args);

        // Then
        Assert.IsType<GroupDoesNotExistException>(result.Exception);
    }

    [Fact]
    public void RunNonExistingGroupInExistingBaseGroupThrowsGroupDoesNotExistException()
    {
        // Given
        CreateDirectory("basegroup");
        var args = new string[] { "group", "--base-group=basegroup" };

        // When
        var result = App().RunAndCatch<GroupDoesNotExistException>(args);

        // Then
        Assert.IsType<GroupDoesNotExistException>(result.Exception);
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
