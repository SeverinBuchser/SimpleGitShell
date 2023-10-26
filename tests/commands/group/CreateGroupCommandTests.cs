using SimpleGitShell.Commands.Group;
using SimpleGitShell.Library.Exceptions.Group;
using Spectre.Console.Testing;
using Tests.SimpleGitShell.Utils;

namespace Tests.SimpleGitShell.Commands.Group;

[Collection("File System Sequential")]
public class CreateGroupCommandTests : FileSystemTests
{
    private static CommandAppTester App()
    {
        var app = new CommandAppTester();
        app.SetDefaultCommand<CreateGroupCommand>();
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
    public void RunInvalidBaseGroupThrowsGroupNameNotValidException(string group)
    {
        // Given
        var args = new string[] { "group", $"--base-group={group}" };

        // When
        var result = App().RunAndCatch<GroupNameNotValidException>(args);

        // Then
        Assert.IsType<GroupNameNotValidException>(result.Exception);
    }

    [Fact]
    public void RunValidGroupNonExistingCreatesDirectory()
    {
        // Given
        var args = new string[] { "group" };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists("group"));
        DeleteDirectory("group");
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
    public void RunExistingGroupAbortDoesNotOverrideGroup()
    {
        // Given
        var subDirPath = Path.Combine("group", "subdir");
        CreateDirectory(subDirPath);
        SetInput("abort");
        var args = new string[] { "group" };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(subDirPath));

        // Finally
        DeleteDirectory("group");
    }

    [Fact]
    public void RunExistingGroupConfirmOverridesGroup()
    {
        // Given
        var subDirPath = Path.Combine("group", "subdir");
        CreateDirectory(subDirPath);
        SetInput(Path.Combine(".", "group"));
        var args = new string[] { "group" };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.False(Directory.Exists(subDirPath));

        // Finally
        DeleteDirectory("group");
    }

    [Fact]
    public void RunNonExistingBaseGroupThrowsGroupDoesNotExistException()
    {
        // Given
        var args = new string[] { "group", $"--base-group=basegroup" };

        // When
        var result = App().RunAndCatch<GroupDoesNotExistException>(args);

        // Then
        Assert.IsType<GroupDoesNotExistException>(result.Exception);
    }

    [Fact]
    public void RunExistingBaseGroupCreatesGroupInBaseGroup()
    {
        // Given
        CreateDirectory("basegroup");
        var args = new string[] { "group", $"--base-group=basegroup" };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(Path.Combine("basegroup", "group")));
    }
    [Fact]
    public void RunExistingGroupInBaseGroupPromptsUserForConfirmation()
    {
        // Given
        var groupPath = Path.Combine("basegroup", "group");
        CreateDirectory(groupPath);
        CreateNonEmptyDirectory(groupPath, "subdir");
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
    public void RunExistingGroupInBaseGroupAbortDoesNotOverrideRepo()
    {
        // Given
        var groupPath = Path.Combine("basegroup", "group");
        CreateDirectory("basegroup");
        CreateDirectory(groupPath);

        var subDirPath = Path.Combine(groupPath, "subdir");
        CreateDirectory(subDirPath);

        SetInput("abort");
        var args = new string[] { "group", $"--base-group=basegroup" };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(subDirPath));

        // Finally
        DeleteDirectory("basegroup");
    }

    [Fact]
    public void RunExistingGroupInBaseGroupConfirmOverridesRepo()
    {
        // Given
        var groupPath = Path.Combine("basegroup", "group");
        CreateDirectory("basegroup");
        CreateDirectory(groupPath);

        var subDirPath = Path.Combine(groupPath, "subdir");
        CreateDirectory(subDirPath);

        SetInput(groupPath);
        var args = new string[] { "group", $"--base-group=basegroup" };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.False(Directory.Exists(subDirPath));

        // Finally
        DeleteDirectory("basegroup");
    }
}
