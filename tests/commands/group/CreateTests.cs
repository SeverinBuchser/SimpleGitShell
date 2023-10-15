using Server.GitShell.Commands.Group;
using Spectre.Console.Cli;

namespace Tests.Server.GitShell.Commands.Group;

[Collection("File System Sequential")]
public class CreateGroupCommandTests : BaseGroupCommandTests
{
    [Fact]
    public void Execute_EmptyGroupname_ThrowsException()
    {
        var context = new CommandContext(_remainingArgs, "", null);
        var settings = new CreateGroupCommand.Settings {
            Groupname = _InvalidGroupname,
            Force = false
        };
        var command = new CreateGroupCommand();
        Assert.Throws<ArgumentException>(() => command.Execute(context, settings));
    }

    [Fact]
    public void Execute_ValidGroupnameNonExisting_CreatesDirectory() 
    {
        var context = new CommandContext(_remainingArgs, "", null);
        var settings = new CreateGroupCommand.Settings {
            Groupname = _ValidGroupname,
            Force = false
        };
        var command = new CreateGroupCommand();
        var result = command.Execute(context, settings);
        Assert.Equal(0, result);
        Assert.True(Directory.Exists(_ValidGroupname));
        Directory.Delete(_ValidGroupname);
    }

    [Fact]
    public void Execute_ValidGroupnameExisting_ThrowsException() 
    {
        var context = new CommandContext(_remainingArgs, "", null);
        var settings = new CreateGroupCommand.Settings {
            Groupname = _ValidGroupname,
            Force = false
        };
        var command = new CreateGroupCommand();

        _CreateGroupDirectory(_ValidGroupname);
        Assert.True(Directory.Exists(_ValidGroupname));
        Assert.Throws<Exception>(() => command.Execute(context, settings));
        Directory.Delete(_ValidGroupname);
    }

    [Fact]
    public void Execute_ValidGroupnameNonExistingWithForce_CreatesDirectory() 
    {
        var context = new CommandContext(_remainingArgs, "", null);
        var settings = new CreateGroupCommand.Settings {
            Groupname = _ValidGroupname,
            Force = true
        };
        var command = new CreateGroupCommand();
        var result = command.Execute(context, settings);
        Assert.Equal(0, result);
        Assert.True(Directory.Exists(_ValidGroupname));
        Directory.Delete(_ValidGroupname);
    }

    [Fact]
    public void Execute_ValidGroupnameExistingWithForce_OverridesDirectory() 
    {
        var context = new CommandContext(_remainingArgs, "", null);
        var settings = new CreateGroupCommand.Settings {
            Groupname = _ValidGroupname,
            Force = true
        };
        var command = new CreateGroupCommand();

        _CreateGroupDirectory(_ValidGroupname);
        Assert.True(Directory.Exists(_ValidGroupname));
        var result = command.Execute(context, settings);
        Assert.Equal(0, result);
        Assert.True(Directory.Exists(_ValidGroupname));
        Directory.Delete(_ValidGroupname);
    }
} 