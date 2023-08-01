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
            NewAction.Executed += (_, e) 
                => e.ShowViewParameters.CreatedView = Application.NewDetailView(((DetailView)e.Action.SelectionContext).ObjectTypeInfo.Type);
            DeleteAction = new SimpleAction(this,"DeleteUserControlObject",PredefinedCategory.Edit){
                ImageName = "Action_Delete",ConfirmationMessage = "You are about to delete the selected record(s). Do you want to proceed?",
                SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects
            };
            DeleteAction.Executed+=(_, e) 
                => ((DetailView)e.Action.SelectionContext).DeleteSelectObjects();
            ProcessUserControlSelectedObjectAction = new SimpleAction(this,"ProcessUserControlSelectedObject","Hidden");
            ProcessUserControlSelectedObjectAction.Executed+=(_, e) 
                => e.ShowViewParameters.CreatedView = Application.NewDetailView(((DetailView)e.Action.SelectionContext).CurrentObject);
        }

        public SimpleAction DeleteAction{ get; }
        public SimpleAction NewAction{ get; }
        public SimpleAction ProcessUserControlSelectedObjectAction{ get; }

        protected override void OnFrameAssigned(){
            base.OnFrameAssigned();
            if (Frame.Context == TemplateContext.ApplicationWindow){
                Application.DetailViewCreated += (sender, e) => {
                    if (e.View.ObjectTypeInfo.Type == typeof(UserControlObject)){
                        var additionalObjectSpaces = ((NonPersistentObjectSpace)e.View.ObjectSpace).AdditionalObjectSpaces;
                        if (!additionalObjectSpaces.Any()){
                            additionalObjectSpaces.Add(((XafApplication)sender).NewObjectSpace());    
                        }
                        
                    }
                };
            }
        }

        
    }
}