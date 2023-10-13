// using System.Reactive.Linq;
// using DevExpress.ExpressApp;
// using OutlookInspired.Module.Features.Maps;
// using XAF.Testing.RX;
// using XAF.Testing.Win.XAF;
// using XAF.Testing.XAF;
//
// namespace OutlookInspired.Tests.Assert{
//     static class MapItActionExtensions{
//         public static IObservable<Frame> AssertMapItAction(this IObservable<Frame> source,Type objectType,Func<Frame,IObservable<Frame>> assert=null) 
//             => source.SelectMany(frame => frame.View.ToDashboardView().Observe()
//                     .AssertSimpleAction(MapsViewController.MapItActionId,action => action.AssertMapItAction())
//                     .SelectMany(action => action.Trigger(action.Application.WhenFrame(objectType,ViewType.DetailView).Cast<Window>()
//                         .Do(window => ((Form)window.Template).WindowState=FormWindowState.Maximized).DelayOnContext()
//                         .SelectMany(frame1 => ((DetailView)frame1.View).AssertMapsControl().Select(control => control)
//                             .Zip(assert?.Invoke(frame1) ?? default(Frame).Observe()).Take(1)
//                             .Do(_ => frame1.Close())))).To(frame))
//                 .Take(1).IgnoreElements()
//                 .Concat(source)
//                 .ReplayFirstTake();
//     }
// }