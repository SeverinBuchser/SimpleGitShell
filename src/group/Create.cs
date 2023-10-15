using System.CommandLine;
using System.IO;

namespace Server.GitShell.Commands.Group;

public class CreateGroupCommand : Command
{
    public CreateGroupCommand() : base("create", "Creates a group on the Server.")
    {
        var groupnameArgument = new Argument<string>(
            name: "groupname",
            description: "The name of the Group."
        );
        AddArgument(groupnameArgument);
        this.SetHandler(Handle, groupnameArgument);
    }

    public static void Handle(string groupname) {
        if (string.IsNullOrEmpty(groupname)) 
        {
            throw new ArgumentException("Groupname cannot be empty.");
        }
        
        if (Directory.Exists(groupname))
        {
            throw new Exception($"The group \"{groupname}\" already exists.");
        }
        
        Directory.CreateDirectory(groupname);
    }
}