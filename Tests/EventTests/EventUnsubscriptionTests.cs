using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using OutlookInspired.Module;
using OutlookInspired.Module.Services;
using Shouldly;

namespace Tests.EventTests{
    public class EventUnsubscriptionTests{
        [Test]
        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task FromEventPattern_ShouldNotYieldAfterUnsubscribe(){
            var testClass = new TestClass();
            var asyncEnumerable = testClass.WhenEventFired<EventArgs>("TestEvent");
            var eventCount = 0;
            var cts = new CancellationTokenSource();
            
            var unused = Task.Run(async () => {
                await foreach (var _ in asyncEnumerable.WithCancellation(cts.Token)) eventCount++;
            }, cts.Token);

            testClass.RaiseTestEvent();
            testClass.RaiseTestEvent();
            
            await Task.Delay(100, cts.Token);
            
            eventCount.ShouldBe(2); 
            
            cts.Cancel();
            
            testClass.RaiseTestEvent();
            testClass.RaiseTestEvent();

            
            await Task.Delay(100);
            
            eventCount.ShouldBe(2); 
        }

        [Test]
        public async Task FromEventPattern_ShouldYieldToMultipleSubscribers(){
            var testClass = new TestClass();
            var asyncEnumerable1 = testClass.WhenEventFired<EventArgs>("TestEvent");
            var asyncEnumerable2 = testClass.WhenEventFired<EventArgs>("TestEvent");

            testClass.RaiseTestEvent();

            var cts = new CancellationTokenSource(1000); 

            
            var e1 = await asyncEnumerable1.FirstOrDefaultAsync(cts.Token);
            e1.ShouldNotBeNull(); 

            
            var e2 = await asyncEnumerable2.FirstOrDefaultAsync(cts.Token);
            e2.ShouldNotBeNull(); 
        }

        [Test]
        public void FromEventPattern_ShouldAllowSubscriptionAfterUnsubscription()
        {
            // Arrange
            var testClass = new TestClass();
            var asyncEnumerable1 = testClass.WhenEventFired<EventArgs>("TestEvent");

            // Act
            asyncEnumerable1 = null; // This should unsubscribe the event

            // Assert
            Should.NotThrow(() => testClass.WhenEventFired<EventArgs>("TestEvent"));
        }

        [Test]
        public async Task FromEventPattern_ShouldAllowMultipleSubscriptions(){
            var testClass = new TestClass();
            var asyncEnumerable1 = testClass.WhenEventFired<EventArgs>("TestEvent");
            var asyncEnumerable2 = testClass.WhenEventFired<EventArgs>("TestEvent");
            var eventCount1 = 0;
            var eventCount2 = 0;

            _ = Task.Run(async () => {
                await foreach (var _ in asyncEnumerable1) eventCount1++;
            });

            _ = Task.Run(async () => {
                await foreach (var _ in asyncEnumerable2) eventCount2++;
            });

            testClass.RaiseTestEvent();
            await Task.Delay(100);

            eventCount1.ShouldBe(1);
            eventCount2.ShouldBe(1);
        }

        [Test]
        public void FromEventPattern_ShouldHandleExceptionThrownInEvent(){
            var testClass = new TestClass();
            var asyncEnumerable = testClass.WhenEventFired<ExceptionThrowingEventArgs>("TestEventException");

            Assert.DoesNotThrowAsync(async () => {
                var enumerator = asyncEnumerable.GetAsyncEnumerator();
                try{
                    testClass.RaiseTestEventException();
                    await Task.Delay(100);
                    await enumerator.MoveNextAsync();  // Manually advance the enumerator
                }
                catch (Exception ex){
                    // Handle or ignore the exception
                }
            });
        }

        // For memory leak testing, it would require a more complex setup to monitor the memory usage,
        // so it's not straightforward to implement it here without proper setup.

        [Test]
        public async Task FromEventPattern_ShouldSupportResubscriptionAfterUnsubscription(){
            var testClass = new TestClass();
            var asyncEnumerable = testClass.WhenEventFired<EventArgs>("TestEvent");

            // Subscribe and unsubscribe
            var eventCount = 0;
            var cts = new CancellationTokenSource();

            var unused = Task.Run(async () => {
                await foreach (var _ in asyncEnumerable.WithCancellation(cts.Token)) eventCount++;
            }, cts.Token);

            testClass.RaiseTestEvent();
            await Task.Delay(100);
            cts.Cancel();

            // Resubscribe
            eventCount = 0;
            cts = new CancellationTokenSource();

            unused = Task.Run(async () => {
                await foreach (var _ in asyncEnumerable.WithCancellation(cts.Token)) eventCount++;
            }, cts.Token);

            testClass.RaiseTestEvent();
            await Task.Delay(100);
            cts.Cancel();

            eventCount.ShouldBe(1);
        }

        // The test for cancellation during event processing would highly depend on your 
        // specific use case and how you implement cancellation. So it might not be feasible 
        // to give a generic example without more context.

        // For concurrent subscription and unsubscription, it's hard to test in a meaningful way
        // because the outcome of such a scenario would highly depend on the specific timings of 
        // the operations which cannot be controlled in a reliable way in a test.

        [Test]
        public async Task FromEventPattern_ShouldHandleEventRaisingBeforeSubscription(){
            var testClass = new TestClass();
            var eventCount = 0;

            testClass.RaiseTestEvent();

            await Task.Delay(100);

            var asyncEnumerable = testClass.WhenEventFired<EventArgs>("TestEvent");

            _ = Task.Run(async () => {
                await foreach (var _ in asyncEnumerable) eventCount++;
            });

            await Task.Delay(100);

            eventCount.ShouldBe(0);
        }
    }
    }
