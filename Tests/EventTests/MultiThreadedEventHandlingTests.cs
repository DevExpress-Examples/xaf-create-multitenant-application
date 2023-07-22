using NUnit.Framework;
using Shouldly;

namespace Tests.EventTests{
    public class MultiThreadedEventHandlingTests{
        [Test]
        public async Task FromEventPattern_ShouldHandleMultipleThreads(){
            var testClass = new TestClass();
            var asyncEnumerable = testClass.WhenEventFired<EventArgs>("TestEvent");
            var cts = new CancellationTokenSource();
            var eventCount = 0;

            var unused = Task.Run(async () => {
                await foreach (var _ in asyncEnumerable.WithCancellation(cts.Token)){
                    eventCount++;
                }
            }, cts.Token);

            var tasks = Enumerable.Range(0, 10).Select(_ => Task.Run(() => testClass.RaiseTestEvent(), cts.Token)).ToArray();
            await Task.WhenAll(tasks);
            
            await Task.Delay(100, cts.Token);
            
            cts.Cancel();
            
            eventCount.ShouldBe(10);
        }
   
    }
}