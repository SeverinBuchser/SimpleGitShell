using System.Reflection;

namespace SimpleGitShellrary.Utils;

public static class VerstionUtils
{
    public static string InformationalVersion()
    {
        return Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;
    }
}
