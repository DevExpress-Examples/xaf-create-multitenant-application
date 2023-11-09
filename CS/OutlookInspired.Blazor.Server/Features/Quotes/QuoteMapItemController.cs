using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using OutlookInspired.Blazor.Server.Components.Models;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Blazor.Server.Features.Quotes{
    public class QuoteMapItemController:ViewController<DashboardView>{
        
        public QuoteMapItemController() => TargetViewId = "Opportunities";

        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            View.MasterItem().ControlCreated+=MasterDashboardViewItemOnControlCreated;
        }
        
        private void MasterDashboardViewItemOnControlCreated(object sender, EventArgs e){
            var dashboardViewItem = ((DashboardViewItem)sender);
            dashboardViewItem.ControlCreated-=MasterDashboardViewItemOnControlCreated;
            dashboardViewItem.Frame.View.ToDetailView().GetItems<ControlViewItem>().First().ControlCreated+=OnControlCreated;
        }

        private void OnControlCreated(object sender, EventArgs e){
            var controlViewItem = ((ControlViewItem)sender);
            controlViewItem.ControlCreated-=OnControlCreated;
            ((UserControlComponentModel)controlViewItem.Control).CriteriaChanged+=OnCriteriaChanged;
        }

        private void OnCriteriaChanged(object sender, EventArgs e) 
            => View.ChildItem().Frame.View.ToDetailView().GetItems<ControlViewItem>()
                .Select(item => item.Control).Cast<UserControlComponentModel>().First()
                .SetCriteria(((UserControlComponentModel)sender).Criteria?.ToString());
    }
}