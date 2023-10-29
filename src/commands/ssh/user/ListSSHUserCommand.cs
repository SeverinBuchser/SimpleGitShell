using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using SimpleGitShell.Library.Logging;
using SimpleGitShell.Library.Utils;
using Spectre.Console.Cli;

namespace SimpleGitShell.Commands.SSH.User;

[Description("Lists all SSH users.")]
public class ListSSHUserCommand : Command
{

    public override int Execute([NotNull] CommandContext context)
    {

        Logger.Instance.Info($"Registered ssh users:");
        var keys = SSHUtils.ReadKeys();
        if (!keys.Any())
        {
            Logger.Instance.Info($"There are no ssh users.");
        }
        else
        {
            var rows = new List<string[]>();
            foreach (var key in keys)
            {
                rows.Add(new string[] {
                    Path.GetFileName(key)
                });
            }
            Logger.Instance.Table(new string[] { "Key" }, rows);
        }
        return 0;
    }
}
