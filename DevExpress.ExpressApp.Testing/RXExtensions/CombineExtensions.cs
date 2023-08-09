using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace DevExpress.ExpressApp.Testing.RXExtensions{
    public static class CombineExtensions{
        public static IObservable<T> SwitchIfEmpty<T>(this IObservable<T> source, IObservable<T> switchTo) {
            var signal = new AsyncSubject<Unit>();
            return source.Do(_ => {
                signal.OnNext(Unit.Default); 
                signal.OnCompleted();
            }).Concat(switchTo.TakeUntil(signal)); 
        }

        public static IObservable<T> MergeIgnored<T,T2>(this IObservable<T> source,Func<T,IObservable<T2>> secondSelector,Func<T,bool> merge=null)
            => source.Publish(obs => obs.SelectMany(arg => {
                merge ??= _ => true;
                var observable = Observable.Empty<T>();
                if (merge(arg)) {
                    observable = secondSelector(arg).IgnoreElements().To(arg);
                }
                return observable.Merge(arg.Observe());
            }));

        public static IObservable<Unit> MergeToUnit<TSource, TValue>(this IObservable<TSource> source, IObservable<TValue> value, IScheduler scheduler = null) 
            => source.ToUnit().Merge(value.ToUnit());
        
        public static IObservable<object> MergeToObject<TSource, TValue>(this IObservable<TSource> source, IObservable<TValue> value, IScheduler scheduler = null) where TValue:class 
            => source.Select(source1 => source1 as object).WhenNotDefault().Merge(value.To<TValue>());
    }
}