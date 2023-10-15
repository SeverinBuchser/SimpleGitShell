using System.CommandLine;
using Microsoft.Extensions.Logging;
using Server.GitShell.Lib.Logging;

namespace Server.GitShell.Commands.Group;

public class CreateGroupCommand : GroupBaseCommand
{
    public CreateGroupCommand() : base("create", "Creates a group on the Server.")
    {
        _ForceOption.Description = "Overrides group if it exists.";
        this.SetHandler(Handle, _GroupNameArgument, _ForceOption);
    }

    public static void Handle(string groupname, bool force)
    {
        if (string.IsNullOrEmpty(groupname))
        {
            throw new ArgumentException("Groupname cannot be empty.");
        }
        if (force && Directory.Exists(groupname))
        {
            Directory.Move(groupname, groupname + "___tmp");
        }
        else if (Directory.Exists(groupname))
        {
            throw new Exception($"The group \"{groupname}\" already exists.");
        }

        Directory.CreateDirectory(groupname);

        if (force && Directory.Exists(groupname + "___tmp")) 
        {
            Directory.Delete(groupname + "___tmp", true);
            CommandLogger.Default.LogWarning(
                "Group \"{groupname}\" already exists. Old group removed.", groupname
            );
        }

        CommandLogger.Default.LogInformation("Created group \"{groupname}\".", groupname);
    }
}