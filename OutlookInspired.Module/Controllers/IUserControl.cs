using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;

namespace OutlookInspired.Module.Controllers{
    public interface IUserControl:ISelectionContext,IComplexControl{
        void Refresh(object currentObject);
        event EventHandler ProcessObject;
    }
}