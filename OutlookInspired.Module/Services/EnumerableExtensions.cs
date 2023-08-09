using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Module.Services{
    public static class EnumerableExtensions{
        

        public static IEnumerable<T> Finally<T>(this IEnumerable<T> source, Action action){
            return _();
            IEnumerable<T> _(){
                foreach (var element in source) yield return element;
                action();
            }
        }
        
        public static IEnumerable<TSource> SelectMany<TSource>(this IEnumerable<IEnumerable<TSource>> source) 
            => source.SelectMany(sources => sources);

        public static IEnumerable<T> SelectManyRecursive<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> childrenSelector){
            foreach (var i in source){
                yield return i;
                var children = childrenSelector(i);
                if (children == null) continue;
                foreach (var child in SelectManyRecursive(children, childrenSelector))
                    yield return child;
            }
        }
        public static void Enumerate<T>(this IEnumerable<T> source) {
            using var e = source.GetEnumerator();
            while (e.MoveNext()) { }
        }

        public static IEnumerable<TSource> Do<TSource>(
            this IEnumerable<TSource> source, Action<TSource> action)
            => source.Select(item => {
                action(item);
                return item;
            });

        public static IEnumerable<T> IgnoreElements<T>(this IEnumerable<T> source){
            foreach (var unused in source){
                yield break;
            }
        }
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> source,params T[] values) => source.Concat(values.AsEnumerable());

        public static Dictionary<TKey, TObject> ToDictionary<TObject,TKey>(this IEnumerable<TObject> objects,Func<TObject,TKey> keySelector) where TObject:MigrationBaseObject 
            => objects.ToDictionary(keySelector, o => o);

        public static IEnumerable<TValue> To<TSource,TValue>(this IEnumerable<TSource> source,TValue value) 
            => source.Select(_ => value);

        public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this IEnumerable<T> enumerable){
            foreach (var item in enumerable) yield return item;
            await Task.CompletedTask;
        }
        
        public static IEnumerable<T> WhereNotDefault<T,T2>(this IEnumerable<T> source, Func<T,T2> predicate) 
            => source.Where(arg => !predicate(arg).IsDefaultValue());

        public static IEnumerable<TSource> WhereNotDefault<TSource>(this IEnumerable<TSource> source) {
            var type = typeof(TSource);
            if (type.IsClass || type.IsInterface){
                return source.Where(source1 => source1!=null);   
            }
            var instance = type.CreateInstance();
            return source.Where(source1 => !source1.Equals(instance));
        }
        
        public static IEnumerable<string> WhereNotNullOrEmpty(this IEnumerable<string> source) 
            => source.Where(s => s.IsNotNullOrEmpty());
        
        public static IEnumerable<string> WhereNotNull(this IEnumerable<string> source) 
            => source.Where(s => s!=null);
        
        public static IEnumerable<string> WhereNotEmpty(this IEnumerable<string> source) 
            => source.Where(s => s!=String.Empty);
        
        public static bool IsNullOrEmpty(this string strString)
            => string.IsNullOrEmpty(strString);
        public static bool IsNotNullOrEmpty(this string strString)
            => !string.IsNullOrEmpty(strString);
    }
}