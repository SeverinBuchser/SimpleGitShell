using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using SimpleGitShell.Commands.Base.List;
using SimpleGitShell.Commands.Repo.Settings;
using SimpleGitShell.Library.Utils;

namespace SimpleGitShell.Commands.Repo;

[Description("Lists all repositories.")]
public class ListRepoCommand : AListCommandSettings<BaseRepoCommandSettings>
{
    private string? BaseGroup;
    private string? BaseGroupPath;
    protected override string AvailableMessage => $"Available repositories in base group \"{BaseGroup}\":";

    protected override string NoElementsMessage => $"There are no repositories in base group \"{BaseGroup}\".";

    protected override string[] Columns => new string[] { "Repository", "Creation Time" };

    protected override void PreExecute([NotNull] BaseRepoCommandSettings settings)
    {
        BaseGroup = settings.CheckBaseGroupName();
        BaseGroupPath = BaseGroup != "root" ? BaseGroup : ".";
        GroupUtils.ThrowOnNonExistingGroup(BaseGroupPath);
    }

    protected override IEnumerable<string> GetList()
    {
        return Directory.GetDirectories(BaseGroupPath!)
            .Where(dir => dir.EndsWith(".git"))
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
