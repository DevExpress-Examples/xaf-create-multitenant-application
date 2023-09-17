using System.Reactive.Concurrency;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.ViewVariantsModule;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Utils.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using XAF.Testing.RX;
using ListView = DevExpress.ExpressApp.ListView;
using Unit = System.Reactive.Unit;
using View = DevExpress.ExpressApp.View;

namespace XAF.Testing.XAF{
    public static class FrameExtensions{
        public static IObservable<T> SelectUntilViewClosed<T,TFrame>(this IObservable<TFrame> source, Func<TFrame, IObservable<T>> selector) where TFrame:Frame 
            => source.SelectMany(frame => selector(frame).TakeUntilViewClosed(frame));
        
        public static IObservable<T> SwitchUntilViewClosed<T,TFrame>(this IObservable<TFrame> source, Func<TFrame, IObservable<T>> selector) where TFrame:Frame 
            => source.Select(frame => selector(frame).TakeUntilViewClosed(frame)).Switch();
        
        public static IObservable<TFrame> TakeUntilViewClosed<TFrame>(this IObservable<TFrame> source,Frame frame)  
            => source.TakeUntil(frame.View.WhenClosing());
        public static bool When<T>(this T frame, params Nesting[] nesting) where T : Frame 
            => nesting.Any(item => item == Nesting.Any || frame is NestedFrame && item == Nesting.Nested ||
                                   !(frame is NestedFrame) && item == Nesting.Root);

        public static bool When<T>(this T frame, params string[] viewIds) where T : Frame 
            => viewIds.Contains(frame.View?.Id);

        public static bool When<T>(this T frame, params ViewType[] viewTypes) where T : Frame 
            => viewTypes.Any(viewType =>viewType==ViewType.Any|| frame.View is CompositeView compositeView && compositeView.Is(viewType));

        public static bool When<T>(this T frame, params Type[] types) where T : Frame 
            => types.Any(item => frame.View is ObjectView objectView && objectView.Is(objectType:item));
        

        public static IObservable<Frame> ListViewProcessSelectedItem<T>(this Frame frame, Func<T> selectedObject){
            var action = frame.GetController<ListViewProcessCurrentObjectController>().ProcessCurrentObjectAction;
            return action.Trigger(action.WhenExecuted()
                .SelectMany(e => frame.Application.WhenFrame(e.ShowViewParameters.CreatedView.ObjectTypeInfo.Type,ViewType.DetailView))
                .Take(1),selectedObject().YieldItem().Cast<object>().ToArray());
        }

        
        public static IObservable<IFrameContainer> NestedFrameContainers<TWindow>(this TWindow window,
            params Type[] objectTypes) where TWindow : Window
            => window.View.ToCompositeView().NestedFrameContainers(objectTypes);
        public static IObservable<T> WhenFrame<T>(this IObservable<T> source, params ViewType[] viewTypes) where T : Frame
            => source.Where(frame => frame.When(viewTypes));
        
        public static IObservable<T> ToController<T>(this IObservable<Frame> source) where T : Controller 
            => source.SelectMany(window => window.Controllers.Cast<Controller>()).OfType<T>();

        public static IObservable<Window> CloseWindow<TFrame>(this IObservable<TFrame> source) where TFrame:Frame
            => source.Cast<Window>().DelayOnContext().Do(frame => frame.Close()).DelayOnContext().IgnoreElements();
        
        public static IObservable<Unit> WhenAcceptTriggered(this IObservable<DialogController> source) 
            => source.SelectMany(controller => controller.AcceptAction.Trigger().Take(1));
        
        public static IObservable<DashboardViewItem> DashboardViewItem(this IObservable<Frame> source, Func<DashboardViewItem, bool> itemSelector) 
            => source.SelectMany(frame => frame.DashboardViewItem(itemSelector));

        public static IObservable<DashboardViewItem> DashboardViewItem(this Frame frame,Func<DashboardViewItem, bool> itemSelector=null) 
            => frame.DashboardViewItems(ViewType.DetailView).Where(item => item.MasterViewItem(itemSelector)).ToNowObservable()
                .SwitchIfEmpty(frame.DashboardViewItems(ViewType.ListView).Where(item => item.MasterViewItem(itemSelector)).ToNowObservable());

        public static IObservable<Frame> DashboardListViewEditFrame(this Frame frame) 
            => frame.DashboardViewItems(ViewType.ListView).Where(item =>item.MasterViewItem()).ToNowObservable()
                .ToFrame().ToListView().ToEditFrame();
        
        public static bool MasterViewItem(this DashboardViewItem item,Func<DashboardViewItem, bool> masterItem=null) 
            => masterItem?.Invoke(item)??item.Model.ActionsToolbarVisibility != ActionsToolbarVisibility.Hide;
        
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
            => frame.View.ToCompositeView().DashboardViewItems(objectTypes);

        public static IEnumerable<DashboardViewItem> DashboardViewItems(this Frame frame,ViewType viewType,params Type[] objectTypes) 
            => frame.DashboardViewItems(objectTypes).When(viewType);
        public static IEnumerable<DashboardViewItem> DashboardViewItems(this Frame frame,Type objectType,params ViewType[] viewTypes) 
            => frame.DashboardViewItems(objectType.YieldItem().ToArray()).When(viewTypes);
        public static IEnumerable<DashboardViewItem> DashboardViewItems(this Frame frame,params ViewType[] viewTypes) 
            => frame.DashboardViewItems(typeof(object).YieldItem().ToArray()).When(viewTypes);

        public static IEnumerable<DashboardViewItem> When(this IEnumerable<DashboardViewItem> source, params ViewType[] viewTypes) 
            => source.Where(item => viewTypes.All(viewType => item.InnerView.Is(viewType)));

        public static IEnumerable<TViewType> DashboardViewItems<TViewType>(this Frame frame,params Type[] objectTypes) where TViewType:View
            => frame.DashboardViewItems(objectTypes).ToFrame().Select(nestedFrame => nestedFrame.View as TViewType).WhereNotDefault();
        
        public static IObservable<ListPropertyEditor> NestedListViews(this Frame frame, params Type[] objectTypes ) 
            => frame.View.ToDetailView().NestedListViews(objectTypes);

        internal static DetailView ToDetailView(this View view) => (DetailView)view;

        public static IObservable<Frame> DashboardDetailViewFrame(this Frame frame) 
            => frame.DashboardViewItems(ViewType.DetailView).Where(item => !item.MasterViewItem()).ToNowObservable()
                .ToFrame();

        public static IObservable<DetailView> ToDetailView<T>(this IObservable<T> source) where T : Frame
            => source.Select(frame => frame.View.ToDetailView());
        public static IObservable<Unit> WhenDisposedFrame<TFrame>(this TFrame source) where TFrame : Frame
            => source.WhenEvent(nameof(Frame.Disposed)).ToUnit();

        public static IObservable<object> WhenColumnViewObjects(this Frame frame,int count=0) 
            => frame.WhenGridControl().ToFirst().WhenObjects(count).Take(1).Select(o => o);

        public static IObservable<object> WhenGridViewDetailViewObjects(this Frame frame)
            => frame.WhenGridControl().ToFirst().Take(1).Select(control => control.MainView).OfType<GridView>()
                .SelectMany(view => view.WhenGridDetailViewObjects())
                .BufferUntilCompleted();

        public static IObservable<(GridControl gridControl, Frame frame)> WhenGridControl(this Frame frame) 
            => (frame.View is DashboardView ? frame.DashboardViewItems(ViewType.DetailView)
                .Where(item => item.Model.ActionsToolbarVisibility != ActionsToolbarVisibility.Hide)
                .ToNowObservable().ToFrame() : frame.Observe())
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

        public static IObservable<(Frame frame, object o)> WhenObjects(this IObservable<Frame> source,int count=0) 
            => source.SelectMany(frame => frame.WhenObjects(count)).Select(t => t);

        public static IObservable<(Frame frame, object o)> WhenObjects(this Frame frame,int count=0) 
            => frame.WhenColumnViewObjects(count).Select(o => o)
                .SwitchIfEmpty(Observable.Defer(() => frame.View.Observe().SelectMany(view => view.WhenObjectViewObjects(count))))
            .Select(obj => (frame,o: obj));

        public static IObservable<object> WhenGridControlDetailViewObjects(this IObservable<Frame> source) 
            => source.SelectMany(frame => frame.WhenGridViewDetailViewObjects());

        public static IObservable<(Frame listViewFrame, Frame detailViewFrame)> ProcessSelectedObject(this IObservable<Window> source)
            => source.SelectMany(window => window.ProcessSelectedObject());
        public static NestedFrame ToNestedFrame(this Frame frame) => (NestedFrame)frame;
        public static IObservable<Unit> SelectDashboardListViewObject(this IObservable<Frame> source, Func<DashboardViewItem, bool> itemSelector=null) 
            => source.SelectDashboardColumnViewObject(itemSelector)
                .SwitchIfEmpty(Observable.Defer(() => source.SelectMany(window => window.DashboardViewItems(ViewType.ListView).ToNowObservable()
                    .Where(itemSelector??(_ =>true) ).Select(item => item.InnerView.ToListView())
                    .SelectMany(listView => listView.SelectObject()).ToUnit())));

        private static IObservable<ColumnView> SelectDashboardColumnViewObject(this IObservable<DashboardViewItem> source)
            => source.SelectMany(item => item.InnerView.ToDetailView().WhenControlViewItemGridControl()
                .Select(gridControl => gridControl.MainView).Cast<ColumnView>()
                .SelectMany(gridView => gridView.ProcessEvent(EventType.Click)));
        
        private static IObservable<Unit> SelectDashboardColumnViewObject(this IObservable<Frame> source,Func<DashboardViewItem,bool> itemSelector=null) 
            => source.SelectMany(window => window.DashboardViewItems(ViewType.DetailView).Where(itemSelector??(_ =>true) ).ToNowObservable()
                .SelectDashboardColumnViewObject()).ToUnit();

        public static IObservable<ListView> ToListView<T>(this IObservable<T> source) where T : Frame
            => source.Select(frame => frame.View.ToListView());
        
        public static IObservable<Frame> OfView<TView>(this IObservable<Frame> source)
            => source.Where(item => item.View is TView);
        
        public static IObservable<Frame> CreateNewObject(this Frame frame,bool inLine=false) 
            => !inLine ? frame.CreateNewObjectController() : frame.CreateNewObjectEditor();

        private static IObservable<Frame> CreateNewObjectEditor(this Frame frame) 
            => Observable.Defer(() => frame.View.WhenControlsCreated()
                .StartWith(frame.View.ToListView().Editor.Control).WhenNotDefault()
                .SelectMany(_ => frame.View.ToListView().WhenObjects()
                    .Do(existingObject => ((GridListEditor)frame.View.ToListView().Editor).GridView.AddNewRow(frame.View
                        .ToCompositeView().CloneExistingObjectMembers(true,existingObject).ToArray())))
                .To(frame));
[Obsolete("remove the take(1)")]
        private static IObservable<Frame> CreateNewObjectController(this Frame frame) 
            => frame.View.WhenObjectViewObjects(1).Take(1)
                .SelectMany(selectedObject => frame.ColumnViewCreateNewObject()
                    .SwitchIfEmpty(frame.ListViewCreateNewObject())
                    .SelectMany(newObjectFrame => newObjectFrame.View.ToCompositeView()
                        .CloneExistingObjectMembers(false, selectedObject)
                        .Select(_ => default(Frame)).IgnoreElements().Concat(newObjectFrame.YieldItem())));

        private static IObservable<Frame> ColumnViewCreateNewObject(this Frame frame)
            => frame.WhenGridControl().Select(t => t.frame).CreateNewObject();

        public static IObservable<Frame> CreateNewObject(this IObservable<Frame> source)
            => source.ToController<NewObjectViewController>().Select(controller => controller.NewObjectAction)
                .SelectMany(action => action.Trigger(action.Application
                    .WhenRootFrame(action.Controller.Frame.View.ObjectTypeInfo.Type, ViewType.DetailView)
                    .Merge(action.Controller.Frame.View.AsListView().Observe().WhenNotDefault().Select(view => view.EditView).WhenNotDefault()
                        .SelectMany(view => view.WhenCurrentObjectChanged().Where(detailView => detailView.IsNewObject()))
                        .To(action.Controller.Frame))
                    .Take(1)));
        
        private static IObservable<Frame> ListViewCreateNewObject(this Frame frame) 
            => (frame.View is not DashboardView ? frame.Observe()
                : frame.DashboardViewItems(ViewType.ListView).Where(item => item.Model.ActionsToolbarVisibility != ActionsToolbarVisibility.Hide)
                    .ToFrame().ToNowObservable())
                .CreateNewObject();

        public static IObservable<(Frame frame, Frame detailViewFrame)> ProcessSelectedObject(this Frame listViewFrame) 
            => listViewFrame.WhenGridControl()
                .Publish(source => source.SelectMany(t => listViewFrame.Application.WhenFrame(((NestedFrame)t.frame)
                            .DashboardChildDetailView().ObjectTypeInfo.Type, ViewType.DetailView)
                        .Where(frame1 => frame1.View.ObjectSpace.GetKeyValue(frame1.View.CurrentObject)
                            .Equals(((ColumnView)t.gridControl.MainView).FocusedRowObjectKey(frame1.View.ObjectSpace))))
                    .Merge(Observable.Defer(() => source.ToFirst().ProcessEvent(EventType.DoubleClick).To<Frame>().IgnoreElements())))
                .SwitchIfEmpty(listViewFrame.ProcessListViewSelectedItem())
                .Select(detailViewFrame => (frame: listViewFrame,detailViewFrame));

        public static DetailView DashboardChildDetailView(this NestedFrame listViewFrame) 
            => ((DashboardView)listViewFrame.ViewItem.View).Views<DetailView>().First(detailView => detailView!=listViewFrame.View);

        public static IObservable<Frame> ProcessListViewSelectedItem(this Frame frame) 
            => frame.View is not DashboardView ? frame.WhenObjects(1).ToSecond().Take(1).SelectMany(o => frame.ListViewProcessSelectedItem(() => o))
                : frame.DashboardViewItems(ViewType.ListView).ToNowObservable().ToFrame()
                    .SelectMany(frame1 => frame1.View.ToListView().WhenObjectViewObjects(1).Take(1)
                        .SelectMany(o => frame1.ListViewProcessSelectedItem(() => o)));

        public static IObservable<(GridControl gridControl, Frame frame)> WhenGridControl(this IObservable<Frame> source) 
            => source.OfView<DetailView>().SelectMany(frame =>
                frame.View.ToDetailView().WhenControlViewItemGridControl().Select(gridControl => (gridControl, frame)));
    }
}