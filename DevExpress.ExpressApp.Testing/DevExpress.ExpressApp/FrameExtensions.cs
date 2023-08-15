using System.Reactive.Linq;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Testing.RXExtensions;
using DevExpress.ExpressApp.ViewVariantsModule;
using DevExpress.Utils.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using Unit = System.Reactive.Unit;

namespace DevExpress.ExpressApp.Testing.DevExpress.ExpressApp{
    public static class FrameExtensions{
        public static bool When<T>(this T frame, params Nesting[] nesting) where T : Frame 
            => nesting.Any(item => item == Nesting.Any || frame is NestedFrame && item == Nesting.Nested ||
                                   !(frame is NestedFrame) && item == Nesting.Root);

        public static bool When<T>(this T frame, params string[] viewIds) where T : Frame 
            => viewIds.Contains(frame.View?.Id);

        public static bool When<T>(this T frame, params ViewType[] viewTypes) where T : Frame 
            => viewTypes.Any(viewType =>viewType==ViewType.Any|| frame.View is CompositeView compositeView && compositeView.Is(viewType));

        public static bool When<T>(this T frame, params Type[] types) where T : Frame 
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
            => source.SelectMany(frame => frame.View.WhenActivated().To(frame).WaitUntilInactive(1.Seconds()).Take(1).ObserveOnContext())
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
        
        public static IEnumerable<DashboardViewItem> DashboardViewItems(this Frame frame,params Type[] objectTypes) 
            => frame.View.ToCompositeView().GetItems<DashboardViewItem>().Where(item => item.InnerView.Is(objectTypes));
        
        public static IEnumerable<DashboardViewItem> DashboardViewItems(this Frame frame,ViewType viewType,params Type[] objectTypes) 
            => frame.DashboardViewItems(objectTypes).When(viewType);
        public static IEnumerable<DashboardViewItem> DashboardViewItems(this Window frame,Type objectType,params ViewType[] viewTypes) 
            => frame.DashboardViewItems(objectType.YieldItem().ToArray()).When(viewTypes);
        public static IEnumerable<DashboardViewItem> DashboardViewItems(this Window frame,params ViewType[] viewTypes) 
            => frame.DashboardViewItems(typeof(object).YieldItem().ToArray()).When(viewTypes);

        public static IEnumerable<DashboardViewItem> When(this IEnumerable<DashboardViewItem> source, params ViewType[] viewTypes) 
            => source.Where(item => viewTypes.All(viewType => item.InnerView.Is(viewType)));

        public static IEnumerable<TViewType> DashboardViewItems<TViewType>(this Frame frame,params Type[] objectTypes) where TViewType:View
            => frame.DashboardViewItems(objectTypes).Select(item => item.Frame.View as TViewType).WhereNotDefault();
        
        public static IObservable<ListPropertyEditor> NestedListViews(this Frame frame, params Type[] objectTypes ) 
            => frame.View.ToDetailView().NestedListViews(objectTypes);

        internal static DetailView ToDetailView(this View view) => (DetailView)view;
        public static IObservable<DetailView> ToDetailView<T>(this IObservable<T> source) where T : Frame
            => source.Select(frame => frame.View.ToDetailView());
        public static IObservable<Unit> WhenDisposedFrame<TFrame>(this TFrame source) where TFrame : Frame
            => source.WhenEvent(nameof(Frame.Disposed)).ToUnit();
        
        // public static IObservable<object> WhenListViewObjects(this Frame frame) 
        //     => frame.DashboardViewItems<ListView>().ToNowObservable()
        //         .SelectMany(listView => listView.WhenObjects());

        public static IObservable<object> ColumnViewHasObjects(this Frame frame) 
            => frame.WhenGridControl().ToFirst().HasObjects();

        public static IObservable<object> GridViewDetailViewObjects(this Frame frame)
            => frame.WhenGridControl().ToFirst().Take(1).Select(control => control.MainView).OfType<GridView>()
                .SelectMany(view => view.GridDetailViewObjects())
                .BufferUntilCompleted();

        public static IObservable<(GridControl gridControl, Frame frame)> WhenGridControl(this Frame frame) 
            => (frame.View is DashboardView ? frame.DashboardViewItems(ViewType.DetailView)
                .Where(item => item.Model.ActionsToolbarVisibility != ActionsToolbarVisibility.Hide)
                .ToNowObservable().Select(item => item.Frame) : frame.Observe())
                .If(frame1 => frame1.View is DetailView,frame1 => frame1.View.ToDetailView().WhenControlViewItemGridControl()
                    .Select(gridControl => (gridControl, frame1)));

        public static T Action<T>(this Frame frame, string id) where T:ActionBase
            => frame.Actions<T>(id).FirstOrDefault();
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

        public static IObservable<(Frame frame, object o)> WhenObjects(this IObservable<Frame> source) 
            => source.SelectMany(frame => frame.WhenObjects());

        private static IObservable<(Frame frame, object o)> WhenObjects(this Frame frame) 
            => frame.ColumnViewHasObjects()
                .SwitchIfEmpty(Observable.Defer(() => frame.View.ToListView().WhenObjects()))
            .Select(obj => (frame,o: obj));

        public static IObservable<object> WhenGridControlDetailViewObjects(this IObservable<Frame> source) 
            => source.SelectMany(frame => frame.GridViewDetailViewObjects());

        public static IObservable<Unit> ProcessSelectedObject(this IObservable<Window> source)
            => source.SelectMany(window => window.ProcessSelectedObject());
        public static NestedFrame ToNestedFrame(this Frame frame) => (NestedFrame)frame;
        public static IObservable<Unit> SelectListViewObject(this IObservable<Window> source, Func<DashboardViewItem, bool> itemSelector=null) 
            => source.SelectColumnViewObject(itemSelector)
                .SwitchIfEmpty(source.SelectMany(window => window.DashboardViewItems(ViewType.ListView).ToNowObservable()
                    .Where(itemSelector??(_ =>true) ).Select(item => item.InnerView.ToListView())
                    .SelectMany(listView => listView.SelectObject(listView.Objects().Take(1).ToArray())).ToUnit()));

        private static IObservable<ColumnView> SelectColumnViewObject(this IObservable<DashboardViewItem> source)
            => source.SelectMany(item => item.InnerView.ToDetailView().WhenControlViewItemGridControl()
                .Select(gridControl => gridControl.MainView).Cast<ColumnView>()
                .SelectMany(gridView => gridView.ProcessEvent(EventType.Click)));
        
        private static IObservable<Unit> SelectColumnViewObject(this IObservable<Window> source,Func<DashboardViewItem,bool> itemSelector=null) 
            => source.SelectMany(window => window.DashboardViewItems(ViewType.DetailView).Where(itemSelector??(_ =>true) ).ToNowObservable()
                .SelectColumnViewObject()).ToUnit();

        public static IObservable<ListView> ToListView<T>(this IObservable<T> source) where T : Frame
            => source.Select(frame => frame.View.ToListView());
        
        public static IObservable<Frame> OfView<TView>(this IObservable<Frame> source)
            => source.Where(item => item.View is TView);
        
        public static IObservable<Frame> CreateNewObject(this Frame frame)
            => frame.ColumnViewCreateNewObject().SwitchIfEmpty(frame.ListViewCreateNewObject())
                .SelectMany(frame1 => frame1.View.ToDetailView().CloneRequiredMembers().ToNowObservable().IgnoreElements().To<Frame>().Concat(frame1.Observe()));

        private static IObservable<Frame> ColumnViewCreateNewObject(this Frame frame)
            => frame.WhenGridControl().Select(t => t.frame).CreateNewObject();

        public static IObservable<Frame> CreateNewObject(this IObservable<Frame> source)
            => source.ToController<NewObjectViewController>().Select(controller => controller.NewObjectAction)
                .SelectMany(action => action.Trigger(action.Application
                    .WhenRootFrame(action.Controller.Frame.View.ObjectTypeInfo.Type, ViewType.DetailView)));
        
        private static IObservable<Frame> ListViewCreateNewObject(this Frame frame) 
            => (frame.View is not DashboardView ? frame.Observe()
                : frame.DashboardViewItems(ViewType.ListView).Where(item => item.Model.ActionsToolbarVisibility != ActionsToolbarVisibility.Hide)
                    .Select(item => item.Frame).ToNowObservable()).CreateNewObject();

        public static IObservable<Unit> ProcessSelectedObject(this Frame frame) 
            => frame.WhenGridControl()
                .Publish(source => source.SelectMany(t => frame.Application.WhenFrame(( (NestedFrame)t.frame).DashboardChildDetailView().ObjectTypeInfo.Type, ViewType.DetailView)
                        .Where(frame1 => frame1.View.ObjectSpace.GetKeyValue(frame1.View.CurrentObject)
                            .Equals(frame1.View.ObjectSpace.GetKeyValue(((ColumnView)t.gridControl.MainView).FocusedRowObject)))
                        .CloseWindow())
                    .Merge(source.ToFirst().ProcessEvent(EventType.DoubleClick).To<Frame>().IgnoreElements())).ToUnit()
                .SwitchIfEmpty(frame.ProcessListViewSelectedItem());

        public static DetailView DashboardChildDetailView(this NestedFrame listViewFrame) 
            => ((DashboardView)listViewFrame.ViewItem.View).Views<DetailView>().First(detailView => detailView!=listViewFrame.View);

        public static IObservable<Unit> ProcessListViewSelectedItem(this Frame frame) 
            => frame.View is not DashboardView ? frame.WhenObjects().ToSecond().Take(1).SelectMany(o => frame.ListViewProcessSelectedItem(() => o)).ToUnit()
                : frame.DashboardViewItems(ViewType.ListView).ToNowObservable().Select(item => item.Frame)
                    .SelectMany(frame1 => frame1.View.ToListView().WhenObjects().Take(1)
                        .SelectMany(o => frame1.ListViewProcessSelectedItem(() => o))).ToUnit();

        public static IObservable<(GridControl gridControl, Frame frame)> WhenGridControl(this IObservable<Frame> source) 
            => source.OfView<DetailView>().SelectMany(frame =>
                frame.View.ToDetailView().WhenControlViewItemGridControl().Select(gridControl => (gridControl, frame)));
    }
}