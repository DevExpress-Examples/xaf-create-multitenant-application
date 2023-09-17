using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Module.Features.Maps{
    public class MasterDetailUpdater:HideViewActionsUpdater{
        protected override string[] ActionIds() 
            => new[]{ "Save", "SaveAndClose", "SaveAndNew", "ShowAllContexts", "NextObject", "PreviousObject" };

        protected override string[] ViewIds() 
            => new[]{
                Customer.GridViewDetailView, Customer.LayoutViewDetailView,
                Employee.LayoutViewDetailView, Product.CardViewDetailView,
                Order.GridViewDetailView
            };
    }
}

