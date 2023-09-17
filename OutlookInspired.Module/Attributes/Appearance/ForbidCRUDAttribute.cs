using DevExpress.ExpressApp.SystemModule;

namespace OutlookInspired.Module.Attributes.Appearance{
    public class ForbidCRUDAttribute:DeactivateActionAttribute{
        public ForbidCRUDAttribute(params string[] contexts) : base("New","Save","Delete") => Context = string.Join(";", contexts);
        public ForbidCRUDAttribute(bool forbidProcessSelectedObject,params string[] contexts) : this(contexts){
            if (forbidProcessSelectedObject){
                TargetItems += $";{ListViewProcessCurrentObjectController.ListViewShowObjectActionId}".TrimStart(';');   
            }
        }
    }
}