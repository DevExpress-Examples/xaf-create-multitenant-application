using System.ComponentModel;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Testing.RXExtensions;

namespace DevExpress.ExpressApp.Testing.DevExpress.ExpressApp{
    public static class XafApplicationExtensions{
        public static IObservable<(ListView listView, XafApplication application)> WhenListViewCreated(this IObservable<XafApplication> source,Type objectType=null) 
            => source.SelectMany(application => application.WhenListViewCreated(objectType).Pair(application));

        public static IObservable<ListView> WhenListViewCreated(this XafApplication application,Type objectType=null) 
            => application.WhenEvent<ListViewCreatedEventArgs>(nameof(XafApplication.ListViewCreated))
                .Select(pattern => pattern.ListView)
                .Where(view => objectType==null||objectType.IsAssignableFrom(view.ObjectTypeInfo.Type));
        
        public static IObservable<DetailView> ToDetailView(this IObservable<(XafApplication application, DetailViewCreatedEventArgs e)> source) 
            => source.Select(_ => _.e.View);
        
        public static IObservable<(XafApplication application, DetailViewCreatedEventArgs e)> WhenDetailViewCreated(this XafApplication application,Type objectType) 
            => application.WhenDetailViewCreated().Where(_ => objectType.IsAssignableFrom(_.e.View.ObjectTypeInfo.Type));

        public static IObservable<(XafApplication application, DetailViewCreatedEventArgs e)> WhenDetailViewCreated(this XafApplication application) 
            => application.WhenEvent<DetailViewCreatedEventArgs>(nameof(XafApplication.DetailViewCreated)).InversePair(application);

        public static IObservable<Window> WhenWindowCreated(this XafApplication application,bool isMain=false,bool emitIfMainExists=true) {
            var windowCreated = application.WhenFrameCreated().Select(frame => frame).OfType<Window>();
            return isMain ? emitIfMainExists && application.MainWindow != null ? application.MainWindow.Observe().ObserveOn(SynchronizationContext.Current!)
                : windowCreated.WhenMainWindowAvailable() : windowCreated;
        }

        private static IObservable<Window> WhenMainWindowAvailable(this IObservable<Window> windowCreated) 
            => windowCreated.When(TemplateContext.ApplicationWindow).TemplateChanged().Cast<Window>()
                .SelectMany(window => window.WhenEvent("Showing").To(window)).Take(1);

        public static IObservable<Frame> WhenFrameCreated(this XafApplication application,TemplateContext templateContext=default)
            => application.WhenEvent<FrameCreatedEventArgs>(nameof(XafApplication.FrameCreated)).Select(e => e.Frame)
                .Where(frame => frame.Application==application&& (templateContext==default ||frame.Context == templateContext));

        public static IObservable<Frame> WhenFrame(this XafApplication application)
            => application.WhenFrameViewChanged();
        public static IObservable<Frame> WhenFrame(this XafApplication application, Type objectType , params ViewType[] viewTypes) 
            => application.WhenFrame(objectType).WhenFrame(viewTypes);

        public static IObservable<Frame> WhenFrame(this XafApplication application, Nesting nesting) 
            => application.WhenFrame().WhenFrame(nesting);
        
        public static IObservable<Frame> WhenFrame(this XafApplication application, params string[] viewIds) 
            => application.WhenFrame().WhenFrame(viewIds);
        
        public static IObservable<Frame> WhenFrame(this XafApplication application, Type objectType ,
            ViewType viewType = ViewType.Any, Nesting nesting = Nesting.Any) 
            => application.WhenFrame(_ => objectType,_ => viewType,nesting);
        
        public static IObservable<Frame> WhenFrame(this XafApplication application, params ViewType[] viewTypes) 
            => application.WhenFrame().WhenFrame(viewTypes);
        
        public static IObservable<Frame> WhenFrame(this XafApplication application, Func<Frame,Type> objectType,
            Func<Frame,ViewType> viewType = null, Nesting nesting = Nesting.Any) 
            => application.WhenFrame().WhenFrame(objectType,viewType,nesting);

        public static IObservable<T> WhenFrame<T>(this IObservable<T> source, Func<Frame,Type> objectType = null,
            Func<Frame,ViewType> viewType = null, Nesting nesting = Nesting.Any) where T:Frame
            => source.Where(frame => frame.When(nesting))
                .SelectMany(frame => frame.WhenFrame(viewType?.Invoke(frame)??ViewType.Any, objectType?.Invoke(frame)));
        
        public static IObservable<Window> Navigate(this XafApplication application,string viewId) 
            => application.Navigate(viewId,application.WhenFrame(viewId).Take(1)).Cast<Window>();

        public static IObservable<Frame> Navigate(this XafApplication application,string viewId, IObservable<Frame> afterNavigation) 
            => afterNavigation.Publish(source => application.MainWindow == null ? application.WhenWindowCreated(true)
                    .SelectMany(window => window.Navigate(viewId, source))
                : application.MainWindow.Navigate(viewId, source));

        private static IObservable<Frame> Navigate(this Window window,string viewId, IObservable<Frame> afterNavigation){
            var controller = window.GetController<ShowNavigationItemController>();
            return controller.ShowNavigationItemAction.Trigger(afterNavigation,
                    () => controller.FindNavigationItemByViewShortcut(new ViewShortcut(viewId, null)));
        }

        public static IObservable<Frame> WhenFrameViewChanged(this XafApplication application) 
            => application.WhenFrameCreated().Where(frame => frame.Context!=TemplateContext.ApplicationWindow).Select(frame => frame)
                .WhenViewChanged();

        public static IObservable<DetailView> WhenExistingObjectRootDetailView(this XafApplication application,Type objectType=null)
            => application.WhenExistingObjectRootDetailViewFrame(objectType).Select(frame => frame.View).Cast<DetailView>();
        public static IObservable<Frame> WhenExistingObjectRootDetailViewFrame(this XafApplication application,Type objectType=null)
            => application.WhenRootDetailViewFrame(objectType).Where(frame => !frame.View.ToDetailView().IsNewObject());

        public static IObservable<DetailView> WhenRootDetailView(this XafApplication application, Type objectType=null) 
            => application.WhenRootDetailViewFrame(objectType).Select(frame => frame.View).OfType<DetailView>();
        public static IObservable<Frame> WhenRootDetailViewFrame(this XafApplication application, Type objectType=null) 
            => application.WhenRootFrame(objectType,ViewType.DetailView);
        public static IObservable<Frame> WhenRootFrame(this XafApplication application, Type objectType=null) 
            => application.WhenRootFrame(objectType,ViewType.DetailView).WhenNotDefault(frame => frame.View.CurrentObject);

        public static IObservable<DetailView> NewObjectRootDetailView(this XafApplication application,Type objectType)
            => application.NewObjectRootFrame(objectType).Select(frame => frame.View.ToDetailView());
        public static IObservable<Frame> NewObjectRootFrame(this XafApplication application,Type objectType=null)
            => application.WhenRootFrame(objectType).Where(frame => frame.View.ToCompositeView().IsNewObject());

        public static IObservable<(Type type, object keyValue, XafApplication application)> WhenDeleteObject(this IObservable<Frame> source)
            => source.SelectMany(frame => {
                    var keyValue = frame.View.ObjectSpace.GetKeyValue(frame.View.CurrentObject);
                    var type = frame.View.ObjectTypeInfo.Type;
                    var application = frame.Application;
                    return frame.GetController<DeleteObjectsViewController>().DeleteAction
                        .Trigger(frame.WhenDisposedFrame().Select(_ => (type,keyValue,application)));
            });
        

        public static IObservable<Frame> WhenSaveNewObject(this IObservable<Frame> source)
            => source.Do(frame => frame.GetController<ModificationsController>().SaveAction.DoExecute());
        
        public static IObservable<IObjectSpace> WhenObjectSpaceCreated(this XafApplication application,bool includeNonPersistent=true,bool includeNested=false) 
            => application.WhenEvent<ObjectSpaceCreatedEventArgs>(nameof(XafApplication.ObjectSpaceCreated)).InversePair(application)
                .Where(t => (includeNonPersistent || t.source.ObjectSpace is not NonPersistentObjectSpace)&& (includeNested || t.source.ObjectSpace is not INestedObjectSpace)).Select(t => t.source.ObjectSpace);

        public static IObservable<(IObjectSpace objectSpace, CancelEventArgs e)> WhenCommiting(this XafApplication  application)
            => application.WhenObjectSpaceCreated().SelectMany(objectSpace => objectSpace.WhenCommiting().Select(e => (objectSpace,e)));
        
        public static IObservable<(IObjectSpace objectSpace, IEnumerable<T> objects)> WhenCommiting<T>(
            this XafApplication application, ObjectModification objectModification = ObjectModification.All) where T : class 
            => application.WhenObjectSpaceCreated().SelectMany(objectSpace => objectSpace.WhenCommiting<T>(objectModification));
        
        public static IObservable<(IObjectSpace objectSpace, IEnumerable<T> objects)> WhenCommitted<T>(
            this XafApplication application,ObjectModification objectModification,[CallerMemberName]string caller="") where T:class
            => application.WhenObjectSpaceCreated()
                .SelectMany(objectSpace => objectSpace.WhenCommitted<T>(objectModification,caller).TakeUntil(objectSpace.WhenDisposed()));
        public static IObservable<View> WhenRootView(this XafApplication application,Type objectType,params ViewType[] viewTypes) 
            => application.WhenRootFrame(objectType,viewTypes).Select(frame => frame.View);
        public static IObservable<Frame> WhenRootFrame(this XafApplication application,Type objectType,params ViewType[] viewTypes) 
            => application.WhenFrame(objectType,viewTypes).When(TemplateContext.View);

    }
}