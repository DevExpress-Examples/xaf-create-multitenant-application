using DevExpress.ExpressApp;

namespace OutlookInspired.Module.Services{
    internal static class FrameExtensions{
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