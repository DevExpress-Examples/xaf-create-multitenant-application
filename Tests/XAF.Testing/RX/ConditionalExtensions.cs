using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace XAF.Testing.RX{
    public static class ConditionalExtensions{
        public static IObservable<T> TakeFirst<T>(this IObservable<T> source, Func<T, bool> predicate)
            => source.Where(predicate).Take(1);
        public static IConnectableObservable<T> TakeAndReplay<T>(this IObservable<T> source, int count)
            => source.Take(count).Replay(count);
        
        public static IObservable<T> TakeUntilCompleted<T,T2>(this IObservable<T> source, IObservable<T2> next)
            => source.TakeUntil(next.WhenCompleted());
        public static IObservable<T> TakeUntilFinished<T,T2>(this IObservable<T> source, IObservable<T2> next)
            => source.TakeUntil(next.WhenFinished());

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
        
        public static IObservable<T> If<T>(this IObservable<T> source, Func<bool> predicate,
            Func<IObservable<T>, IObservable<T>> ifTrue, Func<IObservable<T>, IObservable<T>> ifFalse=null) 
            => Observable.Defer(() => predicate() ? ifTrue(source) : ifFalse?.Invoke(source)??source);
        
    }
}