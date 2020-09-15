using System.Diagnostics;

namespace CQRSAndMediator.Scaffolding.Utilities
{
    public static class ExecuteCommandUtility
    {
        public static string Run(string command)
        {
            var procStartInfo = new ProcessStartInfo("cmd", "/c " + command)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            // wrap IDisposable into using (in order to release hProcess) 
            using var process = new Process { StartInfo = procStartInfo };
            process.Start();

            // Add this: wait until process does its work
            process.WaitForExit();

            // and only then read the result
            var result = process.StandardOutput.ReadToEnd();
            return result;
        }
    }
}
