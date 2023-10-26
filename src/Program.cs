using SimpleGitShell.Commands.Group;
using SimpleGitShell.Commands.Repo;
using SimpleGitShell.Commands.SSH.User;
using SimpleGitShell.Library.Logging;
using SimpleGitShell.Library.Utils.Processes;
using Spectre.Console.Cli;

namespace SimpleGitShell;

public class Program
{
    private static int Main(string[] args)
    {
        if (args.Length == 2 && args[0] == "-c")
        {
            args = args[1].Split(" ").Select(arg => arg.Replace("'", "\"")).ToArray();
            switch (args[0])
            {
                case "git-receive-pack":
                case "git-upload-pack":
                    var process = new StdandardInputProcess(args[0], string.Join(" ", args.Skip(1)));
                    process.Start();
                    process.WaitForExit();
                    return process.ExitCode;

                default:
                    return Shell(args);
            }
        }
        return 1;
    }

    private static int Shell(string[] args)
    {
        var app = BuildShell();
        try
        {
            return app.Run(args);
        }

#pragma warning disable CA1031
        catch (Exception e)
#pragma warning restore CA1031
        {
            Logger.Instance.Error(e.Message);
            return 128;
        }
    }

    private static CommandApp BuildShell()
    {
        var app = new CommandApp();
        app.Configure(config =>
        {
            // TODO
            config.SetApplicationVersion("___VERSION___");
            config.AddBranch("group", config =>
            {
                config.AddCommand<ListGroupCommand>("list");
                config.AddCommand<CreateGroupCommand>("create");
                config.AddCommand<RemoveGroupCommand>("remove");
            });

            config.AddBranch("repo", config =>
            {
                config.AddCommand<ListRepoCommand>("list");
                config.AddCommand<CreateRepoCommand>("create");
                config.AddCommand<RemoveRepoCommand>("remove");
            });
            config.AddBranch("ssh", config =>
            {
                config.AddBranch("user", config =>
                {
                    config.AddCommand<AddSSHUserCommand>("add");
                    config.AddCommand<RemoveSSHUserCommand>("remove");
                });
            });
        });
        return app;
    }
}
