using DevExpress.ExpressApp;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Win.Features.Maps;

namespace OutlookInspired.Win.Features.Employees{
    [Obsolete]
    public class MyClass:ObjectViewController<ListView,Employee>{
        protected override void OnActivated(){
            base.OnActivated();

        }
    }
    public class RouteMapsViewController:RouteMapsViewController<Employee>{
        
    }
}