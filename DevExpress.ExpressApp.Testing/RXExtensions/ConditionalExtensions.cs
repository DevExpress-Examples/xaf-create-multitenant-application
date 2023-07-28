using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace DevExpress.ExpressApp.Testing.RXExtensions{
    public static class ConditionalExtensions{
        public static IConnectableObservable<T> TakeAndReplay<T>(this IObservable<T> source, int count)
            => source.Take(count).Replay(count);
        
        public static IObservable<T> TakeUntilCompleted<T,T2>(this IObservable<T> source, IObservable<T2> next)
            => source.TakeUntil(next.WhenCompleted());

        public static IObservable<TResult> If<TSource, TResult>(this IObservable<TSource> source,
            Func<int,TSource, bool> predicate, Func<TSource, IObservable<TResult>> thenSource, Func<TSource, IObservable<TResult>> elseSource) 
            => source.SelectMany((value, i) => predicate(i,value) ? thenSource(value) : elseSource(value));

        public static IObservable<TResult> If<TSource, TResult>(this IObservable<TSource> source,
            Func<TSource, bool> predicate, Func<TSource, IObservable<TResult>> thenSource, Func<TSource, IObservable<TResult>> elseSource) 
            => source.SelectMany(value => predicate(value) ? thenSource(value) : elseSource(value));

        public static IObservable<TResult> If<TSource, TResult>(this IObservable<TSource> source,
            Func<TSource, bool> predicate, Func<TSource, IObservable<TResult>> thenSource) 
            => source.SelectMany(value => predicate(value) ? thenSource(value) :Observable.Empty<TResult>());
        
        public static IObservable<TResult> If<TSource, TResult>(this IObservable<TSource> source,
            Func<TSource, bool> predicate, Func<IObservable<TResult>> thenSource, Func< IObservable<TResult>> elseSource) 
            => source.If(predicate, _ => thenSource(),_ => elseSource());
        
        public static IObservable<TSource> TakeWhileInclusive<TSource>(this IObservable<TSource> source, Func<TSource, bool> predicate) 
            => source.TakeUntil(source.SkipWhile(predicate).Skip(1));
    }
}