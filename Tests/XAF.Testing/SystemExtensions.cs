using System.Diagnostics;
using System.IO;
using System.Reactive.Linq;
using XAF.Testing.RX;

namespace XAF.Testing{
    public static class SystemExtensions{
        public static IObservable<string> WhenOutputDataReceived(this Process process)
            => process.WhenEvent<DataReceivedEventArgs>(nameof(Process.OutputDataReceived))
                .TakeUntil(process.WhenExited())
                .Select(pattern => pattern.Data);
        
        public static IObservable<Process> WhenExited(this Process process) 
            => process.WhenEvent(nameof(Process.Exited)).Take(1).To(process);
        
        public static bool StartWithEvents(this Process process,bool outputDataReceived=true,bool outputErrorReceived=true,bool enableRaisingEvents=true,bool createNoWindow=true){
            process.StartInfo.RedirectStandardOutput = outputDataReceived;
            process.StartInfo.RedirectStandardError = outputErrorReceived;
            if (outputDataReceived||outputErrorReceived){
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = createNoWindow;
            }
            var start = process.Start();
            process.EnableRaisingEvents = enableRaisingEvents;
            if (start&&outputDataReceived){
                process.BeginOutputReadLine();    
            }
            if (start&&outputErrorReceived){
                process.BeginErrorReadLine();    
            }
            return start;
        }

        public static void KillAll(this AppDomain appDomain) 
            => Process.GetProcessesByName("OutlookInspired.MiddleTier")
                .Do(process => {
                    process.Kill();
                    process.WaitForExit();
                }).Enumerate();

        public static async Task RunDotNet(this AppDomain appDomain,string path,string configuration,Func<string,bool> outputReceived){
            var process = new Process{
                StartInfo = new ProcessStartInfo{
                    FileName = "dotnet",
                    Arguments = $"run --configuration {configuration}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = Path.GetFullPath($"{appDomain.BaseDirectory}{path}")
                }
            };

            process.StartWithEvents();

            await process.WhenOutputDataReceived().TakeFirst(outputReceived);
        }

    }
}