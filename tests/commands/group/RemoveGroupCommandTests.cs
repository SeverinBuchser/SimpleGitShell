using Server.GitShell.Commands.Group;
using Spectre.Console.Testing;

namespace Tests.Server.GitShell.Commands.Group;

[Collection("File System Sequential")]
public class RemoveGroupCommandTests : BaseGroupCommandTests
{
    private static CommandAppTester App()
    {
        var app = new CommandAppTester();
        app.SetDefaultCommand<RemoveGroupCommand>();
        return app;
    }
    
    [Fact]
    public void Execute_EmptyGroup_ThrowsEmptyGroupNameException()
    {
        // Given
        var args = new string[]{_InvalidGroup};

        // When
        var result = App().RunAndCatch<EmptyGroupNameException>(args);

        // Then
        Assert.IsType<EmptyGroupNameException>(result.Exception);
        Assert.Equal($"The name of the Group cannot be empty.", result.Exception.Message);
    }

    [Fact]
    public void Execute_ValidGroupNonExisting_ThrowsGroupDoesNotExistException() 
    {
        // Given
        var args = new string[]{_ValidGroup};

        // When
        var result = App().RunAndCatch<GroupDoesNotExistException>(args);

        // Then
        Assert.IsType<GroupDoesNotExistException>(result.Exception);
        Assert.Equal($"The group \"{_ValidGroup}\" does not exist.", result.Exception.Message);
    }

    [Fact]
    public void Execute_ValidGroupExistingEmpty_RemovesDirectory() 
    {
        // Given
        _CreateDirectory(_ValidGroup);
        var args = new string[]{_ValidGroup};

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.False(Directory.Exists(_ValidGroup));
    }

    [Fact]
    public void Execute_ValidGroupExistingNonEmpty_ThrowsGroupNonEmptyException() 
    {
        // Given
        _CreateNonEmptyDirectory(_ValidGroup, _SubDir);
        var args = new string[]{_ValidGroup};

        // When
        var result = App().RunAndCatch<GroupNonEmptyException>(args);

        // Then
        Assert.IsType<GroupNonEmptyException>(result.Exception);
        Assert.Equal($"The group \"{_ValidGroup}\" is not empty.", result.Exception.Message);
        
        // Finally
        _DeleteDirectory(_ValidGroup);
    }

    [Fact]
    public void Execute_EmptyGroupWithForce_ThrowsEmptyGroupNameException()
    {
        // Given
        var args = new string[]{_InvalidGroup, "-f"};

        // When
        var result = App().RunAndCatch<EmptyGroupNameException>(args);

        // Then
        Assert.IsType<EmptyGroupNameException>(result.Exception);
        Assert.Equal($"The name of the Group cannot be empty.", result.Exception.Message);
    }

    [Fact]
    public void Execute_ValidGroupNonExistingWithForce_ThrowsGroupDoesNotExistException() 
    {
        // Given
        var args = new string[]{_ValidGroup, "-f"};

        // When
        var result = App().RunAndCatch<GroupDoesNotExistException>(args);

        // Then
        Assert.IsType<GroupDoesNotExistException>(result.Exception);
        Assert.Equal($"The group \"{_ValidGroup}\" does not exist.", result.Exception.Message);
    }

    [Fact]
    public void Execute_ValidGroupExistingWithForce_RemovesDirectory() 
    {
        // Given
        _CreateDirectory(_ValidGroup);
        var args = new string[]{_ValidGroup, "-f"};

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.False(Directory.Exists(_ValidGroup));
    }

    [Fact]
    public void Execute_ValidGroupExistingNonEmptyWithForce_RemovesDirectory() 
    {
        // Given
        _CreateNonEmptyDirectory(_ValidGroup, _SubDir);
        var args = new string[]{_ValidGroup, "-f"};

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.False(Directory.Exists(_ValidGroup));
    }
} 