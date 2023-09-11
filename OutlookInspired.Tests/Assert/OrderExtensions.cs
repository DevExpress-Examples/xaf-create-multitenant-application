using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraLayout;
using OutlookInspired.Module.BusinessObjects;
using XAF.Testing;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.ImportData.Assert{
    static class OrderExtensions{
        internal static IObservable<Frame> AssertOrderReportsAction(this IObservable<Frame> source){
            return source.DashboardViewItem(item => item.MasterViewItem()).ToFrame()
                .AssertSingleChoiceAction(Module.Controllers.Orders.ReportController.ReportActionId, 2,item => item.Data == null ? 2 : 0)
                .AssertReports(item => item.ParentItem is{ Data: null })
                .AssertOrderInvoice()
                .IgnoreElements().Concat(source).ReplayFirstTake();
        }
        internal static IObservable<Frame> AssertOrderInvoice(this IObservable<SingleChoiceAction> source)
            => source.SelectMany(action => action.Items.SelectManyRecursive(item => item.Items).Where(item => item.ParentItem==null&&!item.Items.Any()).ToNowObservable()
                .SelectManySequential(item => action.Trigger(action.Application.WhenFrame(typeof(Order),ViewType.DetailView).Take(1)
                    .SelectUntilViewClosed(frame => ((DetailView)frame.View).AssertRichEditControl().To(frame)
                        .CloseWindow().To(action.Frame())),() => item)));
        
        internal static IObservable<Unit> AssertOrderDetailView(this Frame frame, IObservable<TabbedGroup> orderTabControl) 
            => frame.AssertNestedOrderItems( )
                .ReplayFirstTake();    
    }
}