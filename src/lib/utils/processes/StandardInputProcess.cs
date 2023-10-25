using System.Diagnostics;

namespace Server.GitShell.Lib.Utils.Processes;

public class StdandardInputProcess : Process
{
    public StdandardInputProcess(string fileName) : this(fileName, "") {}
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
