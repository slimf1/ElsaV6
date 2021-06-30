using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Text;

namespace ElsaV6.Utils
{
    public class Shell
    {
        public static string Command(string command)
        {
#if _WINDOWS
            var shellResult = PowerShell
                                .Create(RunspaceMode.NewRunspace)
                                .AddScript(command)
                                .Invoke();

            return string.Join("\n", shellResult);
#else
            var escapedArgs = command.Replace("\"", "\\\"");
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            var result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return result;
#endif
        }
    }
}
