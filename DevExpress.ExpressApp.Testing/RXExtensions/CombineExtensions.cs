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

        public static IObservable<Unit> MergeToUnit<TSource, TValue>(this IObservable<TSource> source, IObservable<TValue> value, IScheduler scheduler = null) 
            => source.ToUnit().Merge(value.ToUnit());
    }
}