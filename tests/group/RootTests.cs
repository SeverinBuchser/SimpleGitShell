using System.IO;
using Server.GitShell.Commands.Group;

namespace Server.GitShellCommands.Tests;

public class CreateGroupCommandTests
{
    [Fact]
    public void Handle_EmptyGroupname_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => CreateGroupCommand.Handle(""));
    }

    [Fact]
    public void Handle_NullGroupname_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => CreateGroupCommand.Handle(null));
    }

    [Fact]
    public void Handle_ValidGroupname_CreatesDirectory() 
    {
        CreateGroupCommand.Handle("groupname");
        var directory = Directory.Exists("./groupname");
        Console.WriteLine(directory);
        Console.WriteLine(Directory.GetCurrentDirectory());
    }
} 