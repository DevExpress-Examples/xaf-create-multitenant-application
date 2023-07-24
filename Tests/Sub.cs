// using NUnit.Framework;
// using OutlookInspired.Module.Services;
// using Tests.Extensions;
//
// namespace Tests{
//     public class Sub{
//         [Test]
//         public async Task TestEmissionOfSubsequentEvents()
//         {
//             var someClass1 = new SomeClass("1");
//             var someClass2 = new SomeClass("2");
//
//             // start observing events
//             var eventObservable = new[] { someClass1, someClass2 }
//                 .Select(someClass => someClass.WhenEventFired("Event1")).ToArray()
//                     
//                     ;
//
//             // var testObserver = new TestObserver<SomeClass>(eventObservable);
//             // var task = Task.Run(async () => await testObserver.EnumerateAsync());
//             //
//             // // Mock event emissions for Event1
//             // await Task.Delay(1000);
//             // someClass1.RaiseEvent1();
//             // someClass2.RaiseEvent1();
//             //
//             // // proceed with the rest of the test
//             // await Task.Delay(1000);
//             // someClass1.RaiseEvent2();
//             //
//             // await Task.Delay(1000);
//             // someClass2.RaiseEvent2();
//             // someClass2.RaiseEvent2();
//
//             // await task;
//             // Assert.AreEqual(4, testObserver.ItemsCount);        }
//         
//     }
//
//     public abstract class EventArgsBase:EventArgs{
//         private readonly string _s;
//
//         protected EventArgsBase(string s){
//             _s = s;
//         }
//
//         public override string ToString(){
//             return $"{GetType().Name}={_s}";
//         }
//     }
//
//     public class SomeClass{
//         private readonly string _s;
//         public event EventHandler<EventArgs1> Event1;
//         public event EventHandler<EventArgs2> Event2;
//
//         public SomeClass(string s){
//             _s = s;
//         }
//
//         
//         public void RaiseEvent1()
//         {
//             Event1.Invoke(this,new EventArgs1(_s));
//         }
//
//         public void RaiseEvent2()
//         {
//             Event2.Invoke(this,new EventArgs2(_s));
//         }
//
//            }
//
//     public class EventArgs2:EventArgsBase{
//         public EventArgs2(string s) : base(s){
//         }
//     }
//     public class EventArgs1:EventArgsBase{
//         public EventArgs1(string s) : base(s){
//         }
//     }
// }