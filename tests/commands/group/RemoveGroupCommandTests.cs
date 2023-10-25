using SimpleGitShell.Commands.Group;
using SimpleGitShell.Lib.Exceptions.Group;
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
    public void Run_EmptyGroup_ThrowsEmptyGroupNameException()
    {
        // Given
        var args = new string[]{""};
        
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
    public void Run_InvalidGroup_ThrowsGroupNameNotValidException(string group)
    {
        // Given
        var args = new string[]{group};
        
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
    public void Run_InvalidBaseGroup_ThrowsGroupNameNotValidException(string basegroup)
    {
        // Given
        var args = new string[]{"group", $"--base-group={ basegroup }"};
        
        // When
        var result = App().RunAndCatch<GroupNameNotValidException>(args);

        // Then
        Assert.IsType<GroupNameNotValidException>(result.Exception);
    }

    [Fact]
    public void Run_NonExistingBaseGroup_ThrowsGroupDoesNotExistException()
    {
        // Given
        var args = new string[]{"group", "--base-group=basegroup"};
        
        // When
        var result = App().RunAndCatch<GroupDoesNotExistException>(args);

        // Then
        Assert.IsType<GroupDoesNotExistException>(result.Exception);
    }

    [Fact]
    public void Run_NonExistingGroup_ThrowsGroupDoesNotExistException()
    {
        // Given
        var args = new string[]{"group"};
        
        // When
        var result = App().RunAndCatch<GroupDoesNotExistException>(args);

        // Then
        Assert.IsType<GroupDoesNotExistException>(result.Exception);
    }

    [Fact]
    public void Run_NonExistingGroupInExistingBaseGroup_ThrowsGroupDoesNotExistException()
    {
        // Given
        _CreateDirectory("basegroup");
        var args = new string[]{"group", "--base-group=basegroup"};
        
        // When
        var result = App().RunAndCatch<GroupDoesNotExistException>(args);

        // Then
        Assert.IsType<GroupDoesNotExistException>(result.Exception);
        _DeleteDirectory("basegroup");
    }

    [Fact]
    public void Run_ExistingGroup_PromptsUserForConfirmation()
    {
        // Given
        _CreateDirectory("group");
        _SetInput("abort");
        var args = new string[]{"group"};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.Contains("confirm", _CaptureWriter.ToString());
        
        // Finally
        _DeleteDirectory("group");
    }

    [Fact]
    public void Run_ExistingGroupAbort_DoesNotRemoveGroup()
    {
        // Given
        _CreateDirectory("group");
        _SetInput("abort");
        var args = new string[]{"group"};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists("group"));
        
        // Finally
        _DeleteDirectory("group");
    }

    [Fact]
    public void Run_ExistingGroupConfirm_RemovesGroup()
    {
        // Given
        _CreateDirectory("group");
        _SetInput(Path.Combine(".", "group"));
        var args = new string[]{"group"};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.False(Directory.Exists("group"));
    }
    
    [Fact]
    public void Run_ExistingGroupInGroup_PromptsUserForConfirmation()
    {
        // Given
        _CreateNonEmptyDirectory("basegroup", "group");
        _SetInput("abort");
        var args = new string[]{"group", $"--base-group=basegroup"};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.Contains("confirm", _CaptureWriter.ToString());
        
        // Finally
        _DeleteDirectory("basegroup");
    }

    [Fact]
    public void Run_ExistingGroupInGroupAbort_DoesNotOverrideGroup()
    {
        // Given
        var groupPath = Path.Combine("basegroup", "group");
        _CreateDirectory(groupPath);

        _SetInput("abort");
        var args = new string[]{"group", $"--base-group=basegroup"};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(groupPath));
        
        // Finally
        _DeleteDirectory("basegroup");
    }

    [Fact]
    public void Run_ExistingGroupInGroupConfirm_OverridesGroup()
    {
        // Given
        var groupPath = Path.Combine("basegroup", "group");
        _CreateDirectory(groupPath);

        _SetInput(groupPath);
        var args = new string[]{"group", $"--base-group=basegroup"};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.False(Directory.Exists(groupPath));
        
        // Finally
        _DeleteDirectory("basegroup");
    }
} 