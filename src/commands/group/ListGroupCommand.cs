using System.ComponentModel;
using SimpleGitShell.Commands.Base.Commands.List;
using SimpleGitShell.Commands.Base.Settings;

namespace SimpleGitShell.Commands.Group;

[Description("Lists all groups.")]
public class ListGroupCommand : AListCommand<BaseGroupSettings>
{
    protected override string AvailableMessage => $"Available groups in base group \"{Settings!.BaseGroup}\":";
    protected override string NoElementsMessage => $"There are no groups in base group \"{Settings!.BaseGroup}\".";
    protected override IEnumerable<string> Columns => new string[] { "Group", "Creation Time" };

    protected override IEnumerable<string> GetElements()
    {
        return Directory.GetDirectories(Settings!.BaseGroupPath)
            .Where(dir => !dir.EndsWith(".git") && !dir.Contains("/.") && !dir.Equals("./git-shell-commands"))
            .OrderBy(s => s);
    }

    protected override string[] ToRow(string element)
    {
        return new string[] {
            Path.GetFileName(element),
            Directory.GetCreationTime(element).ToString("dd/MM/yyyy HH:mm:ss")
        };
    }
}
