using Castle.Components.DictionaryAdapter;
using DevExpress.ExpressApp;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Module.Controllers.Quote{
    public abstract class QuoteMapItemController:ObjectViewController<ListView,QuoteMapItem>{

        protected override void OnFrameAssigned(){
            base.OnFrameAssigned();
            if (Frame.Context == TemplateContext.ApplicationWindow){
                Application.ObjectSpaceCreated+= (_, e) => {
                    if (e.ObjectSpace is NonPersistentObjectSpace objectSpace){
                        objectSpace.ObjectsGetting+=ObjectSpaceOnObjectsGetting;
                    }
                };
            }
        }
        
        private void ObjectSpaceOnObjectsGetting(object sender, ObjectsGettingEventArgs e){
            if (e.ObjectType == typeof(QuoteMapItem)){
                var objectSpace = ((NonPersistentObjectSpace)sender);
                objectSpace.AdditionalObjectSpaces.Add(Application.NewObjectSpace());
                e.Objects = new BindingList<QuoteMapItem>(objectSpace.GetObjectsQuery<BusinessObjects.Quote>().Opportunities().ToArray());
            }
        }
    }
}