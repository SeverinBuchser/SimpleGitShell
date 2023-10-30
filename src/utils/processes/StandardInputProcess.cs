using System.Diagnostics;

namespace SimpleGitShellrary.Utils.Processes;

public class StdandardInputProcess : Process
{
    public StdandardInputProcess(string fileName, string args)
    {
        StartInfo = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = args,
            UseShellExecute = false,
            CreateNoWindow = true
        };
    }
}
