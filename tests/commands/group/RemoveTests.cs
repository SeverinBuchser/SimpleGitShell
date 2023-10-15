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
    public void Execute_EmptyGroupname_ThrowsException()
    {
        // Given
        var args = new string[]{_InvalidGroupname};

        // When
        var result = App().RunAndCatch<Exception>(args);

        // Then
        Assert.IsType<ArgumentException>(result.Exception);
        Assert.Equal($"Groupname cannot be empty.", result.Exception.Message);
    }

    [Fact]
    public void Execute_ValidGroupnameNonExisting_ThrowsException() 
    {
        // Given
        var args = new string[]{_ValidGroupname};

        // When
        var result = App().RunAndCatch<Exception>(args);

        // Then
        Assert.IsType<Exception>(result.Exception);
        Assert.Equal($"The group \"{_ValidGroupname}\" does not exist.", result.Exception.Message);
    }

    [Fact]
    public void Execute_ValidGroupnameExistingEmpty_RemovesDirectory() 
    {
        // Given
        _CreateDirectory(_ValidGroupname);
        var args = new string[]{_ValidGroupname};

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.False(Directory.Exists(_ValidGroupname));
    }

    [Fact]
    public void Execute_ValidGroupnameExistingNonEmpty_ThrowsException() 
    {
        // Given
        _CreateNonEmptyDirectory(_ValidGroupname, _SubDir);
        var args = new string[]{_ValidGroupname};

        // When
        var result = App().RunAndCatch<Exception>(args);

        // Then
        Assert.IsType<Exception>(result.Exception);
        Assert.Equal($"The group \"{_ValidGroupname}\" is not empty. To remove anyway use option \"-f\".", result.Exception.Message);
        
        // Finally
        _DeleteDirectory(_ValidGroupname);
    }

    [Fact]
    public void Execute_EmptyGroupnameWithForce_ThrowsException()
    {
        // Given
        var args = new string[]{_InvalidGroupname, "-f"};

        // When
        var result = App().RunAndCatch<Exception>(args);

        // Then
        Assert.IsType<ArgumentException>(result.Exception);
        Assert.Equal($"Groupname cannot be empty.", result.Exception.Message);
    }

    [Fact]
    public void Execute_ValidGroupnameNonExistingWithForce_ThrowsException() 
    {
        // Given
        var args = new string[]{_ValidGroupname, "-f"};

        // When
        var result = App().RunAndCatch<Exception>(args);

        // Then
        Assert.IsType<Exception>(result.Exception);
        Assert.Equal($"The group \"{_ValidGroupname}\" does not exist.", result.Exception.Message);
    }

    [Fact]
    public void Execute_ValidGroupnameExistingWithForce_RemovesDirectory() 
    {
        // Given
        _CreateDirectory(_ValidGroupname);
        var args = new string[]{_ValidGroupname, "-f"};

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.False(Directory.Exists(_ValidGroupname));
    }

    [Fact]
    public void Execute_ValidGroupnameExistingNonEmptyWithForce_RemovesDirectory() 
    {
        // Given
        _CreateNonEmptyDirectory(_ValidGroupname, _SubDir);
        var args = new string[]{_ValidGroupname, "-f"};

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.False(Directory.Exists(_ValidGroupname));
    }
} 