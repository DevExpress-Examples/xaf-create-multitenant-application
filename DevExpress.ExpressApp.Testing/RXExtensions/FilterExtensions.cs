using System.Collections;
using System.Reactive.Linq;

namespace DevExpress.ExpressApp.Testing.RXExtensions{
    public static class FilterExtensions{
        public static IObservable<TSource> WhenDefault<TSource>(this IObservable<TSource> source) 
            => source.Where(obj => obj.IsDefaultValue());

        public static IObservable<TSource> WhenDefault<TSource,TValue>(this IObservable<TSource> source,Func<TSource, TValue> valueSelector) 
            =>source.Where(source1 => valueSelector(source1).IsDefaultValue());
	    
        public static IObservable<TSource> WhenDefault<TSource>(this IObservable<TSource> source,Func<TSource, object> valueSelector,Func<TSource,Type> valueType) 
            =>source.Where(source1 => valueSelector(source1).IsDefaultValue());
        public static IObservable<TOut> WhenNotEmpty<TOut>(this IObservable<TOut> source) where TOut:IEnumerable
            => source.Where(outs => outs.Cast<object>().Any());
        
        public static IObservable<TSource> WhenNotDefault<TSource,TValue>(this IObservable<TSource> source,Func<TSource,TValue> valueSelector) 
            =>source.Where(source1 => !ReflectionExtensions.IsDefaultValue(valueSelector(source1)));

        public static IObservable<TSource> WhenNotDefault<TSource>(this IObservable<TSource> source) => source.Where(s => !s.IsDefaultValue());
        
        public static IObservable<TSource> WhenNotDefaultOrEmpty<TSource>(this IObservable<TSource> source) where TSource:IEnumerable
            => source.WhenNotDefault().WhenNotEmpty();
        
        public static IObservable<string> WhenNotDefaultOrEmpty(this IObservable<string> source) => source.Where(s => !s.IsNullOrEmpty());
        
        public static IObservable<string> WhenDefaultOrEmpty(this IObservable<string> source) => source.Where(s => s.IsNullOrEmpty());
    }
}