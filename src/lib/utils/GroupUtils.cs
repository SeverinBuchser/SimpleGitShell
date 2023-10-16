using System.Diagnostics.CodeAnalysis;
using Server.GitShell.Lib.Exceptions.Group;

namespace Server.GitShell.Lib.Utils;

public static class GroupUtils {

    private static string ToTmp(string group)
    {
        return group + "___tmp";
    }

    public static void RenameTmp(string group)
    {
        if (Directory.Exists(group)) Directory.Move(group, ToTmp(group));
    }

    public static void UndoRenameTmp(string group) 
    {
        if (Directory.Exists(ToTmp(group))) Directory.Move(ToTmp(group), group);
    }

    public static bool RemoveTmp(string group)
    {
        if (Directory.Exists(ToTmp(group))) {
            Directory.Delete(ToTmp(group), true);
            return true;
        }
        return false;
    }

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