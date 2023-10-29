using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using SimpleGitShell.Commands.Base.Commands.List;
using SimpleGitShell.Commands.Base.Settings;
using SimpleGitShell.Library.Utils;

namespace SimpleGitShell.Commands.Group;

[Description("Lists all groups.")]
public class ListGroupCommand : AListCommandSettings<BaseGroupSettings>
{
    private string? BaseGroup;
    private string? BaseGroupPath;
    protected override string AvailableMessage => $"Available groups in base group \"{BaseGroup}\":";
    protected override string NoElementsMessage => $"There are no groups in base group \"{BaseGroup}\".";
    protected override IEnumerable<string> Columns => new string[] { "Group", "Creation Time" };

    protected override void PreExecute([NotNull] BaseGroupSettings settings)
    {
        BaseGroup = settings.CheckBaseGroupName();
        BaseGroupPath = BaseGroup != "root" ? BaseGroup : ".";
        GroupUtils.ThrowOnNonExistingGroup(BaseGroupPath);
    }

    protected override IEnumerable<string> GetElements()
    {
        return Directory.GetDirectories(BaseGroupPath!)
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
