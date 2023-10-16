using Microsoft.Extensions.DependencyInjection;
using Server.GitShell.Commands.Group;
using Server.GitShell.Lib.Infrastructure;
using Spectre.Console;
using Spectre.Console.Cli;
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
    public void Execute_EmptyGroup_ThrowsException()
    {
        // Given
        var args = new string[]{_InvalidGroup};
        
        // When
        var result = App().RunAndCatch<ArgumentException>(args);

        // Then
        Assert.IsType<ArgumentException>(result.Exception);
        Assert.Equal($"The name of the Group cannot be empty.", result.Exception.Message);
    }

    [Fact]
    public void Execute_ValidGroupNonExisting_CreatesDirectory() 
    {
        // Given
        var args = new string[]{_ValidGroup};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(_ValidGroup));

        // Finally
        _DeleteDirectory(_ValidGroup);
    }

    [Fact]
    public void Execute_ValidGroupExisting_ThrowsException() 
    {
        // Given
        _CreateDirectory(_ValidGroup);
        var args = new string[]{_ValidGroup};
        
        // When
        var result = App().RunAndCatch<Exception>(args);

        // Then
        Assert.IsType<Exception>(result.Exception);
        Assert.Equal($"The group \"{_ValidGroup}\" already exists.", result.Exception.Message);

        // Finally
        _DeleteDirectory(_ValidGroup);
    }

    [Fact]
    public void Execute_ValidGroupNonExistingWithForce_CreatesDirectory() 
    {
        // Given
        var args = new string[]{_ValidGroup, "-f"};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(_ValidGroup));

        // Finally
        _DeleteDirectory(_ValidGroup);
    }

    [Fact]
    public void Execute_ValidGroupExistingWithForce_OverridesDirectory() 
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

        // Finally
        _DeleteDirectory(_ValidGroup);
    }
} 