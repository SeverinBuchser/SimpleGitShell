using Server.GitShell.Commands.Group;
using Spectre.Console.Cli;
using Tests.Server.GitShell.Utils;

namespace Tests.Server.GitShell.Commands.Group;

[Collection("File System Sequential")]
public class CreateGroupCommandTests : DisableConsole
{
    private static string _ValidGroupname = "groupname";
    private static string _InvalidGroupname = "";
    private readonly IRemainingArguments _remainingArgs = new Mock<IRemainingArguments>().Object;

    public override void Dispose()
    {
        base.Dispose();
        if (Directory.Exists(_ValidGroupname)) Directory.Delete(_ValidGroupname, true);
    }

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

        var resultFirstRun = command.Execute(context, settings);
        Assert.Equal(0, resultFirstRun);
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

        var resultFirstRun = command.Execute(context, settings);
        Assert.Equal(0, resultFirstRun);
        Assert.True(Directory.Exists(_ValidGroupname));
        var resultSecondRun = command.Execute(context, settings);
        Assert.Equal(0, resultSecondRun);
        Assert.True(Directory.Exists(_ValidGroupname));
        Directory.Delete(_ValidGroupname);
    }
} 