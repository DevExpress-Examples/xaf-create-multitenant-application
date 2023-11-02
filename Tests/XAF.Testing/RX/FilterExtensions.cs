using System.Collections;
using System.Reactive.Linq;

namespace XAF.Testing.RX{
    public static class FilterExtensions{
        public static IObservable<T> TakeOrOriginal<T>(this IObservable<T> source, int count) => count == 0 ? source : source.Take(count);

        public static IObservable<TSource> WhenDefault<TSource>(this IObservable<TSource> source) 
            => source.Where(obj => obj.IsDefaultValue());

        public static IObservable<TSource> WhenDefault<TSource,TValue>(this IObservable<TSource> source,Func<TSource, TValue> valueSelector) 
            =>source.Where(source1 => valueSelector(source1).IsDefaultValue());
	    
        
        public static IObservable<TOut> WhenNotEmpty<TOut>(this IObservable<TOut> source) where TOut:IEnumerable
            => source.Where(outs => outs.Cast<object>().Any());
        
        public static IObservable<TSource> WhenNotDefault<TSource,TValue>(this IObservable<TSource> source,Func<TSource,TValue> valueSelector) 
            =>source.Where(source1 => !ReflectionExtensions.IsDefaultValue(valueSelector(source1)));

        public static IObservable<TSource> WhenNotDefault<TSource>(this IObservable<TSource> source) => source.Where(s => !s.IsDefaultValue());
        
    }
}