using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace XAF.Testing.RX{
    public static class BufferExtensions{
        public static IObservable<IList<T>> BufferUntilInactive<T>(this IObservable<T> source, TimeSpan delay,IScheduler scheduler=null)
            => source.BufferUntilInactive(delay,window => window.ToList(),scheduler);
        
        public static IObservable<IList<T>> BufferUntilInactive<T>(this IObservable<T> source, TimeSpan delay,Func<IObservable<T>,IObservable<IList<T>>> resultSelector,IScheduler scheduler=null)
            => source.Publish(obs => obs.Window(() => obs.Throttle(delay,scheduler??Scheduler.Default)).SelectMany(resultSelector));
        
        public static IObservable<TSource[]> BufferUntilCompleted<TSource>(this IObservable<TSource> source,bool skipEmpty=false) 
            => source.Buffer(Observable.Never<Unit>()).Where(sources => !skipEmpty || sources.Any()).Select(list => list.ToArray());
    }
}