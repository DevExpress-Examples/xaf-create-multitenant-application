using DevExpress.ExpressApp;
using DevExpress.ExpressApp.MultiTenancy;
using Microsoft.Extensions.DependencyInjection;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Module.Features{
    public class WelcomeController:ObjectViewController<DetailView,Welcome>{
        private Welcome _welcome;
        protected override void OnFrameAssigned(){
            base.OnFrameAssigned();
            if (Application.ServiceProvider.GetRequiredService<ITenantProvider>().TenantId == null) return;
            if (Frame.Context != TemplateContext.ApplicationWindow) return;
            Application.ObjectSpaceCreated-=ApplicationOnObjectSpaceCreated;
            Application.ObjectSpaceCreated+=ApplicationOnObjectSpaceCreated;
            _welcome = Application.CreateObjectSpace(typeof(Welcome)).CreateObject<Welcome>();
        }
        
        private void ApplicationOnObjectSpaceCreated(object sender, ObjectSpaceCreatedEventArgs e){
            if (e.ObjectSpace is not NonPersistentObjectSpace nonPersistentObjectSpace) return;
            nonPersistentObjectSpace.ObjectByKeyGetting += nonPersistentObjectSpace_ObjectByKeyGetting;
        }

        private void nonPersistentObjectSpace_ObjectByKeyGetting(object sender, ObjectByKeyGettingEventArgs e){
            if (!e.ObjectType.IsAssignableFrom(typeof(Welcome))) return;
            e.Object = _welcome;
        }
    }
}