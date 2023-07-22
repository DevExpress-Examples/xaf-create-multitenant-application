using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using OutlookInspired.Module;
using OutlookInspired.Module.Services;
using Shouldly;

namespace Tests.EventTests{
    public class BasicEventHandlingTests{
        [Test]
        public async Task FromEventPattern_ShouldYieldWhenEventIsRaised(){
            var testClass = new TestClass();
            var asyncEnumerable = testClass.WhenEventFired<EventArgs>("TestEvent");

            testClass.RaiseTestEvent();

            var e = await asyncEnumerable.FirstAsync(); 
            e.ShouldNotBeNull(); 
        }
        [Test]
        public async Task FromEventPattern_ShouldNotYieldWhenEventIsNotRaised(){
            var testClass = new TestClass();
            var asyncEnumerable = testClass.WhenEventFired<EventArgs>("TestEvent");

            var cts = new CancellationTokenSource(1000); 
            var e = await asyncEnumerable.FirstOrDefaultAsync(cts.Token); 
            e.ShouldBeNull();
        }

        [Test]
        public void FromEventPattern_ShouldThrowExceptionWhenEventDoesNotExist(){
            var testClass = new TestClass();

            Should.Throw<ArgumentException>(() => testClass.WhenEventFired<EventArgs>("NonexistentEvent"));
        }

        [Test]
        public async Task FromEventPattern_ShouldNotYieldWhenDifferentEventIsRaised(){
            var testClass = new TestClass();
            var asyncEnumerable = testClass.WhenEventFired<EventArgs>("TestEvent");
            
            testClass.RaiseTestEventWithParameter("parameter");
            
            var cts = new CancellationTokenSource(1000);
            var e = await asyncEnumerable.FirstOrDefaultAsync(cts.Token); 
            e.ShouldBeNull();
        }

        [Test]
        public async Task FromEventPattern_ShouldYieldWhenEventWithParameterIsRaised(){
            var testClass = new TestClass();
            var asyncEnumerable = testClass.WhenEventFired<StringEventArgs>("TestEventWithParameter");
            
            testClass.RaiseTestEventWithParameter("parameter");
            
            var e = await asyncEnumerable.FirstAsync(); 
            e.Value.ShouldBe("parameter"); // The event argument should be "parameter"
        }

        [Test]
        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task FromEventPattern_ShouldNotYieldWhenEventWithParameterIsNotRaised(){
            var testClass = new TestClass();
            var asyncEnumerable = testClass.WhenEventFired<StringEventArgs>("TestEventWithParameter");

            var cancellationTokenSource = new CancellationTokenSource(1000); 

            var task = Task.Run(async () => {
                await foreach (var _ in asyncEnumerable.WithCancellation(cancellationTokenSource.Token))
                    Assert.Fail("The event was raised.");
            }, cancellationTokenSource.Token);

            
            if (await Task.WhenAny(task, Task.Delay(1100)) == task){
            }
            else{
                cancellationTokenSource.Cancel(); 
                await task; 
            }
        }

        [Test]
        public async Task FromEventPattern_ShouldYieldWhenEventWithParameterIsRaisedWithCorrectParameter(){
            var testClass = new TestClass();
            var asyncEnumerable = testClass.WhenEventFired<StringEventArgs>("TestEventWithParameter");
            
            testClass.RaiseTestEventWithParameter("parameter");

            var e = await asyncEnumerable.FirstAsync(); 
            e.Value.ShouldBe("parameter"); 
        }

        [Test]
        public async Task FromEventPattern_ShouldYieldMultipleTimesWhenEventIsRaisedMultipleTimes(){
            var testClass = new TestClass();
            var asyncEnumerable = testClass.WhenEventFired<EventArgs>("TestEvent");

            testClass.RaiseTestEvent();
            testClass.RaiseTestEvent();

            var count = 0;
            await foreach (var _ in asyncEnumerable.WithCancellation(new CancellationTokenSource(1000).Token)){
                count++;
                if (count == 2) break;
            }

            count.ShouldBe(2); 
        }

        [Test]
        public async Task FromEventPattern_ShouldNotYieldWhenEventIsRaisedOnDifferentInstance(){
            var testClass1 = new TestClass();
            var testClass2 = new TestClass();
            var asyncEnumerable = testClass1.WhenEventFired<EventArgs>("TestEvent");

            testClass2.RaiseTestEvent();

            var cts = new CancellationTokenSource(1000); 
            var e = await asyncEnumerable.FirstOrDefaultAsync(cts.Token); 
            e.ShouldBeNull();
        }

        [Test]
        public async Task FromEventPattern_ShouldYieldWhenEventWithParameterIsRaisedWithNullParameter(){
            var testClass = new TestClass();
            var asyncEnumerable = testClass.WhenEventFired<StringEventArgs>("TestEventWithParameter");

            testClass.RaiseTestEventWithParameter(null!);
            
            var e = await asyncEnumerable.FirstAsync(); 
            e.Value.ShouldBeNull(); 
        }

    }
}