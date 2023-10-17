using System.Diagnostics.CodeAnalysis;
using Server.GitShell.Lib.Exceptions.Group;

namespace Server.GitShell.Lib.Utils;

public static class GroupUtils 
{
    public static void ThrowOnEmptyGroupName([NotNullWhen(true)] string? group)
    {
        if (string.IsNullOrEmpty(group)) throw new EmptyGroupNameException();
    }

    public static void ThrowOnExistingGroup(string group) 
    {
        if (Directory.Exists(group)) throw new GroupAlreadyExistsException(group);
    }

    public static void ThrowOnNonExistingGroup(string group) 
    {
        if (!Directory.Exists(group)) throw new GroupDoesNotExistException(group);
    }

    public static void ThrowOnNonEmptyGroup(string group)
    {
        if(Directory.EnumerateFileSystemEntries(group).Any()) throw new GroupNonEmptyException(group);
    }
}