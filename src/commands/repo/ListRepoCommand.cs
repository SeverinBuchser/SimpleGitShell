using System.ComponentModel;
using SimpleGitShell.Commands.Base.Commands.List;
using SimpleGitShell.Commands.Base.Settings;

namespace SimpleGitShell.Commands.Repo;

[Description("Lists all repositories.")]
public class ListRepoCommand : AListCommand<BaseGroupSettings>
{
    protected override string AvailableMessage => $"Available repositories in base group \"{Settings!.BaseGroup}\":";
    protected override string NoElementsMessage => $"There are no repositories in base group \"{Settings!.BaseGroup}\".";
    protected override IEnumerable<string> Columns => new string[] { "Repository", "Creation Time" };
    protected override IEnumerable<string> GetElements()
    {
        return Directory.GetDirectories(Settings!.BaseGroupPath)
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
