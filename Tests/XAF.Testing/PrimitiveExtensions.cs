using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Reactive.Linq;
using System.Text;
using Aqua.EnumerableExtensions;
using static XAF.Testing.WinInterop;

namespace XAF.Testing{

    public static class PrimitiveExtensions{
        private static readonly Random Random = new(DateTime.Now.Millisecond);
        public static int GetAvailablePort(this IPEndPoint[] endPoints,int startRange = 1024, int endRange = 49151) 
            => startRange.Range( endRange - startRange).ToArray().OrderBy(_ => Random.Next()).ToArray()
                .First(port => endPoints.All(endPoint => endPoint.Port != port));

        public static string ReverseStackTrace(this Exception exception) => $"{exception.FromHierarchy(exception1 => exception1.InnerException).Select(exception1 =>$"{exception1.StackTrace}" ).Reverse().StringJoin(Environment.NewLine)}";

        public static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source, Func<TSource, TSource> nextItem) where TSource : class 
            => source.FromHierarchy( nextItem, s => s != null);

        public static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source, Func<TSource, TSource> nextItem, Func<TSource, bool> canContinue){
            for (var current = source; canContinue(current); current = nextItem(current)) yield return current;
        }
        public static IObservable<T> MoveToInactiveMonitor<T>(this IObservable<T> source,WindowPosition position = WindowPosition.None,bool alwaysOnTop=false) where T:Process 
            => source.Select(process => process.MoveToInactiveMonitor(position,alwaysOnTop));


        public static T MoveToInactiveMonitor<T>(this T process, WindowPosition position = WindowPosition.None,bool alwaysOnTop=false) where T : Process{
            if (position != WindowPosition.None){
                process.MainWindowHandle.UseInactiveMonitorBounds(rect => {
                    for (var i = 0; i < 2; i++){
                        GetWindowRect(process.MainWindowHandle, out var currentRect);
                        var currentWidth = currentRect.Right - currentRect.Left;
                        var currentHeight = currentRect.Bottom - currentRect.Top;
                        var (x, y, width, height) = position switch{
                            WindowPosition.FullScreen => (rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top),
                            WindowPosition.BottomRight => (rect.Right - currentWidth, rect.Bottom - currentHeight, currentWidth, currentHeight),
                            WindowPosition.BottomLeft => (rect.Left, rect.Bottom - currentHeight, currentWidth, currentHeight),
                            _ => throw new ArgumentOutOfRangeException(nameof(position), position, null)
                        };
                        process.MainWindowHandle.Move(x, y, width, height);
                    }
                });    
            }
            process.MainWindowHandle.AlwaysOnTop(alwaysOnTop);
            return process;
        }
        public static void WriteSection(this string text){
            var dashCount = text.Length + 12; 
            var dashes = new string('-', dashCount);
            Console.WriteLine(dashes);
            Console.WriteLine($"##[section]{text}");
            Console.WriteLine(dashes);
        }
        
        public static byte[] Bytes(this string s, Encoding encoding = null) 
            => s == null ? Array.Empty<byte>() : (encoding ?? Encoding.UTF8).GetBytes(s);
        
        public static void Await(this object any, Func<Task> invoker) {
            var originalContext = SynchronizationContext.Current;
            try {
                var context = new SingleThreadedSynchronizationContext();
                SynchronizationContext.SetSynchronizationContext(context);
                var task = invoker.Invoke();
                task.ContinueWith(_ => context.Queue.CompleteAdding());
                while (context.Queue.TryTake(out var work, Timeout.Infinite))
                    work.d.Invoke(work.state);
                task.GetAwaiter().GetResult();
            }
            finally {
                SynchronizationContext.SetSynchronizationContext(originalContext);
            }
        }

        internal sealed class SingleThreadedSynchronizationContext : SynchronizationContext {
            public BlockingCollection<(SendOrPostCallback d, object state)> Queue{ get; } = new();

            public override void Post(SendOrPostCallback d, object state){
                if (!Queue.IsAddingCompleted) {
                    Queue.Add((d, state));
                }
            }
        }

    }
    public enum WindowPosition{
        None,
        FullScreen,
        BottomRight,
        BottomLeft
    }

}