using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;

namespace OutlookInspired.Module.Services{
    internal static class FrameExtensions{
        public static IEnumerable<ActionBase> ActiveActions(this Frame frame, params string[] actionsIds)
            => frame.Actions(actionsIds).Where(action => action.Active);
        public static IEnumerable<ActionBase> Actions(this Frame frame,params string[] actionsIds) 
            => frame.Actions<ActionBase>(actionsIds);
        
        public static IEnumerable<T> Actions<T>(this Frame frame,params string[] actionsIds) where T : ActionBase 
            => frame.Controllers.Cast<Controller>().SelectMany(controller => controller.Actions).OfType<T>()
                .Where(actionBase => !actionsIds.Any()|| actionsIds.Any(s => s==actionBase.Id));
        public static bool When<T>(this T frame, params Nesting[] nesting) where T : Frame 
            => nesting.Any(item => item == Nesting.Any || frame is NestedFrame && item == Nesting.Nested ||
                                   !(frame is NestedFrame) && item == Nesting.Root);
        public static bool When<T>(this T frame, params string[] viewIds) where T : Frame 
            => viewIds.Contains(frame.View?.Id);

        public static bool When<T>(this T frame, params ViewType[] viewTypes) where T : Frame 
            => viewTypes.Any(viewType =>viewType==ViewType.Any|| frame.View is CompositeView compositeView && compositeView.Is(viewType));
        
        public static bool When<T>(this T frame, params Type[] types) where T : Frame 
            => types.Any(item => frame.View is ObjectView objectView && objectView.Is(objectType:item));

        public static T ParentObject<T>(this Frame frame) where T : class => frame.As<NestedFrame>()?.ViewItem.View.CurrentObject as T;
        
        public static T As<T>(this object obj) 
            => obj is T variable ? variable : default;
    }
}