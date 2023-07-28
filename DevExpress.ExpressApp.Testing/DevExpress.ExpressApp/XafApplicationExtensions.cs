using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Testing.RXExtensions;
using DevExpress.ExpressApp.Win;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Utils.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Layout;
using DevExpress.XtraGrid.Views.Layout.Handler;

namespace DevExpress.ExpressApp.Testing.DevExpress.ExpressApp{
    public static class XafApplicationExtensions{
        public static TestObserver<T> StartWinTest<T>(this WinApplication application, IObservable<T> test) 
            => application.Start( test, new WindowsFormsSynchronizationContext());

        private static TestObserver<T> Start<T>(this WinApplication application, IObservable<T> test, SynchronizationContext context) 
            => application.Start(application.WhenFrameCreated().Take(1)
                .Do(_ => SynchronizationContext.SetSynchronizationContext(context))
                .IgnoreElements().To<T>()
                .Merge(test.BufferUntilCompleted().Do(_ => application.Exit()).SelectMany()
                    .DoNotComplete())
                .Catch<T, Exception>(exception => {
                    context.Send(_ => application.Exit(),null);
                    return Observable.Throw<T>(exception);
                })
            );

        public static void DeleteModelDiffs(this WinApplication application){
            using var objectSpace = application.CreateObjectSpace(typeof(ModelDifference));
            objectSpace.Delete(objectSpace.GetObjectsQuery<ModelDifference>().ToArray());
            objectSpace.CommitChanges();
        }
        public static void ChangeStartupState(this WinApplication application,FormWindowState windowState) 
            => application.WhenFrameCreated(TemplateContext.ApplicationWindow)
                .TemplateChanged().Select(frame => frame.Template)
                .Cast<Form>()
                .Do(form => form.WindowState = windowState).Take(1)
                .Subscribe();


        public static TestObserver<T> Start<T>(this WinApplication application, IObservable<T> test){
            var testObserver = test.Test();
            application.Start();
            testObserver.Error?.ThrowCaptured();
            return testObserver;
        }
        
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
            => application.Navigate(viewId,application.WhenFrame(viewId)).Cast<Window>();

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

        public static IObservable<DetailView> ExistingObjectRootDetailView(this XafApplication application,Type objectType=null)
            => application.RootDetailView(objectType).Where(detailView => !detailView.IsNewObject());

        public static IObservable<DetailView> RootDetailView(this XafApplication application, Type objectType=null) 
            => application.RootFrame(objectType,ViewType.DetailView).Select(frame => frame.View).Cast<DetailView>();
        public static IObservable<Frame> RootFrame(this XafApplication application, Type objectType=null) 
            => application.RootFrame(objectType,ViewType.DetailView).WhenNotDefault(frame => frame.View.CurrentObject);

        public static IObservable<DetailView> NewObjectRootDetailView(this XafApplication application,Type objectType)
            => application.NewObjectRootFrame(objectType).Select(frame => frame.View.ToDetailView());
        public static IObservable<Frame> NewObjectRootFrame(this XafApplication application,Type objectType=null)
            => application.RootFrame(objectType).Where(frame => frame.View.ToCompositeView().IsNewObject());
        
        public static IObservable<bool> DeleteCurrentObject(this XafApplication application)
            => application.NewObjectRootFrame().SelectMany(frame => frame.View.ObjectSpace.WhenCommitted<object>(ObjectModification.New)
                .WaitUntilInactive(1.Seconds()).ObserveOnContext()
                .Select(_ => {
                    var keyValue = frame.View.ObjectSpace.GetKeyValue(frame.View.CurrentObject);
                    var type = frame.View.ObjectTypeInfo.Type;
                    var deleteObjectsViewController = frame.GetController<DeleteObjectsViewController>();
                    deleteObjectsViewController.DeleteAction.ConfirmationMessage = null;
                    deleteObjectsViewController.DeleteAction.DoExecute();
                    return application.NewObjectSpace().GetObjectByKey(type, keyValue)==null;
                }).WhenNotDefault());
        
        public static IObservable<Unit> SaveNewObject(this XafApplication application)
            => application.NewObjectRootFrame()
                .SelectMany(frame => frame.View.ToDetailView().CloneRequiredMembers().ToNowObservable()
                    .ConcatToUnit(frame.GetController<ModificationsController>().SaveAction.Observe().Do(action => action.DoExecute())));

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
        public static IObservable<View> RootView(this XafApplication application,Type objectType,params ViewType[] viewTypes) 
            => application.RootFrame(objectType,viewTypes).Select(frame => frame.View);
        public static IObservable<Frame> RootFrame(this XafApplication application,Type objectType,params ViewType[] viewTypes) 
            => application.WhenFrame(objectType,viewTypes).When(TemplateContext.View);
        
        public static IObservable<LayoutView> ProcessEvent(this IObservable<GridControl> source,EventType eventType)
            => source.Select(control => control.MainView).Cast<LayoutView>()
                .SelectMany(layoutView => layoutView.ProcessEvent(eventType));

        public static IObservable<LayoutView> ProcessEvent(this LayoutView layoutView,EventType eventType) 
            => layoutView.WhenEvent<FocusedRowObjectChangedEventArgs>(nameof(layoutView.FocusedRowObjectChanged))
                .WaitUntilInactive(1.Seconds()).ObserveOnContext()
                .WhenNotDefault(e => e.Row)
                .Do(_ => new LayoutViewHandler(layoutView).ProcessEvent(eventType, EventArgs.Empty))
                .To(layoutView)
                .Merge(Observable.Defer(() => {
                    var row = layoutView.FindRow(((IEnumerable<object>)layoutView.DataSource).First());
                    layoutView.Focus();
                    layoutView.SelectRow(row);
                    layoutView.SelectRow(2);
                    layoutView.FocusedRowHandle = 2;
                    return Observable.Empty<LayoutView>();
                }).To<LayoutView>());


        public static IObservable<(GridControl gridControl, Frame frame)> WhenGridControl(this IObservable<Frame> source) 
            => source.Select(frame => frame).OfView<DetailView>().SelectMany(frame =>
                frame.View.ToDetailView().WhenGridControl().Select(gridControl => (gridControl, frame)));

        public static IObservable<GridControl> WhenGridControl(this DetailView detailView) 
            => detailView.GetItems<ControlViewItem>().ToNowObservable()
                .SelectMany(item => item.WhenControlCreated().Select(_ => item.Control).StartWith(item.Control).WhenNotDefault().Cast<Control>())
                .SelectMany(control => control.Controls.OfType<GridControl>())
                // .SelectMany(control => control.WhenEvent(nameof(GridControl.HandleCreated)).To(control))
            ;
    }
}