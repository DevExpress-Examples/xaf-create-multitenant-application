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

    }
}