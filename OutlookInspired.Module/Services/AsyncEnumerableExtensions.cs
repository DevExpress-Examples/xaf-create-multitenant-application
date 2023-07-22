using System.Collections.Concurrent;

namespace OutlookInspired.Module.Services{
    public static class AsyncEnumerableExtensions{
        
        public static IAsyncEnumerable<TResult> Cast<TResult>(this IAsyncEnumerable<object> source) => source.Select(item => (TResult)item);

        public static async IAsyncEnumerable<TResult> Select<TSource, TResult>(
            this IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector){
            await foreach (var item in source){
                yield return selector(item);
            }
        }
        public static IAsyncEnumerable<object> Concat(this IAsyncEnumerable<object> first, IAsyncEnumerable<object> second) 
            => first.EnumerateAll( second);

        public static async IAsyncEnumerable<TSource> EnumerateAll<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second){
            await foreach (var element in first.Enumerate()) yield return element;
            await foreach (var element in second.Enumerate()) yield return element;
        }

        private static async IAsyncEnumerable<TSource> Enumerate<TSource>(this IAsyncEnumerable<TSource> source){
            await foreach (var element in source) yield return element;
        }

        public static IAsyncEnumerable<T> Create<T>(Func<CancellationToken, IAsyncEnumerator<T>> getEnumerator) => new DelegateAsyncEnumerable<T>(getEnumerator);

        private class DelegateAsyncEnumerable<T> : IAsyncEnumerable<T>{
            private readonly Func<CancellationToken, IAsyncEnumerator<T>> _getEnumerator;
            public DelegateAsyncEnumerable(Func<CancellationToken, IAsyncEnumerator<T>> getEnumerator) => _getEnumerator = getEnumerator;
            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) => _getEnumerator(cancellationToken);
        }

        public static async Task ToArrayAsync<T>(this IAsyncEnumerable<T> source){
            var bag = new ConcurrentBag<T>();
            await foreach (var item in source){
                bag.Add(item);
            }

            bag.ToArray();
        }

        public static async IAsyncEnumerable<TResult> SelectMany<TSource, TResult>(
            this IAsyncEnumerable<TSource> source, Func<TSource, IAsyncEnumerable<TResult>> selector){
            await foreach (var item in source){
                var results = selector(item);
                await foreach (var result in results) yield return result;
            }
        }
        
        public static async IAsyncEnumerable<TResult> SelectAwait<TSource, TResult>(
            this IAsyncEnumerable<TSource> source, Func<TSource, Task<TResult>> selector){
            await foreach (var item in source){
                yield return await selector(item);
            }
        }

        public static async IAsyncEnumerable<T> Skip<T>(this IAsyncEnumerable<T> source, int count){
            await foreach (var item in source)
                if (count > 0)
                    count--;
                else
                    yield return item;
        }
        
        public static async Task<T> FirstAsync<T>(this IAsyncEnumerable<T> source){
            await foreach (var item in source) return item;
            throw new InvalidOperationException("The source sequence is empty.");
        }

        public static async Task<List<T>> Buffer<T>(this IAsyncEnumerable<T> source){
            var buffer = new List<T>();
            await foreach (var item in source) buffer.Add(item);
            return buffer;
        }

        public static async IAsyncEnumerable<T> Take<T>(this IAsyncEnumerable<T> source, int count){
            await foreach (var item in source)
                if (count-- > 0)
                    yield return item;
                else
                    break;
        }

        public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> source){
            var list = new List<T>();
            await foreach (var item in source) list.Add(item);
            return list;
        }

        public static IAsyncEnumerable<T> To<T>(this IAsyncEnumerable<object> source, T value)
            => source.Select(_ => value);
        
        public static Task ForEachAsync<T>(this IAsyncEnumerable<T> source, Action<T> action) 
            => source.ForEachAsync( action, CancellationToken.None);

        public static async Task ForEachAsync<T>(this IAsyncEnumerable<T> source, Action<T> action,
            CancellationToken cancellationToken){
            await foreach (var item in source.WithCancellation(cancellationToken)){
                if (cancellationToken.IsCancellationRequested)
                    break;

                action(item);
            }
        }
            
        public static async Task<int> CountAsync<T>(this IAsyncEnumerable<T> source){
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            var count = 0;
            await foreach (var unused in source) count++;
            return count;
        }
        public static async IAsyncEnumerable<T> Finally<T>(this IAsyncEnumerable<T> source, Action finallyAction){
            try{
                await foreach (var item in source) yield return item;
            }
            finally{
                finallyAction();
            }
        }

        public static async Task<T> FirstOrDefaultAsync<T>(this IAsyncEnumerable<T> source){
            await foreach (var item in source) return item;
            return default;
        }

        public static async Task<TSource> FirstOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken ){
            try{
                var timeoutTask = Task.Delay(Timeout.Infinite, cancellationToken);
                var firstOrDefaultTask = source.FirstOrDefaultAsyncInternal(cancellationToken);
                var completedTask = await Task.WhenAny(firstOrDefaultTask, timeoutTask).ConfigureAwait(false);

                if (completedTask == timeoutTask) throw new OperationCanceledException(cancellationToken);

                return await firstOrDefaultTask.ConfigureAwait(false);
            }
            catch (OperationCanceledException){
                return default; 
            }
        }

        private static async Task<TSource> FirstOrDefaultAsyncInternal<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken){
            await using var enumerator = source.GetAsyncEnumerator(cancellationToken);
            return await enumerator.MoveNextAsync() ? enumerator.Current : default;
        }
    }
}