using NUnit.Framework;
using OutlookInspired.Module.Services;
using Shouldly;

namespace Tests.EventTests{
    public class EventOrderTests{
        [Test]
        public async Task FromEventPattern_ShouldKeepOrderOfEvents(){
            var testClass = new TestClass();
            var asyncEnumerable = testClass.WhenEventFired<StringEventArgs>("TestEventWithParameter");

            var eventList = new List<StringEventArgs>();

            _ = Task.Run(async () => {
                await foreach (var e in asyncEnumerable){
                    eventList.Add(e);

                    if (eventList.Count >= 2) break;
                }
            });


            testClass.RaiseTestEventWithParameter("param1");
            await Task.Delay(100);

            testClass.RaiseTestEventWithParameter("param2");
            await Task.Delay(100);


            eventList.Count.ShouldBe(2);
            eventList[0].Value.ShouldBe("param1");
            eventList[1].Value.ShouldBe("param2");
        }

        [Test]
        public async Task FromEventPattern_ShouldKeepOrderOfMultipleSameEvents(){
            // Arrange
            var testClass = new TestClass();
            var asyncEnumerable = testClass.WhenEventFired<EventArgs>("TestEvent");
            var eventOrder = new List<string>();
            var semaphore = new SemaphoreSlim(0, 1);

            // Start listening to the events
            _ = Task.Run(async () => {
                await foreach (var _ in asyncEnumerable){
                    eventOrder.Add("TestEvent");
                    semaphore.Release(); // Signal that an event has been processed
                }
            });

            // Act
            testClass.RaiseTestEvent();
            await semaphore.WaitAsync(); // Wait for the event to be processed
            testClass.RaiseTestEvent();
            await semaphore.WaitAsync(); // Wait for the event to be processed
            testClass.RaiseTestEvent();
            await semaphore.WaitAsync(); // Wait for the event to be processed

            // Assert
            Assert.AreEqual(new List<string>{ "TestEvent", "TestEvent", "TestEvent" }, eventOrder);
        }

        [Test]
        public async Task WhenEventFired_ShouldKeepOrderOfMixedEvents(){
            // Arrange
            var testClass = new TestClass();
            var events = new List<string>();

            var eventNames = new[]
                { nameof(TestClass.TestEvent1), nameof(TestClass.TestEvent2), nameof(TestClass.TestEvent3) };

            foreach (var eventName in eventNames){
                var asyncEnumerable = testClass.WhenEventFired<EventArgs>(eventName);
                _ = Task.Run(async () => {
                    await foreach (var _ in asyncEnumerable) events.Add(eventName);
                });
            }

            // Act
            foreach (var eventName in eventNames){
                switch (eventName){
                    case nameof(TestClass.TestEvent1):
                        testClass.RaiseTestEvent1();
                        break;
                    case nameof(TestClass.TestEvent2):
                        testClass.RaiseTestEvent2();
                        break;
                    case nameof(TestClass.TestEvent3):
                        testClass.RaiseTestEvent3();
                        break;
                }

                await Task.Delay(100); // Wait for the event to be processed before raising the next one
            }

            // Assert
            for (var i = 0; i < eventNames.Length; i++) Assert.AreEqual(eventNames[i], events[i]);
        }
        
         [Test]
        public async Task WhenEventFired_ShouldKeepOrderOfMultipleDifferentEvents(){
            // Arrange
            var testClass = new TestClass();
            var events = new List<string>();

            var eventNames = new[]
                { nameof(TestClass.TestEvent1), nameof(TestClass.TestEvent2), nameof(TestClass.TestEvent3), nameof(TestClass.TestEvent4), nameof(TestClass.TestEvent5) };

            foreach (var eventName in eventNames){
                var asyncEnumerable = testClass.WhenEventFired<EventArgs>(eventName);
                _ = Task.Run(async () => {
                    await foreach (var _ in asyncEnumerable) events.Add(eventName);
                });
            }

            // Act
            foreach (var eventName in eventNames){
                switch (eventName){
                    case nameof(TestClass.TestEvent1):
                        testClass.RaiseTestEvent1();
                        break;
                    case nameof(TestClass.TestEvent2):
                        testClass.RaiseTestEvent2();
                        break;
                    case nameof(TestClass.TestEvent3):
                        testClass.RaiseTestEvent3();
                        break;
                    case nameof(TestClass.TestEvent4):
                        testClass.RaiseTestEvent4();
                        break;
                    case nameof(TestClass.TestEvent5):
                        testClass.RaiseTestEvent5();
                        break;
                }

                await Task.Delay(100); // Wait for the event to be processed before raising the next one
            }

            // Assert
            for (var i = 0; i < eventNames.Length; i++) Assert.AreEqual(eventNames[i], events[i]);
        }

        [Test]
        public async Task WhenEventFired_ShouldKeepOrderAndValueOfEventsWithParameter(){
            var testClass = new TestClass();
            var asyncEnumerable = testClass.WhenEventFired<StringEventArgs>("TestEventWithParameter");

            var eventList = new List<StringEventArgs>();

            _ = Task.Run(async () => {
                await foreach (var e in asyncEnumerable){
                    eventList.Add(e);

                    if (eventList.Count >= 3) break;
                }
            });

            testClass.RaiseTestEventWithParameter("param1");
            await Task.Delay(100);

            testClass.RaiseTestEventWithParameter("param2");
            await Task.Delay(100);

            testClass.RaiseTestEventWithParameter("param3");
            await Task.Delay(100);

            eventList.Count.ShouldBe(3);
            eventList[0].Value.ShouldBe("param1");
            eventList[1].Value.ShouldBe("param2");
            eventList[2].Value.ShouldBe("param3");
        }
    }
    }
