namespace Server.GitShell.Lib.Utils;

public static class DirectoryUtils 
{
    private static string ToTmp(string dir)
    {
        return dir + "___tmp";
    }

    public static void RenameTmp(string dir)
    {
        if (Directory.Exists(dir)) Directory.Move(dir, ToTmp(dir));
    }

    public static void UndoRenameTmp(string dir) 
    {
        if (Directory.Exists(ToTmp(dir))) Directory.Move(ToTmp(dir), dir);
    }

    public static bool RemoveTmp(string dir)
    {
        if (Directory.Exists(ToTmp(dir))) {
            Directory.Delete(ToTmp(dir), true);
            return true;
        }
        return false;
    }
}