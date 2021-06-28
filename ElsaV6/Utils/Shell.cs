using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ElsaV6.Utils
{
    public class Shell
    {
        public static string Command(string command)
        {
#if _WINDOWS
            var shell = System
                .Management
                .Automation
                .PowerShell
                .Create(System.Management.Automation.RunspaceMode.NewRunspace)
                .AddScript(command)
                .Invoke();

            return shell.FirstOrDefault()?.ToString();
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
