using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Testing.DevExpress.ExpressApp;
using DevExpress.ExpressApp.Testing.RXExtensions;
using DevExpress.ExpressApp.Win;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Utils.Controls;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Tests.ImportData.Extensions{
    public static class XafApplicationExtensions{
        public static void DeleteModelDiffs(this WinApplication application){
            using var objectSpace = application.CreateObjectSpace(typeof(ModelDifference));
            objectSpace.Delete(objectSpace.GetObjectsQuery<ModelDifference>().ToArray());
            objectSpace.CommitChanges();
        }
        
        public static void ChangeStartupState(this WinApplication application,FormWindowState windowState) 
            => application.WhenFrameCreated(TemplateContext.ApplicationWindow)
                .TemplateChanged().Select(frame => frame.Template)
                .Cast<Form>()
                .Do(form => form.WindowState = windowState).Take(1)
                .Subscribe();
        
        
        


    }
}