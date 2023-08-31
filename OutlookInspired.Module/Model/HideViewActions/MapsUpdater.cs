using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Module.Model.HideViewActions{
    public class MapsUpdater : HideViewActionsUpdater{
        protected override string[] ActionIds() 
            => new[]{ "OpenObject" };

        protected override string[] ViewIds() 
            => new[]{
                Customer.CustomerDetailViewMaps,Employee.EmployeeDetailViewMaps,
                Product.ProductDetailViewMaps, Order.OrderDetailViewMaps
            };
    }
}