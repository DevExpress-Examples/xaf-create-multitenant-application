using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Module.Services{
    public static class XafApplicationExtensions{
        public static IAsyncEnumerable<Frame> WhenFrameCreated(this XafApplication application){
            Console.WriteLine("Subscribing to FrameCreated event for ");
            return application.WhenEventFired<FrameCreatedEventArgs>(nameof(application.FrameCreated))
                .Do(args => Console.WriteLine("WhenFrameCreated fires "))
                .Select(e => e.Frame);
        }

        public static IAsyncEnumerable<Frame> WhenFrame(this XafApplication application)
            => application.WhenFrameViewChanged();
        public static IAsyncEnumerable<Frame> WhenFrame(this XafApplication application, Type objectType , params ViewType[] viewTypes) 
            => application.WhenFrame(objectType).WhenFrame(viewTypes);

        public static IAsyncEnumerable<Frame> WhenFrame(this XafApplication application, Nesting nesting) 
            => application.WhenFrame().WhenFrame(nesting);
        
        public static IAsyncEnumerable<Frame> WhenFrame(this XafApplication application, params string[] viewIds) 
            => application.WhenFrame().WhenFrame(viewIds);
        
        public static IAsyncEnumerable<Frame> WhenFrame(this XafApplication application, Type objectType ,
            ViewType viewType = ViewType.Any, Nesting nesting = Nesting.Any) 
            => application.WhenFrame(_ => objectType,_ => viewType,nesting);
        
        public static IAsyncEnumerable<Frame> WhenFrame(this XafApplication application, Func<Frame,Type> objectType,
            Func<Frame,ViewType> viewType = null, Nesting nesting = Nesting.Any) 
            => application.WhenFrame().WhenFrame(objectType,viewType,nesting);

        public static IAsyncEnumerable<T> WhenFrame<T>(this IAsyncEnumerable<T> source, Func<Frame,Type> objectType = null,
            Func<Frame,ViewType> viewType = null, Nesting nesting = Nesting.Any) where T:Frame
            => source.Where(frame => frame.When(nesting))
                .SelectMany(frame => frame.WhenFrame(viewType?.Invoke(frame)??ViewType.Any, objectType?.Invoke(frame)));
        
        public static IAsyncEnumerable<Frame> Navigate(this XafApplication application,string viewId) 
            => application.Navigate(viewId,application.WhenFrame(viewId));

        public static IAsyncEnumerable<Frame> Navigate(this XafApplication application,string viewId, IAsyncEnumerable<Frame> afterNavigation) 
            => application.Defer(() => {
                var controller = application.MainWindow.GetController<ShowNavigationItemController>();
                controller.ShowNavigationItemAction.SelectedItem= controller.FindNavigationItemByViewShortcut(new ViewShortcut(viewId, null));
                return controller.ShowNavigationItemAction.Trigger(afterNavigation);
            });
        
        public static IAsyncEnumerable<Frame> WhenFrameViewChanged(this XafApplication application) 
            => application.WhenFrameCreated().Where(frame => frame.Context!=TemplateContext.ApplicationWindow).Select(frame => frame).WhenViewChanged();

        public static IObjectSpace NewObjectSpace(this XafApplication application) 
            => application.CreateObjectSpace(typeof(MigrationBaseObject));
    }
}