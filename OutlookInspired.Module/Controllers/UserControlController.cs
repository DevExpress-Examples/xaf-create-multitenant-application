using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Module.Controllers{
    public class UserControlController : ObjectViewController<DetailView,UserControlObject>{
        public UserControlController(){
            NewAction = new SimpleAction(this,"CreateNewObject",PredefinedCategory.Edit){
                ImageName = "Action_New"
            };
            NewAction.Executed += (_, e) => {
                var type = ((DetailView)e.Action.SelectionContext).ObjectTypeInfo.Type;
                var objectSpace = Application.CreateObjectSpace(type);
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, objectSpace.CreateObject(type));
            };
            DeleteAction = new SimpleAction(this,"DeleteUserControlObject",PredefinedCategory.Edit){
                ImageName = "Action_Delete",ConfirmationMessage = "You are about to delete the selected record(s). Do you want to proceed?",
                SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects
            };
            DeleteAction.Executed+=(_, e) => {
                var detailView = ((DetailView)e.Action.SelectionContext);
                detailView.ObjectSpace.Delete(detailView.CurrentObject);
                detailView.ObjectSpace.CommitChanges();
            };
            ProcessUserControlSelectedObjectAction = new SimpleAction(this,"ProcessUserControlSelectedObject",PredefinedCategory.Unspecified);
            ProcessUserControlSelectedObjectAction.Executed+=(_, e) 
                => e.ShowViewParameters.CreatedView = Application.NewDetailView(((DetailView)e.Action.SelectionContext).CurrentObject);
        }

        public SimpleAction DeleteAction{ get; }

        public SimpleAction NewAction{ get; }

        public SimpleAction ProcessUserControlSelectedObjectAction{ get; }

        protected override void OnFrameAssigned(){
            base.OnFrameAssigned();
            if (Frame.Context == TemplateContext.ApplicationWindow){
                Application.DetailViewCreated += (_, e) => {
                    if (e.View.ObjectTypeInfo.Type == typeof(UserControlObject)){
                        ((NonPersistentObjectSpace)e.View.ObjectSpace).AdditionalObjectSpaces.Add(Application.NewObjectSpace());
                    }
                };
            }
        }
    }
}