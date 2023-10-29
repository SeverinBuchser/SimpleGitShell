using System.Diagnostics.CodeAnalysis;
using SimpleGitShell.Library.Logging;
using Spectre.Console.Cli;

namespace SimpleGitShell.Commands.Base.List;

public abstract class AListCommand : Command
{
    protected abstract string AvailableMessage { get; }
    protected abstract string NoElementsMessage { get; }
    protected abstract string[] Columns { get; }
    protected abstract IEnumerable<string> GetList();
    protected abstract string[] ToRow(string element);

    public override int Execute([NotNull] CommandContext context)
    {
        Logger.Instance.Info(AvailableMessage);
        var list = GetList();
        if (!list.Any())
        {
            Logger.Instance.Info(NoElementsMessage);
        }
        else
        {
            var rows = new List<string[]>();
            foreach (var element in list)
            {
                rows.Add(ToRow(element));
            }
            Logger.Instance.Table(Columns, rows);
        }
        return 0;
    }
}
