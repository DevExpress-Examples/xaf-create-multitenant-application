using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Features.Maps;
using XAF.Testing;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.Services{
    public static class MapItActionExtensions{
        public static IObservable<Unit> AssertEmployeeMaps(this XafApplication application,string view, string viewVariant) 
            => application.AssertNavigation(view, viewVariant,source => source.AssertSelectDashboardListViewObject()
                .Select(frame => frame)
                .AssertMapItAction(typeof(Employee), frame => frame.AssertNestedListView(typeof(RoutePoint), assert: _ => AssertAction.HasObject)).ToUnit(),
                application.CanNavigate(view).ToUnit());
        

        public static IObservable<Unit> AssertCustomerMaps(this XafApplication application,string view, string viewVariant) 
            => application.AssertNavigation(view, viewVariant,source => source.AssertSelectDashboardListViewObject()
                .AssertMapItAction(typeof(Customer), frame => frame.AssertNestedListView(typeof(MapItem), assert: _ => AssertAction.HasObject)).ToUnit(),
                application.CanNavigate(view).ToUnit());

        public static IObservable<Unit> AssertProductMaps(this XafApplication application,string view, string viewVariant) 
            => application.AssertNavigation(view, viewVariant,source => source.AssertSelectDashboardListViewObject()
                .AssertMapItAction(typeof(Product), frame => frame.AssertNestedListView(typeof(MapItem), assert: _ => AssertAction.HasObject)).ToUnit(),
                application.CanNavigate(view).ToUnit());

        public static IObservable<Unit> AssertOrderMaps(this XafApplication application,string view, string viewVariant) 
            => application.AssertNavigation(view, viewVariant,source => source.AssertSelectDashboardListViewObject()
                .AssertMapItAction(typeof(Order), frame => ((DetailView)frame.View).AssertPdfViewer().To(frame)).ToUnit(),
                application.CanNavigate(view).ToUnit());

        public static IObservable<Unit> AssertOpportunityMaps(this XafApplication application,string view, string viewVariant) 
            => application.AssertNavigation(view, viewVariant,source => source.AssertMapItAction(typeof(Quote)).ToUnit(),
                application.CanNavigate(view).ToUnit());

        public static IObservable<Frame> AssertMapItAction(this IObservable<Frame> source,Type objectType,Func<Frame,IObservable<Frame>> assert=null) 
            => source.SelectMany(frame => frame.View.ToDashboardView().Observe()
                    .AssertSimpleAction(MapsViewController.MapItActionId,action => action.AssertMapItAction())
                    .SelectMany(action => action.Trigger(action.Application.WhenFrame(objectType, ViewType.DetailView).Cast<Window>().Take(1)
                        .WhenMaximized()
                        .SelectMany(frame1 => ((DetailView)frame1.View).AssertMapsControl().Select(unit => unit)
                            .Zip(assert?.Invoke(frame1) ?? default(Frame).Observe()).Take(1)
                            .Select(_ => frame1))))
                    .CloseWindow(frame).To(frame)
                )
                .IgnoreElements()
                .Concat(source)
                .ReplayFirstTake();
    }
}