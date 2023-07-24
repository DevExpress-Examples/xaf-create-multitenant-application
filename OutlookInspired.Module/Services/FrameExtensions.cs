using DevExpress.ExpressApp;

namespace OutlookInspired.Module.Services{
    public static class FrameExtensions{
        public static bool When<T>(this T frame, params Nesting[] nesting) where T : Frame 
            => nesting.Any(item => item == Nesting.Any || frame is NestedFrame && item == Nesting.Nested ||
                                   !(frame is NestedFrame) && item == Nesting.Root);
        public static bool When<T>(this T frame, params string[] viewIds) where T : Frame 
            => viewIds.Contains(frame.View?.Id);

        public static bool When<T>(this T frame, params ViewType[] viewTypes) where T : Frame 
            => viewTypes.Any(item =>item==ViewType.Any|| frame.View is ObjectView objectView && objectView.Is(item));
        
        public static bool When<T>(this T frame, params Type[] types) where T : Frame 
            => types.Any(item => frame.View is ObjectView objectView && objectView.Is(objectType:item));

        public static IAsyncEnumerable<T> WhenFrame<T>(this IAsyncEnumerable<T> source, params ViewType[] viewTypes) where T : Frame
            => source.Where(frame => frame.When(viewTypes));
        
        public static IAsyncEnumerable<T> WhenFrame<T>(this IAsyncEnumerable<T> source, params Nesting[] nesting) where T:Frame 
            => source.Where(frame => frame.When(nesting));
        
        public static IAsyncEnumerable<T> WhenFrame<T>(this IAsyncEnumerable<T> source, params string[] viewIds) where T:Frame 
            => source.Where(frame => frame.When(viewIds));
        
        public static IAsyncEnumerable<T> WhenFrame<T>(this IAsyncEnumerable<T> source, params Type[] objectTypes) where T:Frame 
            => source.Where(frame => frame.When(objectTypes));
        
        public static IAsyncEnumerable<T> WhenFrame<T>(this T frame,ViewType viewType, Type types) where T : Frame 
            => frame.View != null ? frame.When(viewType) && frame.When(types)
                    ? new[]{frame}.ToAsyncEnumerable().Select(frame1 => frame1) : Empty<T>()
                : frame.WhenViewChanged().Where(frame1 => frame1.When(viewType) && frame1.When(types)).To(frame);

        static IAsyncEnumerable<T> Empty<T>() => Enumerable.Empty<T>().ToAsyncEnumerable();

        public static IAsyncEnumerable<TFrame> WhenViewChanged<TFrame>(this IAsyncEnumerable<TFrame> source) where TFrame : Frame
            => source.SelectMany(frame => frame.WhenViewChanged());
        public static IAsyncEnumerable<TFrame> WhenViewChanged<TFrame>(this TFrame frame) where TFrame : Frame{
            Console.WriteLine("Subscribing to ViewChanged event for frame: " + frame);
            return frame.WhenEventFired<ViewChangedEventArgs>(nameof(Frame.ViewChanged))
                .To(frame);
        }

        public static IAsyncEnumerable<Unit> WhenDisposedFrame<TFrame>(this TFrame source) where TFrame : Frame
            => source.WhenEventFired(nameof(Frame.Disposed)).ToUnit();
    }
}