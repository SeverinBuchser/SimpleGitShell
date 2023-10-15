using Server.GitShell.Commands.Group;
using Spectre.Console.Cli;
using Tests.Server.GitShell.Utils;

namespace Tests.Server.GitShell.Commands.Group;

[Collection("File System Sequential")]
public class RemoveGroupCommandTests : BaseGroupCommandTests
{
    [Fact]
    public void Execute_EmptyGroupname_ThrowsException()
    {
        var context = new CommandContext(_remainingArgs, "", null);
        var settings = new RemoveGroupCommand.Settings {
            Groupname = _InvalidGroupname,
            Force = false
        };
        var command = new RemoveGroupCommand();
        Assert.Throws<ArgumentException>(() => command.Execute(context, settings));
    }

    [Fact]
    public void Execute_ValidGroupnameNonExisting_ThrowsException() 
    {
        var context = new CommandContext(_remainingArgs, "", null);
        var settings = new RemoveGroupCommand.Settings {
            Groupname = _ValidGroupname,
            Force = false
        };
        var command = new RemoveGroupCommand();
        Assert.Throws<Exception>(() => command.Execute(context, settings));
    }

    [Fact]
    public void Execute_ValidGroupnameExistingEmpty_RemovesDirectory() 
    {
        var context = new CommandContext(_remainingArgs, "", null);
        var settings = new RemoveGroupCommand.Settings {
            Groupname = _ValidGroupname,
            Force = false
        };
        var command = new RemoveGroupCommand();

        _CreateGroupDirectory(_ValidGroupname);
        Assert.True(Directory.Exists(_ValidGroupname));
        var result = command.Execute(context, settings);
        Assert.Equal(0, result);
        Assert.False(Directory.Exists(_ValidGroupname));
    }

    [Fact]
    public void Execute_ValidGroupnameExistingNonEmpty_ThrowsException() 
    {
        var context = new CommandContext(_remainingArgs, "", null);
        var settings = new RemoveGroupCommand.Settings {
            Groupname = _ValidGroupname,
            Force = false
        };
        var command = new RemoveGroupCommand();

        _CreateNonEmptyGroupDirectory(_ValidGroupname, _SubDir);
        Assert.True(Directory.Exists(_ValidGroupname));
        Assert.True(Directory.Exists(Path.Combine(_ValidGroupname, _SubDir)));
        Assert.Throws<Exception>(() => command.Execute(context, settings));
        Directory.Delete(_ValidGroupname, true);
    }

    [Fact]
    public void Execute_EmptyGroupnameWithForce_ThrowsException()
    {
        var context = new CommandContext(_remainingArgs, "", null);
        var settings = new RemoveGroupCommand.Settings {
            Groupname = _InvalidGroupname,
            Force = true
        };
        var command = new RemoveGroupCommand();
        Assert.Throws<ArgumentException>(() => command.Execute(context, settings));
    }

    [Fact]
    public void Execute_ValidGroupnameNonExistingWithForce_ThrowsException() 
    {
        var context = new CommandContext(_remainingArgs, "", null);
        var settings = new RemoveGroupCommand.Settings {
            Groupname = _ValidGroupname,
            Force = true
        };
        var command = new RemoveGroupCommand();
        Assert.Throws<Exception>(() => command.Execute(context, settings));
    }

    [Fact]
    public void Execute_ValidGroupnameExistingWithForce_RemovesDirectory() 
    {
        var context = new CommandContext(_remainingArgs, "", null);
        var settings = new RemoveGroupCommand.Settings {
            Groupname = _ValidGroupname,
            Force = true
        };
        var command = new RemoveGroupCommand();

        _CreateGroupDirectory(_ValidGroupname);
        Assert.True(Directory.Exists(_ValidGroupname));
        var result = command.Execute(context, settings);
        Assert.Equal(0, result);
        Assert.False(Directory.Exists(_ValidGroupname));
    }

    [Fact]
    public void Execute_ValidGroupnameExistingNonEmptyWithForce_RemovesDirectory() 
    {
        var context = new CommandContext(_remainingArgs, "", null);
        var settings = new RemoveGroupCommand.Settings {
            Groupname = _ValidGroupname,
            Force = true
        };
        var command = new RemoveGroupCommand();

        _CreateNonEmptyGroupDirectory(_ValidGroupname, _SubDir);
        Assert.True(Directory.Exists(_ValidGroupname));
        Assert.True(Directory.Exists(Path.Combine(_ValidGroupname, _SubDir)));
        var result = command.Execute(context, settings);
        Assert.Equal(0, result);
        Assert.False(Directory.Exists(_ValidGroupname));
    }
} 