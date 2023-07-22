using NUnit.Framework;
using OutlookInspired.Module;
using OutlookInspired.Module.Services;
using Shouldly;

namespace Tests.EventTests{
    public class MultiParameterEventHandlingTests{
        [Test]
        public async Task FromEventPattern_ShouldHandleMultipleParameters(){
            
            var testClass = new TestClass();
            var asyncEnumerable = testClass.WhenEventFired<MultiStringEventArgs>("TestEventWithMultipleParameters");
            
            testClass.RaiseTestEventWithMultipleParameters("param1", "param2");

            var e = await asyncEnumerable.FirstAsync(); 
            e.Value1.ShouldBe("param1");
            e.Value2.ShouldBe("param2");
        }
    }
}