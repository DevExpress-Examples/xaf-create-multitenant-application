using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;

namespace OutlookInspired.Module.Controllers{
    public class TestController:ViewController{
        public TestController(){
            var simpleAction = new SimpleAction(this,"test",PredefinedCategory.View);
            simpleAction.Executed += (sender, args) => {
                var newObjectViewController = Frame.GetController<NewObjectViewController>();
                newObjectViewController.NewObjectAction.DoExecute(newObjectViewController.NewObjectAction.Items
                    .First());
            };
        }
    }
}