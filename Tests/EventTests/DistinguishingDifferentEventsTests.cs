using System.Collections.Concurrent;
using NUnit.Framework;
using OutlookInspired.Module;
using OutlookInspired.Module.Services;
using Shouldly;

namespace Tests.EventTests{
    public class DistinguishingDifferentEventsTests{
        [Test]
        public async Task FromEventPattern_ShouldDistinguishDifferentEvents(){
            var testClass = new TestClass();
            var asyncEnumerable1 = testClass.WhenEventFired<EventArgs>("TestEvent");
            var asyncEnumerable2 = testClass.WhenEventFired<StringEventArgs>("TestEventWithParameter");

            
            testClass.RaiseTestEvent();
            testClass.RaiseTestEventWithParameter("parameter");

            
            var e1 = await asyncEnumerable1.FirstAsync(); 
            e1.ShouldNotBeNull();

            var e2 = await asyncEnumerable2.FirstAsync(); 
            e2.Value.ShouldBe("parameter");
        }

        [Test]
        public void FromEventPattern_ShouldMaintainOrderOfDifferentEvents()
        {
            // Arrange
            var testClass = new TestClass();
            var asyncEnumerable1 = testClass.WhenEventFired<EventArgs>("TestEvent");
            var asyncEnumerable2 = testClass.WhenEventFired<StringEventArgs>("TestEventWithParameter");
            var events = new List<string>();

            var eventHandled = new AutoResetEvent(false);

            // Start listening in the background
            _ = Task.Run(async () =>
            {
                await foreach (var _ in asyncEnumerable1)
                {
                    events.Add("TestEvent");
                    eventHandled.Set(); // Signal that an event has been handled
                }
            });

            _ = Task.Run(async () =>
            {
                await foreach (var _ in asyncEnumerable2)
                {
                    events.Add("TestEventWithParameter");
                    eventHandled.Set(); // Signal that an event has been handled
                }
            });

            // Act
            testClass.RaiseTestEvent();
            eventHandled.WaitOne(); // Wait for the event to be handled

            testClass.RaiseTestEventWithParameter("parameter");
            eventHandled.WaitOne(); // Wait for the event to be handled

            // Assert
            events[0].ShouldBe("TestEvent");
            events[1].ShouldBe("TestEventWithParameter");
        }

        [Test]
        public async Task FromEventPattern_ShouldDistinguishMultipleInstancesOfSameEvent(){
            // Arrange
            var testClass = new TestClass();
            var asyncEnumerable = testClass.WhenEventFired<EventArgs>("TestEvent");
            var counter = 0;

            // Start listening in the background
            _ = Task.Run(async () => {
                await foreach (var _ in asyncEnumerable) counter++;
            });

            // Act
            testClass.RaiseTestEvent();
            testClass.RaiseTestEvent();

            // Assert
            await Task.Delay(1000); // Wait for the event to be processed
            counter.ShouldBe(2);
        }

        [Test]
        public async Task FromEventPattern_ShouldDistinguishEventWithAndWithoutParameter()
        {
            // Arrange
            var testClass = new TestClass();
            var asyncEnumerable1 = testClass.WhenEventFired<EventArgs>("TestEvent");
            var asyncEnumerable2 = testClass.WhenEventFired<StringEventArgs>("TestEventWithParameter");
            var events = new ConcurrentBag<string>(); // Using thread-safe ConcurrentBag instead of List

            // Start listening in the background
            _ = Task.Run(async () =>
            {
                await foreach (var _ in asyncEnumerable1)
                {
                    events.Add("TestEvent");
                }
            });

            _ = Task.Run(async () =>
            {
                await foreach (var _ in asyncEnumerable2)
                {
                    events.Add("TestEventWithParameter");
                }
            });

            // Act
            testClass.RaiseTestEvent();
            testClass.RaiseTestEventWithParameter("parameter");

            // Wait a small amount of time for the handlers to run
            await Task.Delay(100);

            // Assert
            // Note that the order of events might not be guaranteed in a multithreaded environment
            events.ShouldContain("TestEvent");
            events.ShouldContain("TestEventWithParameter");
        }


    }
}