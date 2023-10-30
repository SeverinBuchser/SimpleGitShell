using System.ComponentModel;
using SimpleGitShell.Commands.Base.Commands.List;
using SimpleGitShellrary.Utils;

namespace SimpleGitShell.Commands.SSH.User;

[Description("Lists all SSH users.")]
public class ListSSHUserCommand : AListCommand
{
    protected override string AvailableMessage => $"Available ssh users:";
    protected override string NoElementsMessage => $"There are no ssh users.";
    protected override IEnumerable<string> Columns => new string[] { "Key" };
    protected override IEnumerable<string> GetElements()
    {
        return SSHUtils.ReadKeys();
    }

    protected override string[] ToRow(string element)
    {
        return new string[] {
            Path.GetFileName(SSHUtils.Comment(element))
        };
    }
}
