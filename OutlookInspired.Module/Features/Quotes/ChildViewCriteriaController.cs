using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Features.ViewFilter;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Module.Features.Quotes{
    public class ChildViewCriteriaController:ViewController<DashboardView>{
        public ChildViewCriteriaController() => TargetViewId = "Opportunities";
        protected override void OnDeactivated(){
            base.OnDeactivated();
            if (View.MasterItem().Frame.View is ListView listView)
                listView.CollectionSource.CriteriaApplied-=CollectionSourceOnCriteriaApplied;
        }

        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            var dashboardViewItem = View.MasterItem();
            if (dashboardViewItem.Frame != null){
                MasterDashboardViewItemOnControlCreated(dashboardViewItem,EventArgs.Empty);
            }
            else{
                dashboardViewItem.ControlCreated+=MasterDashboardViewItemOnControlCreated;
            }
            
        }

        [Obsolete("blazor")]
        private void MasterDashboardViewItemOnControlCreated(object sender, EventArgs e){
            var dashboardViewItem = ((DashboardViewItem)sender);
            dashboardViewItem.ControlCreated-=MasterDashboardViewItemOnControlCreated;
            if (dashboardViewItem.Frame.View is ListView listView){
                listView.CollectionSource.CriteriaApplied+=CollectionSourceOnCriteriaApplied;    
            }
            
        }

        protected override void OnFrameAssigned(){
            base.OnFrameAssigned();
            if (Frame.Context != TemplateContext.ApplicationWindow) return;
            Application.ObjectSpaceCreated-=Application_ObjectSpaceCreated;
            Application.ObjectSpaceCreated += Application_ObjectSpaceCreated;
        }

        private void Application_ObjectSpaceCreated(object sender, ObjectSpaceCreatedEventArgs e){
            if (e.ObjectSpace is not NonPersistentObjectSpace objectSpace) return;
            objectSpace.ObjectsGetting+= (o, args) => {
                if (args.ObjectType != typeof(QuoteMapItem)) return;
                args.Objects = ((NonPersistentObjectSpace)o)!.Opportunities().ToBindingList();
            };
        }

        [Obsolete("ondeactivated blazor")]
        private void CollectionSourceOnCriteriaApplied(object sender, EventArgs e){
            var childViewCollectionSource = ((CollectionSource)View.ChildItem().Frame.View.ToListView().CollectionSource);
            ((ProxyCollection)childViewCollectionSource.Collection)
                .SetCollection(childViewCollectionSource.ObjectSpace
                .Opportunities(View.MasterItem().Frame.View.ToListView().CollectionSource.Criteria[nameof(ViewFilterController)]?.ToString())
                .ToBindingList());
            
        }
    }
}