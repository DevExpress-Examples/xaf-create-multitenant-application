using DevExpress.ExpressApp;
using OutlookInspired.Blazor.Server.Editors.LayoutView;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Blazor.Server.Features.Customers{
    public class CustomerLayoutViewController:ObjectViewController<ListView, Customer>{
        
        
        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            if (View.Id != Customer.LayoutViewListView) return;
            var model = ((LayoutViewListEditor)View.Editor).Control;
            model.ImageSelector = o => ((Customer)o).Logo;
            model.HeaderSelector = o => ((Customer)o).Name;
            model.InfoItemsSelector = o => {
                var customer = ((Customer)o);
                return new Dictionary<string, string>{
                    { "HOME OFFICE", customer.HomeOfficeLine },
                    { "BILLING ADDRESS", customer.BillingAddressLine }
                };
            };
        }
        
    }
}