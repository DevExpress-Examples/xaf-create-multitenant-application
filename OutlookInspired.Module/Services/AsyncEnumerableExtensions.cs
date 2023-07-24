using System.Collections.Concurrent;

namespace OutlookInspired.Module.Services{
    public static class AsyncEnumerableExtensions{
        // public static async Task<int> CountAsync<T>(this IAsyncEnumerable<T> source){
        //     if (source == null)
        //         throw new ArgumentNullException(nameof(source));
        //     var count = 0;
        //     await foreach (var unused in source) count++;
        //     return count;
        // }

        // public static async IAsyncEnumerable<T> Finally<T>(this IAsyncEnumerable<T> source, Action finallyAction){
        //     try{
        //         await foreach (var item in source) yield return item;
        //     }
        //     finally{
        //         finallyAction();
        //     }
        // }

        // public static async IAsyncEnumerable<T> DoNotComplete<T>(this IAsyncEnumerable<T> source){
        //     await using var enumerator = source.GetAsyncEnumerator();
        //     while (await enumerator.MoveNextAsync()) yield return enumerator.Current;
        //
        //     while (true){
        //         await Task.Delay(Timeout.Infinite);
        //         yield break;
        //     }
        // }

        // public static IAsyncEnumerable<T> Do<T>(this IAsyncEnumerable<T> source, Action<T> @do) 
        //     => source.Select(arg => {
        //         @do(arg);
        //         return arg;
        //     });

        // public static IAsyncEnumerable<T> WhenNotDefault<T>(this IAsyncEnumerable<T> source) => source.Where(arg => !arg.IsDefaultValue());

        // public static IAsyncEnumerable<TSource> WhenNotDefault<TSource,TValue>(this IAsyncEnumerable<TSource> source,Func<TSource,TValue> valueSelector) 
            // =>source.Where(source1 => !valueSelector(source1).IsDefaultValue());
        
        // public static async IAsyncEnumerable<T> Where<T>(this IAsyncEnumerable<T> source, Func<T, bool> predicate){
        //     await foreach (var item in source)
        //         if (predicate(item))
        //             yield return item;
        // }
        // public static async IAsyncEnumerable<T> StartWith<T>(this IAsyncEnumerable<T> source, T item){
        //     yield return item;
        //     await foreach (var element in source) yield return element;
        // }
        
        public static IAsyncEnumerable<Unit> ToUnit<T>(this IAsyncEnumerable<T> source) => source.Select(_ => Unit.Default);

        public static async IAsyncEnumerable<T> TakeUntil<T>(this IAsyncEnumerable<T> source,
            IAsyncEnumerable<Unit> trigger){
            await using var triggerEnumerator = trigger.GetAsyncEnumerator();
            await foreach (var item in source){
                if (await triggerEnumerator.MoveNextAsync()){
                    await triggerEnumerator.DisposeAsync();
                    break;
                }

                yield return item;
            }
        }
        
        public static IAsyncEnumerable<TResult> Cast<TResult>(this IAsyncEnumerable<object> source) => source.Select(item => (TResult)item);

        // public static async IAsyncEnumerable<TResult> Select<TSource, TResult>(
        //     this IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector){
        //     await foreach (var item in source) yield return selector(item);
        // }

        public static IAsyncEnumerable<object> Concat(this IAsyncEnumerable<object> first,
            IAsyncEnumerable<object> second) =>
            first.EnumerateAll(second);

        public static async IAsyncEnumerable<TSource> EnumerateAll<TSource>(this IAsyncEnumerable<TSource> first,
            IAsyncEnumerable<TSource> second){
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

            IAsyncEnumerator<T> IAsyncEnumerable<T>.GetAsyncEnumerator(CancellationToken cancellationToken ){
                return _getEnumerator(cancellationToken);
            }
        }

        public static async Task<T[]> ToArrayAsync<T>(this IAsyncEnumerable<T> source){
            var bag = new ConcurrentBag<T>();
            await foreach (var item in source) bag.Add(item);
            return bag.ToArray();
        }

        // public static async IAsyncEnumerable<TResult> SelectMany<TSource, TResult>(
        //     this IAsyncEnumerable<TSource> source, Func<TSource, IAsyncEnumerable<TResult>> selector){
        //     await foreach (var item in source){
        //         var results = selector(item);
        //         await foreach (var result in results) yield return result;
        //     }
        // }

        // public static IAsyncEnumerable<T> ObserveOn<T>(this IAsyncEnumerable<T> source, SynchronizationContext context) 
        //     => source.Select(item => {
        //         var tcs = new TaskCompletionSource<T>();
        //         context.Post(_ => { tcs.SetResult(item); }, null);
        //         return tcs.Task;
        //     }).SelectMany(task => task.ToAsyncEnumerable());
        
        // public static IAsyncEnumerable<T> ObserveOnContext<T>(this IAsyncEnumerable<T> source) 
        //     => source.ObserveOn(SynchronizationContext.Current);

        // public static async IAsyncEnumerable<T> Delay<T>(this IAsyncEnumerable<T> source, TimeSpan delay){
        //     await Task.Delay(delay);
        //     await foreach (var item in source) yield return item;
        // }


        // public static async IAsyncEnumerable<TResult> SelectAwait<TSource, TResult>(
        //     this IAsyncEnumerable<TSource> source, Func<TSource, Task<TResult>> selector){
        //     await foreach (var item in source) yield return await selector(item);
        // }

        // public static async IAsyncEnumerable<T> Skip<T>(this IAsyncEnumerable<T> source, int count){
        //     await foreach (var item in source)
        //         if (count > 0)
        //             count--;
        //         else
        //             yield return item;
        // }

        public static async Task<T> FirstAsync<T>(this IAsyncEnumerable<T> source){
            await foreach (var item in source) return item;
            throw new InvalidOperationException("The source sequence is empty.");
        }

        // public static async IAsyncEnumerable<T> SelectMany<T>(
        //     this IAsyncEnumerable<IEnumerable<T>> source){
        //     await foreach (var innerSequence in source)
        //     foreach (var item in innerSequence)
        //         yield return item;
        // }

        // public static async IAsyncEnumerable<T> SelectMany<T>(this IAsyncEnumerable<IAsyncEnumerable<T>> source){
        //     await foreach (var innerSequence in source)
        //     await foreach (var item in innerSequence)
        //         yield return item;
        // }

        // public static async IAsyncEnumerable<List<TSource>> Buffer<TSource, TBuffer>(
        //     this IAsyncEnumerable<TSource> source, IAsyncEnumerable<TBuffer> bufferBoundary){
        //     var buffer = new List<TSource>();
        //     var boundaryEnumerator = bufferBoundary.GetAsyncEnumerator();
        //
        //     await foreach (var item in source){
        //         buffer.Add(item);
        //         if (await boundaryEnumerator.MoveNextAsync()){
        //             yield return buffer;
        //             buffer = new List<TSource>();
        //         }
        //     }
        //
        //     if (buffer.Count > 0) yield return buffer;
        // }

        // public static async IAsyncEnumerable<List<T>> Buffer<T>(this IAsyncEnumerable<T> source, int count){
        //     var buffer = new List<T>();
        //     await foreach (var item in source){
        //         buffer.Add(item);
        //         if (buffer.Count >= count){
        //             yield return buffer;
        //             buffer = new List<T>();
        //         }
        //     }
        //
        //     if (buffer.Count > 0) yield return buffer;
        // }

        // public static async Task<List<T>> BufferAsync<T>(this IAsyncEnumerable<T> source){
        //     var buffer = new List<T>();
        //     await foreach (var item in source) buffer.Add(item);
        //     return buffer;
        // }

        // public static async IAsyncEnumerable<T> Take<T>(this IAsyncEnumerable<T> source, int count){
        //     await using var enumerator = source.GetAsyncEnumerator();
        //     while(count-- > 0 && await enumerator.MoveNextAsync())
        //         yield return enumerator.Current;
        // }
        
        // public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> source){
        //     var list = new List<T>();
        //     await foreach (var item in source) list.Add(item);
        //     return list;
        // }

        public static IAsyncEnumerable<T> To<T>(this IAsyncEnumerable<object> source, T value) => source.Select(_ => value);

        // public static Task ForEachAsync<T>(this IAsyncEnumerable<T> source, Action<T> action) => source.ForEachAsync(action, CancellationToken.None);

        // public static async Task ForEachAsync<T>(this IAsyncEnumerable<T> source, Action<T> action,
        //     CancellationToken cancellationToken){
        //     await foreach (var item in source.WithCancellation(cancellationToken)){
        //         if (cancellationToken.IsCancellationRequested)
        //             break;
        //
        //         action(item);
        //     }
        // }

        // public static async Task EnumerateAsync<T>(this IAsyncEnumerator<T> enumerator){
        //     try{
        //         while (await enumerator.MoveNextAsync()){
        //         }
        //     }
        //     finally{
        //         await enumerator.DisposeAsync();
        //     }
        // }
        // public static async IAsyncEnumerable<T> Merge<T>(this IAsyncEnumerable<IAsyncEnumerable<T>> source)
        // {
        //     var queue = new ConcurrentQueue<T>();
        //     var cts = new CancellationTokenSource();
        //
        //     async Task processStream(IAsyncEnumerable<T> stream)
        //     {
        //         await foreach (var item in stream)
        //         {
        //             queue.Enqueue(item);
        //         }
        //     }
        //
        //     var tasks = new List<Task>();
        //     await foreach (var stream in source)
        //     {
        //         tasks.Add(processStream(stream));
        //     }
        //
        //     var allTasks = Task.WhenAll(tasks);
        //     while (!allTasks.IsCompleted)
        //     {
        //         while (queue.TryDequeue(out var item))
        //         {
        //             yield return item;
        //         }
        //         await Task.Delay(100); // or a shorter delay if you want to check more often
        //     }
        //
        //     // finish processing remaining items in the queue
        //     while (queue.TryDequeue(out var item))
        //     {
        //         yield return item;
        //     }
        //
        //     // if any of the tasks had an error, propagate that error
        //     await allTasks;
        // }

        // public static async IAsyncEnumerable<T> Merge<T>(
        //     this IAsyncEnumerable<T> first, IAsyncEnumerable<T> second){
        //     var firstEnumerator = first.GetAsyncEnumerator();
        //     var secondEnumerator = second.GetAsyncEnumerator();
        //
        //     while (true){
        //         var moveNextTasks = new List<Task<bool>>{
        //             firstEnumerator.MoveNextAsync().AsTask(),
        //             secondEnumerator.MoveNextAsync().AsTask()
        //         };
        //
        //         while (moveNextTasks.Count > 0){
        //             var completedTask = await Task.WhenAny(moveNextTasks);
        //             moveNextTasks.Remove(completedTask);
        //
        //             if (completedTask.Result){
        //                 if (completedTask == moveNextTasks[0])
        //                     yield return firstEnumerator.Current;
        //                 else
        //                     yield return secondEnumerator.Current;
        //             }
        //         }
        //
        //         if (moveNextTasks.Count == 0) break;
        //     }
        //
        //     await firstEnumerator.DisposeAsync();
        //     await secondEnumerator.DisposeAsync();
        // }
        
        // public static IAsyncEnumerable<T> ToAsyncEnumerable<T>(this T obj) => obj.ToEnumerable().ToAsyncEnumerable();

        // public static async Task<T[]> TakeAsync<T>(this IAsyncEnumerable<T> source, int count){
        //     var result = new List<T>();
        //     await foreach (var item in source){
        //         result.Add(item);
        //         if (--count == 0) break;
        //     }
        //
        //     return result.ToArray();
        // }
        // public static async IAsyncEnumerable<T> TakeAsync<T>(this IAsyncEnumerable<T> source, int count){
        //     var enumerator = source.GetAsyncEnumerator();
        //     try{
        //         for (var i = 0; i < count; i++)
        //             if (await enumerator.MoveNextAsync())
        //                 yield return enumerator.Current;
        //             else
        //                 throw new InvalidOperationException("Not enough elements in the source.");
        //     }
        //     finally{
        //         await enumerator.DisposeAsync();
        //     }
        // }

        // public static async Task<T> LastAsync<T>(this IAsyncEnumerable<T> source){
        //     var enumerator = source.GetAsyncEnumerator();
        //     if (!await enumerator.MoveNextAsync()) throw new InvalidOperationException("The source sequence is empty.");
        //
        //     T lastItem;
        //     do{
        //         lastItem = enumerator.Current;
        //     } while (await enumerator.MoveNextAsync());
        //
        //     return lastItem;
        // }

        // public static async Task<T> FirstOrDefaultAsync<T>(this IAsyncEnumerable<T> source){
        //     await foreach (var item in source) return item;
        //     return default;
        // }

        // public static async Task<TSource> FirstOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source,
        //     CancellationToken cancellationToken){
        //     try{
        //         var timeoutTask = Task.Delay(Timeout.Infinite, cancellationToken);
        //         var firstOrDefaultTask = source.FirstOrDefaultAsyncInternal(cancellationToken);
        //         var completedTask = await Task.WhenAny(firstOrDefaultTask, timeoutTask).ConfigureAwait(false);
        //
        //         if (completedTask == timeoutTask) throw new OperationCanceledException(cancellationToken);
        //
        //         return await firstOrDefaultTask.ConfigureAwait(false);
        //     }
        //     catch (OperationCanceledException){
        //         return default;
        //     }
        // }

        // private static async Task<TSource> FirstOrDefaultAsyncInternal<TSource>(this IAsyncEnumerable<TSource> source,
        //     CancellationToken cancellationToken){
        //     await using var enumerator = source.GetAsyncEnumerator(cancellationToken);
        //     return await enumerator.MoveNextAsync() ? enumerator.Current : default;
        // }

    }
}