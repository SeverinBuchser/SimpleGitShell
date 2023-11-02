using System.Reflection;

namespace SimpleGitShell.Utils;

public static class VerstionUtils
{
    public static string InformationalVersion()
    {
        return Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;
    }
}
