using DevExpress.ExpressApp;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Module.Services{
    public static class XafApplicationExtensions{
        public static IObjectSpace NewObjectSpace(this XafApplication application) 
            => application.CreateObjectSpace(typeof(MigrationBaseObject));
    }
}