using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.XtraGrid;
using DevExpress.XtraPdfViewer;
using XAF.Testing.RX;
using ListView = DevExpress.ExpressApp.ListView;
using TabbedGroup = DevExpress.XtraLayout.TabbedGroup;
using View = DevExpress.ExpressApp.View;

namespace XAF.Testing.XAF{
    public static class AssertExtensions{
        public static IObservable<Frame> AssertFrame(this XafApplication application, params ViewType[] viewTypes) 
            => application.AssertFrame(typeof(object),viewTypes);
        public static IObservable<Frame> AssertFrame(this XafApplication application,Type objectType, params ViewType[] viewTypes) 
            => application.WhenFrame(objectType,viewTypes).Assert($"{nameof(AssertFrame)} {string.Join(", ",viewTypes)}");
        
        public static IObservable<Frame> AssertChangeViewVariant(this IObservable<Frame> source,string variantId) 
            => source.If(_ => variantId!=null,frame => frame.Observe().ChangeViewVariant(variantId),frame => frame.Observe()).Assert($"{variantId}");
        
        public static IObservable<(Frame listViewFrame, Frame detailViewFrame)> AssertProcessSelectedObject(this IObservable<Frame> source)
            => source.SelectMany(window => window.AssertProcessSelectedObject()).TakeAndReplay(1).RefCount();
        public static IObservable<(Frame listViewFrame, Frame detailViewFrame)> AssertProcessSelectedObject(this Frame frame) 
            => frame.Observe().SelectMany(frame1 => frame1.ProcessSelectedObject().Assert($"{frame.View.Id}"));
        
        public static IObservable<Frame> AssertCreateNewObject(this Frame window,bool inLine=false)
            => window.CreateNewObject(inLine).Select(frame => (frame.View.Id,t: frame)).Assert(t => t.Id).ToSecond();


        private static IObservable<(ITypeInfo typeInfo, object keyValue, bool needsDelete, Frame source)> DeleteWhenNeeded(
                this IObservable<(ITypeInfo typeInfo, object keyValue, bool needsDelete, Frame source)> source, XafApplication application) 
            => source.If(t => t.needsDelete, t => application.CreateObjectSpace(t.typeInfo.Type)
                .Use(space => {
                    space.Delete(space.GetObjectByKey(t.typeInfo.Type, t.keyValue));
                    space.CommitChanges();
                    return t.Observe();
                }),t => t.Observe());

        public static IObservable<T> DelayOnContext<T>(this IObservable<T> source,int seconds=1) 
            => source.DelayOnContext(seconds.Seconds());
        public static IObservable<T> DelayOnContext<T>(this IObservable<T> source,TimeSpan? timeSpan) 
            => source.If(_ => timeSpan.HasValue,arg => arg.Observe().Delay((TimeSpan)timeSpan!,
                new SynchronizationContextScheduler(SynchronizationContext.Current!)),arg => arg.Observe());

        public static IObservable<T> DelayOnContext<T>(this IObservable<T> source,TimeSpan timeSpan) 
            => source.DelayOnContext((TimeSpan?)timeSpan);

        private static IObservable<(ITypeInfo typeInfo, object keyValue, bool needsDelete, Frame frame, Frame parent, bool isAggregated)> AssertSaveNewObject(
                this IObservable<(Frame frame, Frame parent, bool isAggregated)> source)
            => source.SelectMany(t => t.Observe().WhenSaveObject().DeleteWhenNeeded(t.frame.Application).Select(t1 => (t.frame.View.Id, t: t1))
                .Assert(t1 => t1.Id).Select(t1 => (t1.t.typeInfo,t1.t.keyValue,t1.t.needsDelete,t.frame,t.parent,t.isAggregated)));
        
        public static IObservable<(ITypeInfo typeInfo, object keyValue, bool needsDelete, Frame frame, Frame parent,bool isAggregated)> AssertSaveNewObject(this IObservable<Frame> source)
            => source.Select(frame => (frame,default(Frame),false)).AssertSaveNewObject();

        static IObservable<(Frame frame, Frame parent,bool isAggregated)> AssertDeleteObject(this IObservable<(Frame frame, Frame parent,bool isAggregated)> source)
            => source.WhenDeleteObject().SelectMany(t => t.application.CreateObjectSpace(t.type)
                    .Use(space => space.GetObjectByKey(t.type,t.keyValue).Observe().WhenDefault()))
                .Assert().To<(Frame frame, Frame parent,bool)>();
        
        public static IObservable<Frame> AssertDeleteObject(this IObservable<Frame> source)
            => source.Select(frame => (frame,default(Frame),false)).AssertDeleteObject().ToFirst();

        public static IObservable<Frame> AssertCreateNewObject(this IObservable<Frame> source,bool inLineEdit=false)
            => source.SelectMany(window => window.AssertCreateNewObject(inLineEdit));
        public static IObservable<(Frame frame, Frame parent,bool isAggregated)> AssertCreateNewObject(this IObservable<(Frame frame, Frame parent,bool isAggregated)> source,bool inLineEdit=false)
            => source.SelectMany(t => t.frame.AssertCreateNewObject(inLineEdit).Select(frame => (frame,t.parent,t.isAggregated)));
        
        public static IObservable<Frame> AssertExistingObjectDetailView(this XafApplication application,Func<Frame,IObservable<Unit>> assertDetailview,Type objectType=null)
            => application.WhenExistingObjectRootDetailViewFrame(objectType)
                .Assert(viewId => $"{nameof(AssertExistingObjectDetailView)} {objectType?.Name} {viewId}")
                .ConcatIgnored(assertDetailview);
        public static IObservable<SingleChoiceAction> AssertSingleChoiceAction<TItemDataType>(this IObservable<Frame> source,string actionId,int itemsCount) 
            => source.Select(frame => frame.Action<SingleChoiceAction>(actionId)).Assert($"{nameof(AssertSingleChoiceAction)} {actionId}")
                .SelectMany(choiceAction => choiceAction.Items<TItemDataType>().Skip(itemsCount - 1).ToNowObservable().To(choiceAction))
                .Assert($"{nameof(AssertSingleChoiceAction)} {actionId} {itemsCount}");
        
        public static IObservable<Frame> AssertExistingObjectDetailView(this XafApplication application,Type objectType=null) 
            => application.AssertExistingObjectDetailView(_ => Observable.Empty<Unit>(),objectType);

        public static IObservable<DashboardViewItem> AssertDashboardViewItems(this Frame frame,ViewType viewType,params Type[] objectTypes) 
            => frame.AssertDashboardViewItems(viewType,_ => true,objectTypes);
        public static IObservable<DashboardViewItem> AssertDashboardViewItems(this Frame frame,ViewType viewType,Func<DashboardViewItem,bool> match,params Type[] objectTypes) 
            => frame.DashboardViewItems(viewType,objectTypes).Where(match).ToNowObservable()
                .Assert(item => $"{viewType} {item?.View} {item?.InnerView}");

        public static IObservable<Frame> AssertListView(this XafApplication application,string navigation, string viewVariant=null,Func<Frame, IObservable<Unit>> assertExistingObjectDetailview = null) 
            => application.AssertNavigation(navigation)
                .AssertChangeViewVariant(viewVariant)
                .AssertListView(assertExistingObjectDetailview);

        public static IObservable<Frame> AssertListView(this DashboardViewItem[] items, Type objectType,
            Func<Frame, IObservable<Unit>> assertExistingObjectDetailview = null, AssertAction assert = AssertAction.All) 
            => items.ToNowObservable().SelectMany(item => item.InnerView.ToDetailView().NestedListViews(objectType))
                .Select(editor => editor.Frame).ReplayFirstTake()
                .AssertListView(assertExistingObjectDetailview, assert);

        public static IObservable<Frame> AssertListView(this Frame frame, Func<Frame, IObservable<Unit>> assertExistingObjectDetailview = null, AssertAction assert = AssertAction.All) 
            => frame.Observe().AssertListView(assertExistingObjectDetailview, assert);

        public static IObservable<Frame> AssertItemsAdded(this IObservable<SingleChoiceAction> source, IObservable<object> when)
            => source.AssertItemsAdded().Merge(when.IgnoreElements().To<Frame>()).Assert();
        
        public static IObservable<Frame> AssertItemsAdded(this IObservable<SingleChoiceAction> source) 
            => source.SelectMany(action => action.WhenItemsChanged().Where(e => e.ChangedItemsInfo.Any(pair => pair.Value==ChoiceActionItemChangesType.ItemsAdd)).To(action.Frame()));

        public static IObservable<Frame> AssertListView(this IObservable<Frame> source, Func<Frame, IObservable<Unit>> assertExistingObjectDetailview = null, AssertAction assert = AssertAction.All,bool inlineEdit=false,[CallerMemberName]string caller="") 
            => source.Select(frame => (frame,default(Frame),false)).AssertListView(assertExistingObjectDetailview,assert,inlineEdit,caller);

        private static IObservable<Frame> AssertListView(this IObservable<(Frame frame, Frame parent,bool aggregated)> source,
            Func<Frame, IObservable<Unit>> assertExistingObjectDetailview = null, AssertAction assert = AssertAction.All, bool inlineEdit = false,[CallerMemberName]string caller="") 
            => source.AssertListViewHasObjects( assert)
                .AssertProcessSelectedObject(assertExistingObjectDetailview, assert)
                .AssertCreateSaveAndDeleteObject( source,assert, inlineEdit)
                .AssertGridControlDetailViewObjects( assert, caller,source)
                .IgnoreElements().To<Frame>()
                .Concat(source.ToFirst())
                .TakeAndReplay(1).AutoConnect();

        private static IObservable<Frame> AssertGridControlDetailViewObjects(this  IObservable<Unit> createSaveAndDeleteObject,AssertAction assert, string caller,
            IObservable<(Frame frame, Frame parent, bool aggregated)> source) 
            => source.ToFirst().ConcatIgnored(_ => createSaveAndDeleteObject).If(_ => assert.HasFlag(AssertAction.GridControlDetailView),frame => frame.Observe()
                    .AssertGridControlDetailViewObjects(caller:caller).To<Frame>());

        private static IObservable<(Frame frame, object o)> AssertListViewHasObjects(this IObservable<(Frame frame, Frame parent, bool aggregated)> source, AssertAction assert) 
            => source.ToFirst().If(_ => assert.HasFlag(AssertAction.HasObject), frame => frame.Observe().AssertListViewHasObjects()).TakeAndReplay(1).AutoConnect();

        private static IObservable<Window> AssertProcessSelectedObject(this IObservable<(Frame frame, object o)> listViewHasObjects,
            Func<Frame, IObservable<Unit>> assertExistingObjectDetailview, AssertAction assert) 
            => listViewHasObjects.ToFirst().If(_ => assert.HasFlag(AssertAction.Process),
                    frame => frame.AssertProcessSelectedObject().ToSecond()
                        .ConcatIgnored(assertExistingObjectDetailview??(_ =>Observable.Empty<Unit>()) ))
                .DelayOnContext().CloseWindow()
                .ReplayFirstTake().Select(frame1 => frame1);

        private static IObservable<Unit> AssertCreateSaveAndDeleteObject(this IObservable<Window> processSelectedObject,
            IObservable<(Frame frame, Frame parent, bool aggregated)> source, AssertAction assert, bool inlineEdit) 
            => source.ConcatIgnored(_ => processSelectedObject).AssertCreateNewObject(assert, inlineEdit).AssertSaveNewObject(assert).AssertDeleteObject(assert);

        private static IObservable<Unit> AssertDeleteObject(this IObservable<(Frame frame, Frame parent, bool isAggregated)> source,AssertAction assert) 
            => source.If(_ => assert.HasFlag(AssertAction.Delete),t => t.Observe().AssertDeleteObject().ToUnit(),t => t.frame.Observe().DelayOnContext().CloseWindow().ToUnit());

        private static IObservable<(Frame frame, Frame parent, bool isAggregated)> AssertSaveNewObject(this IObservable<(Frame frame, Frame parent, bool isAggregated)> source, AssertAction assert) 
            => source.If(_ => assert.HasFlag(AssertAction.Save),t => t.Observe().AssertSaveNewObject().Select(t1 => (t1.frame,t1.parent,t1.isAggregated)));

        private static IObservable<(Frame frame, Frame parent, bool isAggregated)> AssertCreateNewObject(this IObservable<(Frame frame, Frame parent, bool aggregated)> source, 
            AssertAction assert, bool inlineEdit) 
            => source.If(_ => assert.HasFlag(AssertAction.New),t => t.Observe().AssertCreateNewObject(inlineEdit));
        
        public static IObservable<Frame> AssertDashboardMasterDetail(this XafApplication application, string navigationView, string viewVariant, 
            Func<Frame, IObservable<Frame>> detailViewFrameSelector = null, Func<DashboardViewItem, bool> listViewFrameSelector = null,
            Func<Frame, IObservable<Unit>> existingObjectDetailview = null,AssertAction assert=AssertAction.All) 
            => application.AssertNavigation(navigationView)
                .AssertDashboardMasterDetail(viewVariant, detailViewFrameSelector, listViewFrameSelector, existingObjectDetailview,assert);

        public static IObservable<Frame> AssertDashboardMasterDetail(this IObservable<Window> source,string viewVariant=null, Func<Frame, IObservable<Frame>> detailViewFrameSelector=null,
            Func<DashboardViewItem, bool> listViewFrameSelector=null, Func<Frame, IObservable<Unit>> existingObjectDetailview=null,AssertAction assert = AssertAction.All) 
            => source.AssertChangeViewVariant(viewVariant)
                .AssertDashboardDetailView(detailViewFrameSelector, listViewFrameSelector).IgnoreElements()
                .Concat(source.AssertDashboardListView(listViewFrameSelector, existingObjectDetailview,assert)).IgnoreElements()
                .Concat(source)
                .ReplayFirstTake();

        public static IObservable<(Frame frame, Frame source)> AssertDialogControllerListView(this IObservable<SingleChoiceAction> action,Type objectType,AssertAction assert=AssertAction.All,bool inlineEdit=false) 
            => action.SelectMany(choiceAction => choiceAction.Trigger(choiceAction.Application.AssertFrame(objectType,ViewType.ListView)
                    .Select(frame => (frame, source: choiceAction.Controller.Frame)), choiceAction.Items.First))
                .SelectMany(t => t.frame.Observe().AssertListView(assert:assert,inlineEdit:inlineEdit).To(t));

        private static IObservable<Frame> AssertDashboardListView(this IObservable<Frame> changeViewVariant, Func<DashboardViewItem, bool> listViewFrameSelector,
            Func<Frame, IObservable<Unit>> assertExistingObjectDetailview, AssertAction assert = AssertAction.All, [CallerMemberName] string caller = "") 
            => changeViewVariant.AssertMasterFrame(listViewFrameSelector).ToFrame()
                .AssertListView(assertExistingObjectDetailview, assert,caller:caller)
                .ReplayFirstTake();

        public static IObservable<Frame> AssertDashboardDetailView(this XafApplication application,string navigationId,string viewVariant) 
            => application.AssertNavigation(navigationId).AssertChangeViewVariant(viewVariant).AssertDashboardDetailView(null);

        private static IObservable<Frame> AssertDashboardDetailView(this IObservable<Frame> source, Func<Frame, IObservable<Frame>> detailViewFrameSelector=null,
            Func<DashboardViewItem, bool> listViewFrameSelector=null){
            var detailViewFrame = source.SelectMany(detailViewFrameSelector??(frame =>frame.DashboardDetailViewFrame()) ).TakeAndReplay(1).AutoConnect();
            return detailViewFrame.IsEmpty()
                .If(isEmpty => isEmpty, _ => source, _ => {
                    
                    var selectListViewObject = source.Cast<Window>().AssertSelectListViewObject(listViewFrameSelector ?? (item => item.MasterViewItem()));
                    var detailViewDisplaysData = selectListViewObject.SelectMany(_ => detailViewFrame)
                        .AssertDetailViewHasObject();
        
                    return detailViewDisplaysData
                        .IgnoreElements().To<Frame>()
                        .Concat(source);
                });
        }
        private static IObservable<Frame> AssertDashboardDetailView(this IObservable<Frame> source,Func<DashboardViewItem,bool> masterItem){
            var detailViewItem = source.SelectMany(frame => frame.AssertDashboardViewItems(ViewType.DetailView, item => !item.MasterViewItem(masterItem)))
                .TakeAndReplay(1).AutoConnect();
            var detailViewDoesNotDisplayData = detailViewItem.AssertDetailViewNotHaveObject();
            var selectListViewObject = source.Cast<Window>().ConcatIgnored(_ => detailViewDoesNotDisplayData)
                .AssertSelectListViewObject(item => item.MasterViewItem(masterItem));
            var detailViewDisplaysData = selectListViewObject.SelectMany(_ => detailViewItem).AssertDetailViewHasObject();
            return detailViewDisplaysData
                .IgnoreElements().To<Frame>()
                .Concat(source);
        }

        public static IObservable<object> AssertObjectsCount(this View view, int objectsCount) 
            => view.WhenDataSourceChanged().FirstAsync(o => o is CollectionSourceBase collectionSourceBase
                    ? collectionSourceBase.GetCount() == objectsCount
                    : ((GridControl)o).MainView.DataRowCount == objectsCount)
                .Assert($"{nameof(AssertObjectsCount)} {view.Id}");

        public static IObservable<object> WhenDataSourceChanged(this View view) 
            => view is ListView listView ? listView.CollectionSource.WhenCriteriaApplied()
                : view.ToDetailView().WhenControlViewItemGridControl().SelectMany(control => control.WhenDataSourceChanged().To(control));

        private static IObservable<DashboardViewItem> AssertMasterFrame(this IObservable<Frame> source,Func<DashboardViewItem, bool> masterItem=null) 
            => source.DashboardViewItem( masterItem).Assert(item => $"{item?.Id}");

        public static IObservable<Window> AssertNavigation(this XafApplication application, string viewId)
            => application.Navigate(viewId).Assert($"{viewId}");

        public static IObservable<Unit> AssertSelectListViewObject(this IObservable<Window> source, Func<DashboardViewItem, bool> itemSelector = null)
            => source.SelectListViewObject(itemSelector).Assert();

        public static IObservable<TTabbedControl> AssertTabControl<TTabbedControl>(this XafApplication application,Type objectType=null) 
            => application.WhenDetailViewCreated(objectType).ToDetailView()
                .SelectMany(detailView => detailView.WhenTabControl()).Cast<TTabbedControl>()
                .Assert(objectType?.Name);

        public static IObservable<object> AssertWindowHasObjects(this IObservable<Frame> source)
            => source.If(window => window.DashboardViewItems<DetailView>().Any(),window => window.Observe().WhenObjects().ToSecond()
                    .Assert($"{nameof(AssertWindowHasObjects)} {window.View.Id}"),
                window => window.DashboardViewItems<ListView>().ToNowObservable().BufferUntilCompleted()
                    .SelectMany(listViews => listViews.ToNowObservable().SelectMany(listView => listView.WhenObjects().Take(1))
                        .Take(listViews.Length))
                    .Assert($"{nameof(AssertWindowHasObjects)} {window.View.Id}"));
        
        public static IObservable<object> AssertGridControlDetailViewObjects(this IObservable<Frame> source,[CallerMemberName]string caller="")
            => source.WhenGridControlDetailViewObjects().Assert(o => $"{o} {caller}");

        public static IObservable<DetailView> AssertDetailViewNotHaveObject(this IObservable<DashboardViewItem> source)
            =>  source.AsView<DetailView>().WhenDefault(detailView => detailView.CurrentObject)
                .Assert(view => $"{view}");
        public static IObservable<DetailView> AssertDetailViewNotHaveObject(this IObservable<Frame> source)
            =>  source.ToDetailView().WhenDefault(detailView => detailView.CurrentObject)
                .Assert(view => $"{view}");

        public static IObservable<Unit> AssertDetailViewGridControlHasObjects(this IObservable<DashboardViewItem> source)
            => source.SelectMany(item => item.InnerView.ToDetailView().WhenControlViewItemGridControl().HasObjects()).ToUnit().Assert();
        public static IObservable<Unit> AssertDetailViewPdfViewerHasPages(this IObservable<DashboardViewItem> source)
            => source.SelectMany(item => item.InnerView.ToDetailView().WhenViewItemWinControl<PropertyEditor>(typeof(PdfViewer)))
                .SelectMany(t => t.item.WhenEvent(nameof(PropertyEditor.ValueRead))
                    .WhenNotDefault(_ => ((PdfViewer)t.control).PageCount))
                .ToUnit().Assert();
        
        public static IObservable<Unit> AssertDetailViewHasObject(this IObservable<DashboardViewItem> source) 
            => source.AsView<DetailView>()
                .SelectMany(detailView => detailView.WhenSelectionChanged()
                    .Merge(detailView.WhenCurrentObjectChanged()).To(detailView).StartWith(detailView)
                    .WhenNotDefault(view => view.CurrentObject)
                    .Assert(_ => $"{detailView.Id}").ToUnit())
                ;
        
        public static IObservable<Unit> AssertDetailViewHasObject(this IObservable<Frame> source) 
            => source.ToDetailView()
                .SelectMany(detailView => detailView.WhenSelectionChanged()
                    .Merge(detailView.WhenCurrentObjectChanged()).To(detailView).StartWith(detailView)
                    .WhenNotDefault(view => view.CurrentObject)
                    .Assert(_ => $"{detailView.Id}").ToUnit())
                ;

        public static IObservable<(Frame frame, object o)> AssertListViewHasObjects(this IObservable<Frame> source)
            => source.AssertObjectViewHasObjects();
            
        public static IObservable<(Frame frame, object o)> AssertObjectViewHasObjects(this IObservable<Frame> source,[CallerMemberName]string caller="")
            => source.WhenObjects().Take(1).Select(t => (msg:$"{t.frame.View.Id} {t.o}", t)).Assert(t => t.msg,caller:caller).ToSecond();

        public static IObservable<Unit> AssertNestedListView(this IObservable<TabbedGroup> source, Frame frame, Type objectType, int selectedTabPageIndex,
            Func<Frame, IObservable<Unit>> existingObjectDetailview = null, AssertAction assert = AssertAction.All,bool inlineEdit=false,[CallerMemberName]string caller="")
            => source.AssertNestedListView(frame, objectType, group => group.SelectedTabPageIndex=selectedTabPageIndex,existingObjectDetailview,assert,inlineEdit,caller);
        
        public static IObservable<Unit> AssertNestedListView(this IObservable<TabbedGroup> source, Frame frame, Type objectType, Action<TabbedGroup> tabGroupAction,
            Func<Frame, IObservable<Unit>> existingObjectDetailview = null, AssertAction assert = AssertAction.All,bool inlineEdit=false,[CallerMemberName]string caller="") 
            => frame.AssertNestedListView(objectType,existingObjectDetailview,assert,inlineEdit,caller)
                .MergeToUnit(source.Do(tabGroupAction).IgnoreElements());

        public static IObservable<Frame> AssertNestedListView(this Frame frame, Type objectType,
            Func<Frame, IObservable<Unit>> existingObjectDetailview = null, AssertAction assert = AssertAction.All,bool inlineEdit=false, [CallerMemberName] string caller = "") 
            => frame.NestedListViews(objectType).Select(editor => editor)
                .Assert($"{nameof(AssertNestedListView)} {objectType.Name}",caller:caller)
                .Select(editor => (editor.Frame,frame,editor.MemberInfo.IsAggregated))
                .AssertListView(existingObjectDetailview, assert,inlineEdit,caller:caller);


        public static IObservable<(Frame frame, object o)> AssertListViewHasObjects(this XafApplication application,Type objectType) 
            => application.WhenFrame(objectType,ViewType.ListView).AssertListViewHasObjects()
                .Assert($"{nameof(AssertListViewHasObjects)} {objectType.Name}");

    }
    
    [Flags]
    public enum AssertAction{
        [Obsolete]
        None=0,
        Delete = 1 << 0,
        New = 1 << 1,
        Save = 1 << 2,
        Process = 1 << 3,
        GridControlDetailView  = 1 << 4,
        HasObject=1<<5,
        [Obsolete]
        AllButDelete=All^Delete,
        [Obsolete("Not implemented")]
        Refresh=1<<6,
        [Obsolete("Not implemented")]
        ResetLayout=1<<7,
        [Obsolete("Not implemented")]
        Filter=1<<8,
        All = Delete | New | Save | Process|GridControlDetailView|HasObject
        
    }
}