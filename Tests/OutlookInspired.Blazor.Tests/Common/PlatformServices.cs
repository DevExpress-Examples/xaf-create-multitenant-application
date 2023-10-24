using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using OutlookInspired.Blazor.Server;
using OutlookInspired.Blazor.Server.Components.DevExtreme.Maps;
using OutlookInspired.Blazor.Server.Components.Models;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Tests.Assert;
using XAF.Testing;
using XAF.Testing.Blazor.XAF;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Blazor.Tests.Common{
    public class MapControlAssertion : IMapsControlAssertion{
        public IObservable<Unit> Assert(DetailView detailView) 
            => detailView.AssertViewItemControl<DxMapModel>(model => model.WhenEvent(nameof(ComponentModelBase.Ready))
                .Select(pattern => pattern).ToUnit()).ToUnit();
    }

    public class FilterViewAssertion:IFilterViewAssertion{
        public IObservable<Frame> Assert(IObservable<SingleChoiceAction> source) 
            => source.AssertDialogControllerListView(typeof(ViewFilter), _ => AssertAction.AllButProcess)
                .ToSecond().IgnoreElements();
    }
    
    public class DashboardColumnViewObjectSelector : IDashboardColumnViewObjectSelector{
        public IObservable<Unit> SelectDashboardColumnViewObject(DashboardViewItem item) 
            => item.InnerView.ToDetailView().GetItems<ControlViewItem>().Select(viewItem => viewItem.Control)
                .Cast<UserControlComponentModel<Employee>>().Do(model => model.SelectObject(model.Objects.First())).ToNowObservable()
                .ToUnit();
    }

}