using DevExpress.ExpressApp;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Features.ViewFilter;
using OutlookInspired.Module.Services;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Win.Controllers.Quotes{
    public class ChildViewCriteriaController:ViewController<DashboardView>{
        public ChildViewCriteriaController() => TargetViewId = "Opportunities";
        protected override void OnDeactivated(){
            base.OnDeactivated();
            View.MasterFrame().View.ToListView().CollectionSource.CriteriaApplied-=CollectionSourceOnCriteriaApplied;
        }

        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            View.MasterFrame().View.ToListView().CollectionSource.CriteriaApplied+=CollectionSourceOnCriteriaApplied;
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

        private void CollectionSourceOnCriteriaApplied(object sender, EventArgs e){
            var childViewCollectionSource = ((CollectionSource)View.ChildFrame().View.ToListView().CollectionSource);
            ((ProxyCollection)childViewCollectionSource.Collection)
                .SetCollection(childViewCollectionSource.ObjectSpace
                .Opportunities(View.MasterFrame().View.ToListView().CollectionSource.Criteria[nameof(ViewFilterController)]?.ToString())
                .ToBindingList());
            
        }
    }
}