using Server.GitShell.Commands.Group;
using Server.GitShell.Lib.Exceptions.Group;
using Spectre.Console.Testing;
using Tests.Server.GitShell.Utils;

namespace Tests.Server.GitShell.Commands.Group;

[Collection("File System Sequential")]
public class CreateGroupCommandTests : FileSystemCommandTests
{
    private static CommandAppTester App()
    {
        var app = new CommandAppTester();
        app.SetDefaultCommand<CreateGroupCommand>();
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
    public void Run_InvalidBaseGroup_ThrowsGroupNameNotValidException(string group)
    {
        // Given
        var args = new string[]{"group", $"--base-group={ group }"};
        
        // When
        var result = App().RunAndCatch<GroupNameNotValidException>(args);

        // Then
        Assert.IsType<GroupNameNotValidException>(result.Exception);
    }

    [Fact]
    public void Run_ValidGroupNonExisting_CreatesDirectory() 
    {
        // Given
        var args = new string[]{"group"};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists("group"));
        _DeleteDirectory("group");
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
    public void Run_ExistingGroupAbort_DoesNotOverrideGroup()
    {
        // Given
        var subDirPath = Path.Combine("group", "subdir");
        _CreateDirectory(subDirPath);
        _SetInput("abort");
        var args = new string[]{"group"};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(subDirPath));
        
        // Finally
        _DeleteDirectory("group");
    }

    [Fact]
    public void Run_ExistingGroupConfirm_OverridesGroup()
    {
        // Given
        var subDirPath = Path.Combine("group", "subdir");
        _CreateDirectory(subDirPath);
        _SetInput(Path.Combine(".", "group"));
        var args = new string[]{"group"};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.False(Directory.Exists(subDirPath));
        
        // Finally
        _DeleteDirectory("group");
    }

    [Fact]
    public void Run_NonExistingBaseGroup_ThrowsGroupDoesNotExistException() 
    {
        // Given
        var args = new string[]{"group", $"--base-group=basegroup"};
        
        // When
        var result = App().RunAndCatch<GroupDoesNotExistException>(args);

        // Then
        Assert.IsType<GroupDoesNotExistException>(result.Exception);
    }

    [Fact]
    public void Run_ExistingBaseGroup_CreatesGroupInBaseGroup() 
    {
        // Given
        _CreateDirectory("basegroup");
        var args = new string[]{"group", $"--base-group=basegroup"};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(Path.Combine("basegroup", "group")));
    }
[Fact]
    public void Run_ExistingGroupInBaseGroup_PromptsUserForConfirmation()
    {
        // Given
        var groupPath = Path.Combine("basegroup", "group");
        _CreateDirectory(groupPath);
        _CreateNonEmptyDirectory(groupPath, "subdir");
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
    public void Run_ExistingGroupInBaseGroupAbort_DoesNotOverrideRepo()
    {
        // Given
        var groupPath = Path.Combine("basegroup", "group");
        _CreateDirectory("basegroup");
        _CreateDirectory(groupPath);

        var subDirPath = Path.Combine(groupPath, "subdir");
        _CreateDirectory(subDirPath);

        _SetInput("abort");
        var args = new string[]{"group", $"--base-group=basegroup"};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(subDirPath));
        
        // Finally
        _DeleteDirectory("basegroup");
    }

    [Fact]
    public void Run_ExistingGroupInBaseGroupConfirm_OverridesRepo()
    {
        // Given
        var groupPath = Path.Combine("basegroup", "group");
        _CreateDirectory("basegroup");
        _CreateDirectory(groupPath);

        var subDirPath = Path.Combine(groupPath, "subdir");
        _CreateDirectory(subDirPath);
        
        _SetInput(groupPath);
        var args = new string[]{"group", $"--base-group=basegroup"};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.False(Directory.Exists(subDirPath));
        
        // Finally
        _DeleteDirectory("basegroup");
    }
} 