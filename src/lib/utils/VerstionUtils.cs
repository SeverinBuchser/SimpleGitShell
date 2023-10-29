using System.Reflection;

namespace SimpleGitShell.Library.Utils;

public class VerstionUtils
{
    public static string InformationalVersion()
    {
        return Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;
    }
}
