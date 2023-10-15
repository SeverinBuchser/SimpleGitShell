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
    public void Execute_EmptyGroupname_ThrowsException()
    {
        // Given
        var args = new string[]{_InvalidGroupname};
        
        // When
        var result = App().RunAndCatch<ArgumentException>(args);

        // Then
        Assert.IsType<ArgumentException>(result.Exception);
        Assert.Equal("Groupname cannot be empty.", result.Exception.Message);
    }

    [Fact]
    public void Execute_ValidGroupnameNonExisting_CreatesDirectory() 
    {
        // Given
        var args = new string[]{_ValidGroupname};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(_ValidGroupname));

        // Finally
        _DeleteDirectory(_ValidGroupname);
    }

    [Fact]
    public void Execute_ValidGroupnameExisting_ThrowsException() 
    {
        // Given
        _CreateDirectory(_ValidGroupname);
        var args = new string[]{_ValidGroupname};
        
        // When
        var result = App().RunAndCatch<Exception>(args);

        // Then
        Assert.IsType<Exception>(result.Exception);
        Assert.Equal($"The group \"{_ValidGroupname}\" already exists.", result.Exception.Message);

        // Finally
        _DeleteDirectory(_ValidGroupname);
    }

    [Fact]
    public void Execute_ValidGroupnameNonExistingWithForce_CreatesDirectory() 
    {
        // Given
        var args = new string[]{_ValidGroupname, "-f"};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(_ValidGroupname));

        // Finally
        _DeleteDirectory(_ValidGroupname);
    }

    [Fact]
    public void Execute_ValidGroupnameExistingWithForce_OverridesDirectory() 
    {
        // Given
        _CreateNonEmptyDirectory(_ValidGroupname, _SubDir);
        var args = new string[]{_ValidGroupname, "-f"};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(_ValidGroupname));
        Assert.False(Directory.Exists(Path.Combine(_ValidGroupname, _SubDir)));

        // Finally
        _DeleteDirectory(_ValidGroupname);
    }
} 