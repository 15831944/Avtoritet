namespace NewLauncher.Utility
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    internal class Utility
    {
        public void ExecuteCommandAsync(string command)
        {
            try
            {
                new Thread(new ParameterizedThreadStart(this.ExecuteCommandSync)) { IsBackground = true, Priority = ThreadPriority.AboveNormal }.Start(command);
            }
            catch (ThreadStartException)
            {
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception)
            {
            }
        }

        public void ExecuteCommandSync(object command)
        {
            try
            {
                ProcessStartInfo info = new ProcessStartInfo("cmd", "/c " + command) {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                Process process = new Process {
                    StartInfo = info
                };
                process.Start();
                Console.WriteLine(process.StandardOutput.ReadToEnd());
            }
            catch (Exception)
            {
            }
        }
    }
}

