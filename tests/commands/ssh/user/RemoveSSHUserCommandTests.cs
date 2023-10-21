using Server.GitShell.Commands.SSH.User;
using Spectre.Console.Testing;
using Tests.Server.GitShell.Utils;

namespace Tests.Server.GitShell.Commands.SSH.User;

[Collection("File System Sequential")]
public class RemoveSSHUserCommandTests : FileSystemTests
{
    private static CommandAppTester App()
    {
        var app = new CommandAppTester();
        app.SetDefaultCommand<RemoveSSHUserCommand>();
        return app;
    }
} 