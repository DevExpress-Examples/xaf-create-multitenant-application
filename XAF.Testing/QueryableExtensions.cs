using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace XAF.Testing{
    public static class QueryableExtensions{
        private static readonly MethodInfo ASAsyncEnumerableMethod=ASAsyncEnumerableMethod = typeof(EntityFrameworkQueryableExtensions)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .First(m => m.Name == "AsAsyncEnumerable" && m.IsGenericMethod);
        public static IAsyncEnumerable<object> EnumerateAsync(this IQueryable source) => source.EnumerateAsync(CancellationToken.None);

        public static async IAsyncEnumerable<object> EnumerateAsync(this IQueryable source,[EnumeratorCancellation] CancellationToken token){
            var asyncEnumerable = ASAsyncEnumerableMethod.MakeGenericMethod(source.ElementType)
                .Invoke(null, new object[] { source });
            var asyncEnumerator = asyncEnumerable!.GetType().GetMethod("GetAsyncEnumerator")!
                .Invoke(asyncEnumerable, new object[] {token });
            while (await (ValueTask<bool>)asyncEnumerator!.GetType().GetMethod("MoveNextAsync")!.Invoke(asyncEnumerator, new object[] { })!){
                yield return asyncEnumerator.GetType().GetProperty("Current")!.GetValue(asyncEnumerator);
            }
        }

        static readonly MethodInfo SkipMethod = typeof(Queryable).GetStaticMethod("Skip");
        static readonly MethodInfo TakeMethod = typeof(Queryable).GetStaticMethod("Take");
        public static async IAsyncEnumerable<object> PaginateAsync(this IQueryable queryable, int pageSize = 100){
            var pageNumber = 0;
            while (true){
                var hasItems = false;
                await foreach (var item in queryable.PaginateAsync( pageSize, pageNumber)){
                    hasItems = true;
                    yield return item;
                }
                if (!hasItems)
                    break;
                pageNumber++;
            }
        }

        private static IAsyncEnumerable<object> PaginateAsync(this IQueryable queryable, int pageSize, int pageNumber) 
            => ((IEnumerable<object>)TakeMethod.MakeGenericMethod(queryable.ElementType)
                .Invoke(null, new[] { SkipMethod.MakeGenericMethod(queryable.ElementType)
                    .Invoke(null, new object[] { queryable, pageNumber * pageSize }), pageSize }))!
            .ToList().ToAsyncEnumerable();
    }
}