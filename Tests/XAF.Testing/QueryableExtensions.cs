using System.Reflection;

namespace XAF.Testing{
    public static class QueryableExtensions{
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