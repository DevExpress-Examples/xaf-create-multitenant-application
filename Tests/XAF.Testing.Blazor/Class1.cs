using System.Diagnostics;
using System.Reactive.Linq;
using XAF.Testing.RX;

namespace XAF.Testing.Blazor;
public static class Class1{
    public static IObservable<Process> MoveToInactiveMonitor(this IObservable<Process> source) 
        => source.Do(process => process.MainWindowHandle.UseInactiveMonitorBounds(rect =>
            process.MainWindowHandle.Move(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top)));

    public static IObservable<Process> Start(this Uri uri) 
        => new ProcessStartInfo{
            FileName = "chrome",
            Arguments = $"--user-data-dir={CreateTempProfilePath("chrome")} {uri}",
            UseShellExecute = true
        }.Start().Observe().Delay(TimeSpan.FromSeconds(1));

    private static string CreateTempProfilePath(string name){
        var path = $"{Path.GetTempPath()}\\{name}";
        if (Directory.Exists(path)){
            Directory.Delete(path,true);
        }
        Directory.CreateDirectory(path);
        return path;
    }
}