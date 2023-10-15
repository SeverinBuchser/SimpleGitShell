using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Server.GitShell.Lib.Logging;
using Spectre.Console.Cli;

namespace Server.GitShell.Commands.Group;

[Description("Creates a group.")]
public class CreateGroupCommand : Command<CreateGroupCommand.Settings>
{
    public class Settings : BaseGroupCommandSettings
    {
        [Description("Overrides group if it already exists.")]
        [CommandOption("-f|--force")]
        [DefaultValue(false)]
        public bool Force { get; init; }
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        if (string.IsNullOrEmpty(settings.Groupname))
        {
            throw new ArgumentException("Groupname cannot be empty.");
        }
        if (settings.Force && Directory.Exists(settings.Groupname))
        {
            Directory.Move(settings.Groupname, settings.Groupname + "___tmp");
        }
        else if (Directory.Exists(settings.Groupname))
        {
            throw new Exception($"The group \"{settings.Groupname}\" already exists.");
        }

        Directory.CreateDirectory(settings.Groupname);

        if (settings.Force && Directory.Exists(settings.Groupname + "___tmp")) 
        {
            Directory.Delete(settings.Groupname + "___tmp", true);
            CommandLogger.Default.LogWarning(
                "Group \"{groupname}\" already exists. Old group removed.", settings.Groupname
            );
        }

        CommandLogger.Default.LogInformation("Created group \"{groupname}\".", settings.Groupname);
        return 0;
    }
}