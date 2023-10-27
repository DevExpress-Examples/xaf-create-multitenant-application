using System.Diagnostics;
using System.Reactive.Linq;
using XAF.Testing.RX;

namespace XAF.Testing{
    public static class SystemExtensions{
        public static IObservable<string> WhenOutputDataReceived(this Process process)
            => process.WhenEvent<DataReceivedEventArgs>(nameof(Process.OutputDataReceived))
                .TakeUntil(process.WhenExited())
                .Select(pattern => pattern.Data);

        public static Process Start(this ProcessStartInfo processStartInfo) 
            => Process.Start(processStartInfo);

        public static IObservable<Process> WhenExited(this Process process){
            process.EnableRaisingEvents=true;
            return process.WhenEvent(nameof(Process.Exited)).Take(1).To(process);
        }

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

        public static T CreateInstance<T>(this Type type) => (T)CreateInstance(type);

        public static object CreateInstance(this Type type,params object[] args) => Activator.CreateInstance(type,args:args);

        public static void KillAll(this AppDomain appDomain,string processName) 
            => Process.GetProcessesByName(processName)
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

            await process.WhenOutputDataReceived().Where(outputReceived).Take(1).FirstOrDefaultAsync();
        }

    }
}