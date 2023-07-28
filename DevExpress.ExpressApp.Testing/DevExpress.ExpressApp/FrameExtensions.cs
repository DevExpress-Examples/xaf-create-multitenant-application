using System.Reactive.Linq;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Testing.RXExtensions;
using DevExpress.ExpressApp.ViewVariantsModule;
using DevExpress.Utils.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Layout;
using Unit = System.Reactive.Unit;

namespace DevExpress.ExpressApp.Testing.DevExpress.ExpressApp{
    public static class FrameExtensions{
        internal static bool When<T>(this T frame, params Nesting[] nesting) where T : Frame 
            => nesting.Any(item => item == Nesting.Any || frame is NestedFrame && item == Nesting.Nested ||
                                   !(frame is NestedFrame) && item == Nesting.Root);

        internal static bool When<T>(this T frame, params string[] viewIds) where T : Frame 
            => viewIds.Contains(frame.View?.Id);

        internal static bool When<T>(this T frame, params ViewType[] viewTypes) where T : Frame 
            => viewTypes.Any(viewType =>viewType==ViewType.Any|| frame.View is CompositeView compositeView && compositeView.Is(viewType));

        internal static bool When<T>(this T frame, params Type[] types) where T : Frame 
            => types.Any(item => frame.View is ObjectView objectView && objectView.Is(objectType:item));
        
        public static IObservable<Frame> ListViewProcessSelectedItem(this Frame frame,Action<SimpleActionExecuteEventArgs> executed) 
            => frame.ListViewProcessSelectedItem(() => frame.View.SelectedObjects.Cast<object>().First() ,executed);

        public static IObservable<Frame> ListViewProcessSelectedItem<T>(this Frame frame, Func<T> selectedObject,Action<SimpleActionExecuteEventArgs> executed=null){
            var action = frame.GetController<ListViewProcessCurrentObjectController>().ProcessCurrentObjectAction;
            var invoke = selectedObject.Invoke()??default(T);
            var afterNavigation = action.WhenExecuted().DoWhen(_ => executed != null, e => executed!(e))
                .SelectMany(e => frame.Application.WhenFrame(e.ShowViewParameters.CreatedView.ObjectTypeInfo.Type).Take(1));
            return action.Trigger(afterNavigation,invoke.YieldItem().Cast<object>().ToArray());
        }

        
        public static IObservable<IFrameContainer> NestedFrameContainers<TWindow>(this TWindow window,
            params Type[] objectTypes) where TWindow : Window
            => window.View.ToCompositeView().NestedFrameContainers(objectTypes);
        public static IObservable<T> WhenFrame<T>(this IObservable<T> source, params ViewType[] viewTypes) where T : Frame
            => source.Where(frame => frame.When(viewTypes));
        
        public static IObservable<T> ToController<T>(this IObservable<Frame> source) where T : Controller 
            => source.SelectMany(window => window.Controllers.Cast<Controller>()).OfType<T>();

        public static IObservable<Window> CloseWindow<TFrame>(this IObservable<TFrame> source) where TFrame:Frame 
            => source.SelectMany(frame => frame.View.WhenActivated().To(frame).WaitUntilInactive(1.Seconds()).ObserveOnContext())
                .Cast<Window>().Do(frame => frame.Close());

        public static IObservable<T> WhenFrame<T>(this IObservable<T> source, params Nesting[] nesting) where T:Frame 
            => source.Where(frame => frame.When(nesting));
        
        public static IObservable<T> WhenFrame<T>(this IObservable<T> source, params string[] viewIds) where T:Frame 
            => source.Where(frame => frame.When(viewIds));
        
        public static IObservable<T> WhenFrame<T>(this IObservable<T> source, params Type[] objectTypes) where T:Frame 
            => source.Where(frame => frame.When(objectTypes));
        
        public static IObservable<TFrame> WhenTemplateChanged<TFrame>(this TFrame item) where TFrame : Frame 
            => item.WhenEvent(nameof(Frame.TemplateChanged)).Select(pattern => pattern).To(item)
                .TakeUntil(item.WhenDisposedFrame());

        public static IObservable<TFrame> When<TFrame>(this IObservable<TFrame> source, TemplateContext templateContext) where TFrame : Frame 
            => source.Where(window => window.Context == templateContext);
        
        public static IObservable<T> TemplateChanged<T>(this IObservable<T> source) where T : Frame 
            => source.SelectMany(item => item.Template != null ? item.Observe() : item.WhenTemplateChanged().Select(_ => item));
        public static IObservable<T> WhenFrame<T>(this T frame,ViewType viewType, Type types) where T : Frame 
            => frame.View != null ? frame.When(viewType) && frame.When(types) ? frame.Observe() : Observable.Empty<T>()
                : frame.WhenViewChanged().Where(t => t.When(viewType) && t.When(types)).To(frame);
        public static IObservable<TFrame> WhenViewChanged<TFrame>(this IObservable<TFrame> source) where TFrame : Frame
            => source.SelectMany(frame => frame.WhenViewChanged());
        
        public static IObservable<TFrame > WhenViewChanged<TFrame>(this TFrame item) where TFrame : Frame 
            => item.WhenEvent<ViewChangedEventArgs>(nameof(Frame.ViewChanged))
                .TakeUntil(item.WhenDisposedFrame()).Select(_ => item);
        public static IObservable<ViewItem> ViewItems(this Window frame,params Type[] objectTypes) 
            => frame.NestedFrameContainers(objectTypes).OfType<ViewItem>();
        
        public static IEnumerable<DashboardViewItem> DashboardViewItems(this Window frame,params Type[] objectTypes) 
            => frame.View.ToCompositeView().GetItems<DashboardViewItem>().Where(item => item.InnerView.Is(objectTypes));
        
        public static IEnumerable<DashboardViewItem> DashboardViewItems(this Window frame,ViewType viewType,params Type[] objectTypes) 
            => frame.DashboardViewItems(objectTypes).When(viewType);

        public static IEnumerable<DashboardViewItem> When(this IEnumerable<DashboardViewItem> source, params ViewType[] viewTypes) 
            => source.Where(item => viewTypes.All(viewType => item.InnerView.Is(viewType)));

        public static IEnumerable<TViewType> DashboardViewItems<TViewType>(this Window frame,params Type[] objectTypes) where TViewType:View
            => frame.DashboardViewItems(objectTypes).Select(item => item.Frame.View as TViewType).WhereNotDefault();
        
        public static IObservable<ListPropertyEditor> NestedListViews(this Frame frame, params Type[] objectTypes ) 
            => frame.View.ToDetailView().NestedListViews(objectTypes);

        public static DetailView ToDetailView(this View view) => (DetailView)view;
        public static IObservable<DetailView> ToDetailView<T>(this IObservable<T> source) where T : Frame
            => source.Select(frame => frame.View.ToDetailView());
        public static IObservable<Unit> WhenDisposedFrame<TFrame>(this TFrame source) where TFrame : Frame
            => source.WhenEvent(nameof(Frame.Disposed)).ToUnit();
        
        
        public static IObservable<Unit> ListViewHasObjects(this Window window) 
            => window.DashboardViewItems<ListView>().ToNowObservable()
                .WhenNotDefault(listView => listView.Objects().Any()).ToUnit();

        public static IObservable<Unit> ColumnViewHasObjects(this Window window) 
            => window.WhenGridControl().ToFirst().WhenNotDefault(control => control.MainView.RowCount).ToUnit();

        public static IObservable<(GridControl gridControl, Frame frame)> WhenGridControl(this Window window) 
            => window.DashboardViewItems(ViewType.DetailView).Where(item => !item.Frame.View.ObjectTypeInfo.IsPersistent).ToNowObservable()
                .SelectMany(item => item.InnerView.ToDetailView().WhenGridControl().Select(gridControl => (gridControl,item.Frame)));

        public static ActionBase Action(this Frame frame, string id) 
            => frame.Actions(id).FirstOrDefault();
        
        public static IEnumerable<ActionBase> Actions(this Frame frame,params string[] actionsIds) 
            => frame.Actions<ActionBase>(actionsIds);
        
        public static IEnumerable<T> Actions<T>(this Frame frame,params string[] actionsIds) where T : ActionBase 
            => frame.Controllers.Cast<Controller>().SelectMany(controller => controller.Actions).OfType<T>()
                .Where(actionBase => !actionsIds.Any()|| actionsIds.Any(s => s==actionBase.Id));
        
        public static IObservable<Frame> ChangeViewVariant(this IObservable<Frame> source,string id) 
            => source.ToController<ChangeVariantController>()
                .Do(controller => controller.ChangeVariantAction.DoExecute(
                    controller.ChangeVariantAction.Items.First(item => item.Id == id)))
                .Select(controller => controller.Frame);

        public static IObservable<Unit> HasObjects(this IObservable<Window> source) 
            => source.SelectMany(window => window.ColumnViewHasObjects().ToUnit()
                    .SwitchIfEmpty(window.ListViewHasObjects().ToUnit()));

        public static IObservable<Unit> ProcessSelectedObject(this IObservable<Window> source)
            => source.SelectMany(window => window.ProcessSelectedObject());
        public static NestedFrame ToNestedFrame(this Frame frame) => (NestedFrame)frame;
        public static IObservable<Unit> SelectListViewObject(this IObservable<Window> source) 
            => source.Publish(itemWindowSource => itemWindowSource.SelectColumnViewObject()
                .SwitchIfEmpty(itemWindowSource.SelectMany(window => window.DashboardViewItems<ListView>().ToNowObservable()
                    .SelectMany(listView => listView.SelectObject(listView.Objects().Take(1).ToArray())).ToUnit())));

        private static IObservable<Unit> SelectColumnViewObject(this IObservable<Window> itemWindowSource) 
            => itemWindowSource.SelectMany(window => window.DashboardViewItems(ViewType.DetailView).Where(item => !item.InnerView.ObjectTypeInfo.IsPersistent).ToNowObservable()
                    .SelectMany(item => item.InnerView.ToDetailView().WhenGridControl().Select(gridControl => gridControl.MainView).Cast<LayoutView>()
                        .SelectMany(gridView => gridView.ProcessEvent(EventType.Click)))).ToUnit();

        public static IObservable<Frame> OfView<TView>(this IObservable<Frame> source)
            => source.Where(item => item.View is TView);
        
        public static IObservable<View> CreateNewObject(this Window window)
            => window.ColumnViewCreateNewObject().SwitchIfEmpty(window.ListViewCreateNewObject());

        private static IObservable<View> ColumnViewCreateNewObject(this Window window,string newActionId=null)
            => window.WhenGridControl().SelectMany(t => t.frame.Actions(newActionId??"CreateNewObject").Cast<SimpleAction>())
                .SelectMany(action => action.Trigger(window.Application
                    .RootView(((NestedFrame)action.Controller.Frame).DashboardChildDetailView().ObjectTypeInfo.Type, ViewType.DetailView)));
        
        private static IObservable<View> ListViewCreateNewObject(this Window window) 
            => window.DashboardViewItems(ViewType.ListView).Select(item => item.Frame).ToNowObservable()
                .ToController<NewObjectViewController>().Select(controller => controller.NewObjectAction)
                .SelectMany(action => action.Trigger(window.Application
                    .RootView(action.Controller.Frame.View.ObjectTypeInfo.Type, ViewType.DetailView)));

        public static IObservable<Unit> ProcessSelectedObject(this Window window) 
            => window.WhenGridControl()
                .Publish(source => source.SelectMany(t => window.Application.WhenFrame(( (NestedFrame)t.frame).DashboardChildDetailView().ObjectTypeInfo.Type, ViewType.DetailView)
                        .Where(frame => frame.View.ObjectSpace.GetKeyValue(frame.View.CurrentObject)
                            .Equals(frame.View.ObjectSpace.GetKeyValue(((LayoutView)t.gridControl.MainView).FocusedRowObject)))
                        .CloseWindow())
                    .Merge(source.ToFirst().ProcessEvent(EventType.DoubleClick).To<Frame>().IgnoreElements())).ToUnit()
                .SwitchIfEmpty(window.ProcessListViewSelectedItem());

        public static DetailView DashboardChildDetailView(this NestedFrame listViewFrame) 
            => ((DashboardView)listViewFrame.ViewItem.View).Views<DetailView>().First(detailView => detailView.ObjectTypeInfo!=listViewFrame.View.ObjectTypeInfo);

        public static IObservable<Unit> ProcessListViewSelectedItem(this Window window) 
            => window.DashboardViewItems(ViewType.ListView).ToNowObservable()
                .SelectMany(item => item.Frame.ListViewProcessSelectedItem(() => item.Frame.View.ToListView().Objects().First()))
                .ToUnit();
    }
}