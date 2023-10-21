using Server.GitShell.Commands.SSH.User;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Server.GitShell.Commands.SSH;

public class Program
{
    static int Main(string[] args)
    {
        var app = new CommandApp();
        app.Configure(config => 
        {
            config.AddBranch("user", config => 
            {
                config.AddCommand<AddSSHUserCommand>("add");
            });
        });
        try 
        {
            return app.Run(args);
        }
        catch (Exception e) {
            AnsiConsole.WriteException(e);
            return 128;
        }
    }
}