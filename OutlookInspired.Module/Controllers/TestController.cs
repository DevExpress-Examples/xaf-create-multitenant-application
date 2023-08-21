using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Module.Controllers{
    public class TestController:ObjectViewController<DetailView,Employee>{
        public TestController(){
            var simpleAction = new SimpleAction(this,"test",PredefinedCategory.View);
            simpleAction.Executed += (sender, args) => {
                var newObjectViewController = Frame.GetController<NewObjectViewController>();
                newObjectViewController.NewObjectAction.DoExecute(newObjectViewController.NewObjectAction.Items
                    .First());
            };
        }

        protected override void OnActivated(){
            base.OnActivated();
            ObjectSpace.ModifiedChanged += (sender, args) => {

            };
        }
    }
}