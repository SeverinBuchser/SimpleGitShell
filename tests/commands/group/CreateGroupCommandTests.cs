using SimpleGitShell.Commands.Group;
using Spectre.Console.Cli;
using Spectre.Console.Testing;
using Tests.SimpleGitShell.TestUtils;

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
    public void RunInvalidBaseGroupThrowsCommandRuntimeException(string baseGroup)
    {
        // Given
        var args = new string[] { "group", $"--base-group={baseGroup}" };

        // When
        var result = App().RunAndCatch<CommandRuntimeException>(args);

        // Then
        Assert.IsType<CommandRuntimeException>(result.Exception);
        Assert.Contains("base group name", result.Exception.Message);
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
    public void RunNonExistingBaseGroupThrowsCommandRuntimeException()
    {
        // Given
        var args = new string[] { "group", $"--base-group=basegroup" };

        // When
        var result = App().RunAndCatch<CommandRuntimeException>(args);

        // Then
        Assert.IsType<CommandRuntimeException>(result.Exception);
        Assert.Contains("directory", result.Exception.Message);
        Assert.Contains("basegroup", result.Exception.Message);
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
