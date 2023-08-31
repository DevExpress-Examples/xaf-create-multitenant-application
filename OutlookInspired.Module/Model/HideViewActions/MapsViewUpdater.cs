using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Module.Model.HideViewActions{
    public class MapsViewUpdater : HideViewActionsUpdater{
        protected override string[] ActionIds() 
            => new[]{ "OpenObject" };

        protected override string[] ViewIds() 
            => new[]{Customer.CustomerDetailViewMaps,Employee.EmployeeDetailViewMaps,Product.ProductDetailViewMaps};
    }
}