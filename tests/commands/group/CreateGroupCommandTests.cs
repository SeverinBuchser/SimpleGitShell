using Server.GitShell.Commands.Group;
using Server.GitShell.Lib.Exceptions.Group;
using Spectre.Console.Testing;

namespace Tests.Server.GitShell.Commands.Group;

[Collection("File System Sequential")]
public class CreateGroupCommandTests : BaseGroupCommandTests
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
        var args = new string[]{_InvalidGroup};
        
        // When
        var result = App().RunAndCatch<EmptyGroupNameException>(args);

        // Then
        Assert.IsType<EmptyGroupNameException>(result.Exception);
    }

    [Fact]
    public void Run_ValidGroupNonExisting_CreatesDirectory() 
    {
        // Given
        var args = new string[]{_ValidGroup};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(_ValidGroup));
    }

    [Fact]
    public void Run_ValidGroupExisting_ThrowsGroupAlreadyExistsException() 
    {
        // Given
        _CreateDirectory(_ValidGroup);
        var args = new string[]{_ValidGroup};
        
        // When
        var result = App().RunAndCatch<GroupAlreadyExistsException>(args);

        // Then
        Assert.IsType<GroupAlreadyExistsException>(result.Exception);
    }

    [Fact]
    public void Run_ValidGroupNonExistingWithForce_CreatesDirectory() 
    {
        // Given
        var args = new string[]{_ValidGroup, "-f"};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(_ValidGroup));
    }

    [Fact]
    public void Run_ValidGroupExistingWithForce_OverridesDirectory() 
    {
        // Given
        _CreateNonEmptyDirectory(_ValidGroup, _SubDir);
        var args = new string[]{_ValidGroup, "-f"};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(_ValidGroup));
        Assert.False(Directory.Exists(Path.Combine(_ValidGroup, _SubDir)));
    }

    [Fact]
    public void Run_NonExistingBaseGroup_ThrowsGroupDoesNotExistException() 
    {
        // Given
        var args = new string[]{_ValidGroup, $"-b={ _ValidGroup }"};
        
        // When
        var result = App().RunAndCatch<GroupDoesNotExistException>(args);

        // Then
        Assert.IsType<GroupDoesNotExistException>(result.Exception);
    }

    [Fact]
    public void Run_ExistingBaseGroup_CreatesDirectoryInGroup() 
    {
        // Given
        _CreateDirectory(_ValidGroup);
        var args = new string[]{_ValidGroup, $"-b={ _ValidGroup }"};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(Path.Combine(_ValidGroup, _ValidGroup)));
    }

    [Fact]
    public void Run_ExistingGroupInBaseGroup_ThrowsGroupAlreadyExistsException() 
    {
        // Given
        _CreateNonEmptyDirectory(_ValidGroup, _ValidGroup);
        var args = new string[]{_ValidGroup, $"-b={ _ValidGroup }"};
        
        // When
        var result = App().RunAndCatch<GroupAlreadyExistsException>(args);

        // Then
        Assert.IsType<GroupAlreadyExistsException>(result.Exception);
    }

    [Fact]
    public void Run_ExistingGroupInBaseGroupWithForce_OverridesGroupInBaseGroup() 
    {
        // Given
        _CreateNonEmptyDirectory(_ValidGroup, _ValidGroup);
        _CreateNonEmptyDirectory(Path.Combine(_ValidGroup, _ValidGroup), _SubDir);
        var args = new string[]{_ValidGroup, $"-b={ _ValidGroup }", "-f"};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(Path.Combine(_ValidGroup, _ValidGroup)));
        Assert.False(Directory.Exists(Path.Combine(_ValidGroup, _ValidGroup, _SubDir)));
    }


} 