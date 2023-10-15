using Server.GitShell.Commands.Group;
using Tests.Server.GitShell.Utils;

namespace Tests.Server.GitShell.Commands.Group;

public class CreateGroupCommandTests : DisableConsole
{
    private static string _ValidGroupname = "groupname";
    private static string _InvalidGroupname = "";

    public override void Dispose()
    {
        base.Dispose();
        if (Directory.Exists(_ValidGroupname)) Directory.Delete(_ValidGroupname, true);
    }

    [Fact]
    public void Handle_EmptyGroupname_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => CreateGroupCommand.Handle(_InvalidGroupname, false));
    }

    [Fact]
    public void Handle_ValidGroupnameNonExisting_CreatesDirectory() 
    {
        CreateGroupCommand.Handle(_ValidGroupname, false);
        Assert.True(Directory.Exists(_ValidGroupname));
        Directory.Delete(_ValidGroupname);
    }

    [Fact]
    public void Handle_ValidGroupnameExisting_ThrowsException() 
    {
        CreateGroupCommand.Handle(_ValidGroupname, false);
        Assert.True(Directory.Exists(_ValidGroupname));
        Assert.Throws<Exception>(() => CreateGroupCommand.Handle(_ValidGroupname, false));
        Directory.Delete(_ValidGroupname);
    }

    [Fact]
    public void Handle_ValidGroupnameNonExistingWithForce_CreatesDirectory() 
    {
        CreateGroupCommand.Handle(_ValidGroupname, true);
        Assert.True(Directory.Exists(_ValidGroupname));
        Directory.Delete(_ValidGroupname);
    }

    [Fact]
    public void Handle_ValidGroupnameExistingWithForce_OverridesDirectory() 
    {
        CreateGroupCommand.Handle(_ValidGroupname, false);
        Assert.True(Directory.Exists(_ValidGroupname));
        CreateGroupCommand.Handle(_ValidGroupname, true);
        Assert.True(Directory.Exists(_ValidGroupname));
        Directory.Delete(_ValidGroupname);
    }
} 