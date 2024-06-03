using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Scheduler.Win;
using DevExpress.XtraScheduler;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Win.Features.Evaluations{
    public class SchedulerListEditorController:ObjectViewController<ListView,Evaluation>{
        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            if (View.Editor is not SchedulerListEditor editor)return;
            editor.SchedulerControl.GroupType = SchedulerGroupType.None;
            editor.ResourcesMappings.Id = nameof(Employee.ID);
        }
    }
}