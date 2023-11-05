using System.Reactive;
using DevExpress.ExpressApp;
using OutlookInspired.Module.BusinessObjects;
using XAF.Testing;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.Services{
    public static class NavigationExtensions{
        public static IObservable<Unit> AssertNavigationItemsCount(this XafApplication application) 
            => application.AssertNavigationItems((action, item) => action.NavigationItems(item)).ToUnit();

        public static IObservable<Unit> AssertNavigationViews(this XafApplication application) 
            => application.AssertNavigationViews<ApplicationUser>(applicationUser => applicationUser.NavigationViews()).ToUnit();
    }
}