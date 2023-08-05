using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;

namespace OutlookInspired.Module.Controllers{
    [AttributeUsage(AttributeTargets.Class)]
    public class DetailUserControlAttribute:Attribute{
        
    }
    public interface IUserControl:ISelectionContext,IComplexControl{
        void Refresh(object currentObject);
        event EventHandler ProcessObject;
        event EventHandler DataSourceOrFilterChanged;
        
    }
}