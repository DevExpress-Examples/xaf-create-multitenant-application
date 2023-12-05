using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Office;
using DevExpress.ExpressApp.SystemModule;

namespace OutlookInspired.Module.Services.Internal{
    
    internal static class FrameExtensions{
        public static void UseObjectDefaultDetailView(this NewObjectViewController controller){
            void Handler(object sender, CreateCustomDetailViewEventArgs e){
                e.ViewId = controller.Application.FindDetailViewId(controller.Frame.View.ObjectTypeInfo.Type);
                controller.CreateCustomDetailView -= Handler;
            }
            controller.CreateCustomDetailView += Handler;
        }

        public static List<T> GetControllers<T>(this Frame frame) where T:class{
            var controllers = new List<T>();
            foreach (var controller in frame.Controllers){
                if (controller is T t)
                    controllers.Add(t);
            }
            return  controllers;
        }

        public static void ShowInDocument(this Frame frame, string template){
            var showInDocumentAction = frame.GetController<RichTextShowInDocumentControllerBase>().ShowInDocumentAction;
            showInDocumentAction.Active.RemoveItem("ByAppearance");
            void OnDetailViewCreated(object sender, DetailViewCreatedEventArgs e){
                e.View.Caption = template;
                frame.Application.DetailViewCreated-=OnDetailViewCreated;
            }
            frame.Application.DetailViewCreated += OnDetailViewCreated;
            showInDocumentAction.DoExecute(item => ((MailMergeDataInfo)item.Data).DisplayName == template);
            showInDocumentAction.Active["ByAppearance"] = false;
        }

        public static IEnumerable<ActionBase> ActiveActions(this Frame frame, params string[] actionsIds)
            => frame.Actions(actionsIds).Where(action => action.Active);
        public static IEnumerable<ActionBase> Actions(this Frame frame,params string[] actionsIds) 
            => frame.Actions<ActionBase>(actionsIds);
        
        public static IEnumerable<T> Actions<T>(this Frame frame,params string[] actionsIds) where T : ActionBase 
            => frame.Controllers.Cast<Controller>().SelectMany(controller => controller.Actions).OfType<T>()
                .Where(actionBase => !actionsIds.Any()|| actionsIds.Any(s => s==actionBase.Id));

        public static T ParentObject<T>(this Frame frame) where T : class
            => frame.ToNestedFrame().ViewItem.CurrentObject as T;

        public static NestedFrame ToNestedFrame(this Frame frame) => (NestedFrame)frame;
        
        public static T As<T>(this object obj) 
            => obj is T variable ? variable : default;
    }
}