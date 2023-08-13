using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Runtime.CompilerServices;
using Unit = System.Reactive.Unit;

namespace DevExpress.ExpressApp.Testing.RXExtensions{
    public static class TransformExtension{
        public static IObservable<T2> SelectManySequential<T1, T2>(this IObservable<T1> source, Func<T1, IObservable<T2>> selector) 
            => source.Select(x => Observable.Defer(() => selector(x))).Concat();
        public static IObservable<T2> SelectManySequential<T1, T2>(this IObservable<T1> source, Func<T1, Task<T2>> selector) 
            => source.SelectManySequential(arg => selector(arg).ToObservable());
        
        public static IObservable<T2> SelectManySequential<T1, T2>(this IObservable<T1> source, Func<T1,int, IObservable<T2>> selector) 
            => source.Select((x,i) => Observable.Defer(() => selector(x,i))).Concat();
        
        public static IObservable<T> ThrowIfEmpty<T>(this IObservable<T> source,[CallerMemberName]string caller="")
            => source.SwitchIfEmpty(Observable.Defer(() => Observable.Throw<T>(new SequenceIsEmptyException($"source is empty {caller}"))));
        public static IObservable<T> ConcatDefer<T>(this IObservable<T> source, Func<IObservable<T>> target)
            => source.Concat(Observable.Defer(target));
        
        public static IObservable<Unit> ConcatToUnit<T,T1>(this IObservable<T> source, IObservable<T1> target)
            => source.ToUnit().Concat(Observable.Defer(target.ToUnit));
        public static IObservable<T[]> WhenCompleted<T>(this IObservable<T> source) 
            => source.IgnoreElements().Select(_ => Array.Empty<T>()).Concat(Array.Empty<T>().Observe(Scheduler.CurrentThread));
        public static IObservable<T> Observe<T>(this T self, IScheduler scheduler = null) 
            => Observable.Return(self, scheduler??ImmediateScheduler.Instance);

        public static IObservable<TValue> To<TSource,TValue>(this IObservable<TSource> source,TValue value) 
            => source.Select(_ => value);

        public static IObservable<T> To<T>(this IObservable<object> source) 
            => source.Select(o =>o is T arg? arg: default);
        
        public static IObservable<T> To<T>(this IObservable<Unit> source) 
            => source.Select(_ => default(T));
        

        public static IObservable<T> To<T,TResult>(this IObservable<TResult> source) 
            => source.Select(_ => default(T)).WhenNotDefault();
        
        public static IObservable<T1> ToFirst<T1, T2>(this IObservable<(T1, T2)> source) 
            => source.Select(tuple => tuple.Item1);

        public static IObservable<T2> ToSecond<T1, T2>(this IObservable<(T1, T2)> source) 
            => source.Select(tuple => tuple.Item2);

        public static IObservable<T1> ToFirst<T1, T2, T3>(this IObservable<(T1, T2, T3)> source) 
            => source.Select(tuple => tuple.Item1);

        public static IObservable<T2> ToSecond<T1, T2, T3>(this IObservable<(T1, T2, T3)> source) 
            => source.Select(tuple => tuple.Item2);

        public static IObservable<T3> ToThird<T1, T2, T3>(this IObservable<(T1, T2, T3)> source) 
            => source.Select(tuple => tuple.Item3);

        public static IObservable<T1> ToFirst<T1, T2, T3, T4>(this IObservable<(T1, T2, T3, T4)> source) 
            => source.Select(tuple => tuple.Item1);

        public static IObservable<T2> ToSecond<T1, T2, T3, T4>(this IObservable<(T1, T2, T3, T4)> source) 
            => source.Select(tuple => tuple.Item2);

        public static IObservable<T3> ToThird<T1, T2, T3, T4>(this IObservable<(T1, T2, T3, T4)> source) 
            => source.Select(tuple => tuple.Item3);

        public static IObservable<T4> ToFourth<T1, T2, T3, T4>(this IObservable<(T1, T2, T3, T4)> source) 
            => source.Select(tuple => tuple.Item4);

        public static IObservable<T1> ToFirst<T1, T2, T3, T4, T5>(this IObservable<(T1, T2, T3, T4, T5)> source) 
            => source.Select(tuple => tuple.Item1);

        public static IObservable<T2> ToSecond<T1, T2, T3, T4, T5>(this IObservable<(T1, T2, T3, T4, T5)> source) 
            => source.Select(tuple => tuple.Item2);

        public static IObservable<T3> ToThird<T1, T2, T3, T4, T5>(this IObservable<(T1, T2, T3, T4, T5)> source) 
            => source.Select(tuple => tuple.Item3);

        public static IObservable<T4> ToFourth<T1, T2, T3, T4, T5>(this IObservable<(T1, T2, T3, T4, T5)> source) 
            => source.Select(tuple => tuple.Item4);

        public static IObservable<T5> ToFifth<T1, T2, T3, T4, T5>(this IObservable<(T1, T2, T3, T4, T5)> source) 
            => source.Select(tuple => tuple.Item5);
        
        public static IObservable<(TValue value, TSource source)> InversePair<TSource, TValue>(this IObservable<TSource> source, TValue value) 
            => source.Select(_ => ( value,_));
        
        public static IObservable<(TSource source, TValue other)> Pair<TSource, TValue>(this IObservable<TSource> source, TValue value) 
            => source.Select(_ => (_, value));
        
        public static IObservable<Unit> ToUnit<T>(this IObservable<T> source) 
            => source.Select(_ => Unit.Default);
        public static IEnumerable<Unit> ToUnit<T>(this IEnumerable<T> source)
            => source.Select(_ => Unit.Default);
        
        public static IObservable<Unit> ToUnit<T1, T2>(this IObservable<(T1, T2)> source) 
            => source.Select(_ => Unit.Default);

        public static IObservable<Unit> ToUnit<T1, T2, T3>(this IObservable<(T1, T2, T3)> source) 
            => source.Select(_ => Unit.Default);

        public static IObservable<Unit> ToUnit<T1, T2, T3, T4>(this IObservable<(T1, T2, T3, T4)> source) 
            => source.Select(_ => Unit.Default);

        public static IObservable<Unit> ToUnit<T1, T2, T3, T4, T5>(this IObservable<(T1, T2, T3, T4, T5)> source) 
            => source.Select(_ => Unit.Default);

        public static IObservable<T> WaitUntilInactive<T>(this IObservable<T> source, TimeSpan timeSpan,int count =1,IScheduler scheduler=null) 
            =>timeSpan==TimeSpan.Zero?source: source.BufferUntilInactive(timeSpan,scheduler:scheduler).SelectMany(list => list.TakeLast(count).ToArray());
        
        public static IObservable<T> WaitUntilInactive<T>(this IObservable<T> source, int seconds, int count = 1,IScheduler scheduler=null)
            => source.WaitUntilInactive(TimeSpan.FromSeconds(seconds), count,scheduler);
        
        public static IObservable<TSource> SelectMany<TSource>(this IObservable<IObservable<TSource>> source) 
            => source.SelectMany(source1 => source1);
        
        public static IObservable<TSource> SelectMany<TSource>(this IObservable<IEnumerable<TSource>> source) 
            => source.SelectMany(source1 => source1.ToNowObservable());
        public static IObservable<TSource> SelectMany<TSource>(this IObservable<IEnumerable<TSource>> source,int take) 
            => source.SelectMany(source1 => source1.Take(take).ToNowObservable());
        
        public static IObservable<TSource> ToObservable<TSource>(this IEnumerable<TSource> source, SynchronizationContext context)
            => source.ToObservable().ObserveOn(context);
        public static IObservable<TSource> ToNowObservable<TSource>(this IEnumerable<TSource> source)
            => source.ToObservable(ImmediateScheduler.Instance);
        
        public static IObservable<T> DoNotComplete<T>(this IObservable<T> source) => source.Concat(Observable.Never<T>());
        
    }
    public class SequenceIsEmptyException:Exception {
        public SequenceIsEmptyException(string message) : base(message){
        }
    }

}