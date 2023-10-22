using System.Diagnostics;
using System.IO.Pipes;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using XAF.Testing.RX;

namespace XAF.Testing{
    public class Logger:Process{
        public static async Task<StreamWriter> Writer(LogContext context=default,WindowPosition inactiveMonitorLocation=WindowPosition.None,bool alwaysOnTop=false) 
            => await new Logger().ConnectClient(context,inactiveMonitorLocation,alwaysOnTop).Writer().ReplayFirstTake();

        public const string ExitSignal = "exit";
        public string PipeName{ get; set; } = nameof(Logger);
        public string ServerName{ get; set; } = ".";
        public TimeSpan ConnectionTimeout{ get; set; } = 5.Seconds();
        public string PowerShellName{ get; set; } = "pwsh.exe";
    }
    public static class LoggerExtensions{
        public static IObservable<NamedPipeClientStream> ConnectClient(this Logger logger,LogContext context=default,WindowPosition inactiveMonitorLocation=WindowPosition.None,bool alwaysOnTop=false){
            var startServer = logger.StartServer(context);
            return startServer.Connect().Do(_ => startServer.MoveToInactiveMonitor(inactiveMonitorLocation,alwaysOnTop));
        }

        private static Logger StartServer(this Logger logger,string condition=".*"){
            var script = $@"function Monitor-Pipe($condition) {{
                        $pipeName = '{logger.PipeName}'
                        $pipe = New-Object System.IO.Pipes.NamedPipeServerStream($pipeName)
                        $pipe.WaitForConnection()
                        $reader = New-Object System.IO.StreamReader($pipe)
                        $interactive=$false
                        while ($true) {{
                            $message = $reader.ReadLine()
                            Write-Host $message
                            if ($message -match $condition){{
                                $interactive=$true
                            }}
                            if ($message -ne '{Logger.ExitSignal}') {{
                                if ($interactive){{
                                    $null = $host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown')
                                }}
                            }}
                            else {{
                                $pipe.Dispose()
                                Start-Sleep -Seconds 5
                                Stop-Process -Id $PID
                            }}
                        }}
                    }}
                    Monitor-Pipe {condition}";
            logger.StartInfo.FileName = logger.PowerShellName;
            logger.StartInfo.Arguments = $"-NoExit -Command {script}";
            logger.StartInfo.UseShellExecute = true;
            Process.GetProcessesByName(Path.GetFileNameWithoutExtension(logger.PowerShellName)).ToArray().Do(process => process.Kill()).Enumerate();
            logger.Start();
            return logger;
        }


        public static IObservable<StreamWriter> Writer(this IObservable<NamedPipeClientStream> source) 
            => source.Select(client => client.Writer());

        public static StreamWriter Writer(this NamedPipeClientStream client){
            return new StreamWriter(client){AutoFlush = true};
        }
        
        public static IObservable<Unit> Write(this IObservable<string> source, NamedPipeClientStream client)
            => new StreamWriter(client).Use(writer => {
                writer.AutoFlush = true;
                return source.SelectManySequential(msg => writer.WriteLineAsync(msg).ToObservable());
            });
        public static IObservable<Unit> Write(this IObservable<NamedPipeClientStream> source, IObservable<string> messages) 
            => source.SelectMany(messages.Write);
        
        private static IObservable<NamedPipeClientStream> Connect(this Logger logger) 
            => Observable.Interval(100.Milliseconds())
                .Select(_ => new NamedPipeClientStream(logger.ServerName, logger.PipeName, PipeDirection.Out))
                .SelectManySequential(clientStream => clientStream.ConnectAsync().ToObservable()
                    .Catch<Unit,TimeoutException>(_ => Observable.Empty<Unit>()).To(clientStream))
                .Timeout(logger.ConnectionTimeout).WhenNotDefault(stream => stream.IsConnected).Take(1);

        [Obsolete]
        public static IObservable<T> Write<T>(this LogContext logContext, IObservable<T> source,
            WindowPosition inactiveMonitorLocation = WindowPosition.None, bool alwaysOnTop = false){
            logContext.Write(inactiveMonitorLocation,alwaysOnTop);
            return source;
        } 
        public static IObservable<T> Log<T>(this IObservable<T> source,LogContext logContext,
            WindowPosition inactiveMonitorLocation = WindowPosition.None, bool alwaysOnTop = false){
            logContext.Write(inactiveMonitorLocation,alwaysOnTop);
            return source.DoOnError(exception => Console.WriteLine(Logger.ExitSignal)).Finally(() => Console.WriteLine(Logger.ExitSignal));
        } 
        public static void Write(this LogContext logContext,WindowPosition inactiveMonitorLocation=WindowPosition.None,bool alwaysOnTop=false) 
            => logContext.Await(async () => Console.SetOut(await Logger.Writer(logContext,inactiveMonitorLocation,alwaysOnTop)));
    }

    public readonly struct LogContext : IEquatable<LogContext>{
        public override bool Equals(object obj) => obj is LogContext other && Equals(other);
        
        public override int GetHashCode() => CustomValue != null ? CustomValue.GetHashCode() : 0;

        private LogContext(string customValue) => CustomValue = customValue;

        public static LogContext All => new("All");
        public static LogContext None => new("None");

        public bool Equals(LogContext other) => CustomValue == other.CustomValue;

        public static bool operator ==(LogContext x, LogContext y){
            return x.Equals(y);
        }

        public static bool operator !=(LogContext x, LogContext y){
            return !x.Equals(y);
        }

        public string CustomValue{ get; }

        public static implicit operator LogContext(string s){
            return new LogContext(s);
        }

        public static implicit operator string(LogContext context) => context.CustomValue switch{
            nameof(All) => ".*",
            nameof(None) => Guid.NewGuid().ToString(),
            _ => context.CustomValue
        };

        
    }

}