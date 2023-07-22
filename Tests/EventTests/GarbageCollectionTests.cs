using NUnit.Framework;
using OutlookInspired.Module;
using OutlookInspired.Module.Services;
using Shouldly;

namespace Tests.EventTests{
    public class GarbageCollectionTests{
        [Test]
        public async Task FromEventPattern_ShouldStopYieldingWhenSourceIsGarbageCollected(){
            var weakReference = new WeakReference(new TestClass());
            var asyncEnumerable = ((TestClass)weakReference.Target!).WhenEventFired<EventArgs>("TestEvent");
            
            GC.Collect();
            GC.WaitForPendingFinalizers();

            var cts = new CancellationTokenSource(1000); 
            var e = await asyncEnumerable.FirstOrDefaultAsync(cts.Token); 
            e.ShouldBeNull(); 
        }

        [Test]
        public async Task FromEventPattern_ShouldStopYieldingWhenListenerIsGarbageCollected(){
            var testClass = new TestClass();
            var disposableEnumerable = testClass.WhenEventFired<EventArgs>("TestEvent") as DisposableAsyncEnumerable<EventArgs>;

            var tcs = new TaskCompletionSource<bool>();
            var unused = new WeakReference(Task.Run(async () => {
                tcs.SetResult(true);
                await foreach (var _ in disposableEnumerable!) Assert.Fail("The event was raised.");
            }));

            await tcs.Task; 
            await disposableEnumerable!.DisposeAsync();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            testClass.RaiseTestEvent(); 

            var cts = new CancellationTokenSource(1000); 
            var e = await disposableEnumerable.FirstOrDefaultAsync(cts.Token);
            e.ShouldBeNull();
        }

    }
}