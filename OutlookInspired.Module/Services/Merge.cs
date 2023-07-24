// // Licensed to the .NET Foundation under one or more agreements.
// // The .NET Foundation licenses this file to you under the Apache 2.0 License.
// // See the LICENSE file in the project root for more information. 
//
// using DevExpress.CodeParser;
//
// namespace System.Linq
// {
//     public static partial class AsyncEnumerableEx
//     {
//         public static IAsyncEnumerable<TSource> Merge<TSource>(params IAsyncEnumerable<TSource>[] sources)
//         {
//             // if (sources == null)
//                 // throw Error.ArgumentNull(nameof(sources));
//
// #if USE_FAIR_AND_CHEAPER_MERGE
// #else
//             return AsyncEnumerable.Create(Core);
//
//             async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
//             {
//                 var count = sources.Length;
//
//                 var enumerators = new IAsyncEnumerator<TSource>?[count];
//                 var moveNextTasks = new Task<bool>[count];
//
//                 try
//                 {
//                     for (var i = 0; i < count; i++)
//                     {
//                         var enumerator = sources[i].GetAsyncEnumerator(cancellationToken);
//                         enumerators[i] = enumerator;
//
//                         // REVIEW: This follows the lead of the original implementation where we kick off MoveNextAsync
//                         //         operations immediately. An alternative would be to do this in a separate stage, thus
//                         //         preventing concurrency across MoveNextAsync and GetAsyncEnumerator calls and avoiding
//                         //         any MoveNextAsync calls before all enumerators are acquired (or an exception has
//                         //         occurred doing so).
//
//                         moveNextTasks[i] = enumerator.MoveNextAsync().AsTask();
//                     }
//
//                     var active = count;
//
//                     while (active > 0)
//                     {
//                         // REVIEW: Performance of WhenAny may be an issue when called repeatedly like this. We should
//                         //         measure and could consider operating directly on the ValueTask<bool> objects, thus
//                         //         also preventing the Task<bool> allocations from AsTask.
//
//                         var moveNextTask = await Task.WhenAny(moveNextTasks).ConfigureAwait(false);
//
//                         // REVIEW: This seems wrong. AsTask can return the original Task<bool> (if the ValueTask<bool>
//                         //         is wrapping one) or return a singleton instance for true and false, at which point
//                         //         the use of IndexOf may pick an element closer to the start of the array because of
//                         //         reference equality checks and aliasing effects. See GetTaskForResult in the BCL.
//
//                         var index = Array.IndexOf(moveNextTasks, moveNextTask);
//
//                         var enumerator = enumerators[index]!; // NB: Only gets set to null after setting task to Never.
//
//                         if (!await moveNextTask.ConfigureAwait(false))
//                         {
//                             moveNextTasks[index] = TaskExt.Never;
//
//                             // REVIEW: The original implementation did not dispose eagerly, which could lead to resource
//                             //         leaks when merged with other long-running sequences.
//
//                             enumerators[index] = null; // NB: Avoids attempt at double dispose in finally if disposing fails.
//                             await enumerator.DisposeAsync().ConfigureAwait(false);
//
//                             active--;
//                         }
//                         else
//                         {
//                             var item = enumerator.Current;
//
//                             moveNextTasks[index] = enumerator.MoveNextAsync().AsTask();
//
//                             yield return item;
//                         }
//                     }
//                 }
//                 finally
//                 {
//                     // REVIEW: The original implementation performs a concurrent dispose, which seems undesirable given the
//                     //         additional uncontrollable source of concurrency and the sequential resource acquisition. In
//                     //         this modern implementation, we release resources in opposite order as we acquired them, thus
//                     //         guaranteeing determinism (and mimicking a series of nested `await using` statements).
//
//                     // REVIEW: If we decide to phase GetAsyncEnumerator and the initial MoveNextAsync calls at the start of
//                     //         the operator implementation, we should make this symmetric and first await all in flight
//                     //         MoveNextAsync operations, prior to disposing the enumerators.
//
//                     var errors = default(List<Exception>);
//
//                     for (var i = count - 1; i >= 0; i--)
//                     {
//                         var moveNextTask = moveNextTasks[i];
//                         var enumerator = enumerators[i];
//
//                         try
//                         {
//                             try
//                             {
//                                 if (moveNextTask != null && moveNextTask != TaskExt.Never)
//                                 {
//                                     _ = await moveNextTask.ConfigureAwait(false);
//                                 }
//                             }
//                             finally
//                             {
//                                 if (enumerator != null)
//                                 {
//                                     await enumerator.DisposeAsync().ConfigureAwait(false);
//                                 }
//                             }
//                         }
//                         catch (Exception ex)
//                         {
//                             if (errors == null)
//                             {
//                                 errors = new List<Exception>();
//                             }
//
//                             errors.Add(ex);
//                         }
//                     }
//
//                     // NB: If we had any errors during cleaning (and awaiting pending operations), we throw these exceptions
//                     //     instead of the original exception that may have led to running the finally block. This is similar
//                     //     to throwing from any finally block (except that we catch all exceptions to ensure cleanup of all
//                     //     concurrent sequences being merged).
//
//                     if (errors != null)
//                     {
//                         throw new AggregateException(errors);
//                     }
//                 }
//             }
// #endif
//         }
//
//         public static IAsyncEnumerable<TSource> Merge<TSource>(this IEnumerable<IAsyncEnumerable<TSource>> sources)
//         {
//             // if (sources == null)
//                 // throw Error.ArgumentNull(nameof(sources));
//
//             //
//             // REVIEW: This implementation does not exploit concurrency. We should not introduce such behavior in order to
//             //         avoid breaking changes, but we could introduce a parallel ConcurrentMerge implementation. It is
//             //         unfortunate though that the Merge overload accepting an array has always been concurrent, so we can't
//             //         change that either (in order to have consistency where Merge is non-concurrent, and ConcurrentMerge
//             //         is). We could consider a breaking change to Ix Async to streamline this, but we should do so when
//             //         shipping with the BCL interfaces (which is already a breaking change to existing Ix Async users). If
//             //         we go that route, we can either have:
//             //
//             //         - All overloads of Merge are concurrent
//             //           - and continue to be named Merge, or,
//             //           - are renamed to ConcurrentMerge for clarity (likely alongside a ConcurrentZip).
//             //         - All overloads of Merge are non-concurrent
//             //           - and are simply SelectMany operator macros (maybe more optimized)
//             //         - Have ConcurrentMerge next to Merge overloads
//             //           - where ConcurrentMerge may need a degree of concurrency parameter (and maybe other options), and,
//             //           - where the overload set of both families may be asymmetric
//             //
//
//             return sources.ToAsyncEnumerable().SelectMany(source => source);
//         }
//
//         public static IAsyncEnumerable<TSource> Merge<TSource>(this IAsyncEnumerable<IAsyncEnumerable<TSource>> sources)
//         {
//             if (sources == null)
//                 throw Error.ArgumentNull(nameof(sources));
//
//             //
//             // REVIEW: This implementation does not exploit concurrency. We should not introduce such behavior in order to
//             //         avoid breaking changes, but we could introduce a parallel ConcurrentMerge implementation.
//             //
//
//             return sources.SelectMany(source => source);
//         }
//     }
// }