using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Module.Model.HideViewActions{
    public class MasterDetailViewUpdater:HideViewActionsUpdater{
        protected override string[] ActionIds() 
            => new[]{ "Save", "SaveAndClose", "SaveAndNew", "ShowAllContexts", "NextObject", "PreviousObject" };

        protected override string[] ViewIds() 
            => new[]{
                Customer.CustomerGridViewDetailView, Customer.CustomerLayoutViewDetailView,
                Employee.EmployeeLayoutViewDetailView, Product.ProductCardViewDetailView,
                Order.OrderGridViewDetailView
            };
    }
}

