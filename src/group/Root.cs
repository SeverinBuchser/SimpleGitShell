using System.CommandLine;

namespace Server.GitShell.Commands.Group;

class GroupCommand : BaseRootCommand
{

    public GroupCommand() : base("Manager for groups.") 
    {
        AddCommand(new CreateGroupCommand());
    }

    static int Main(string[] args)
    {
        return new GroupCommand().Invoke(args);   
    }
}