using NUnit.Framework;
using OutlookInspired.Module.Services;
using Shouldly;

namespace Tests.EventTests{
    public class ExceptionHandlingTests{
        [Test]
        public void FromEventPattern_ShouldHandleExceptionsInEventHandlers(){
            var testClass = new TestClass();
            testClass.TestEvent += (_, _) => throw new InvalidOperationException();
            var asyncEnumerable = testClass.WhenEventFired<EventArgs>("TestEvent");
            
            Should.ThrowAsync<InvalidOperationException>(async () => {
                await foreach (var _ in asyncEnumerable){
                }
            });
        }
    }
}