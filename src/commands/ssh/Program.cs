using Server.GitShell.Commands.SSH.User;
using Server.GitShell.Lib.Logging;
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
                config.AddCommand<RemoveSSHUserCommand>("remove");
            });
        });
        try 
        {
            return app.Run(args);
        }
        catch (Exception e) {
            Logger.Instance.Error(e.Message);
            return 128;
        }
    }
}