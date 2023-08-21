using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.XtraGrid;
using DevExpress.XtraPdfViewer;
using XAF.Testing.RX;
using ListView = DevExpress.ExpressApp.ListView;
using View = DevExpress.ExpressApp.View;

namespace XAF.Testing.XAF{
    public static class AssertExtensions{
        public static IObservable<Frame> AssertFrame(this XafApplication application, params ViewType[] viewTypes) 
            => application.WhenFrame(viewTypes).Assert($"{nameof(AssertFrame)} {string.Join(", ",viewTypes)}");
        
        public static IObservable<Frame> AssertChangeViewVariant(this IObservable<Frame> source,string variantId) 
            => source.If(_ => variantId!=null,frame => frame.Observe().ChangeViewVariant(variantId),frame => frame.Observe()).Assert($"{variantId}");
        
        public static IObservable<(Frame listViewFrame, Frame detailViewFrame)> AssertProcessSelectedObject(this IObservable<Frame> source)
            => source.SelectMany(window => window.AssertProcessSelectedObject()).TakeAndReplay(1).RefCount();
        public static IObservable<(Frame listViewFrame, Frame detailViewFrame)> AssertProcessSelectedObject(this Frame frame) 
            => frame.ProcessSelectedObject().Assert($"{frame.View.Id}");
        
        public static IObservable<Frame> AssertCreateNewObject(this Frame window)
            => window.CreateNewObject().Select(frame => (frame.View.Id,t: frame)).Assert(t => t.Id).ToSecond();

        public static IObservable<(Frame frame, Frame source)> AssertSaveNewObject(this IObservable<(Frame frame, Frame source)> source)
            => source.WhenSaveObject().Select(t => (t.frame.View.Id,t)).Assert(t => t.Id).ToSecond();
        
        public static IObservable<Frame> AssertSaveNewObject(this IObservable<Frame> source)
            => source.WhenSaveObject().Select(frame => (frame.View.Id,frame)).Assert(t => t.Id).ToSecond();
        
        public static IObservable<Frame> AssertDeleteObject(this IObservable<Frame> source)
            => source.WhenDeleteObject().SelectMany(t => t.application.CreateObjectSpace()
                .Use(space => space.GetObjectByKey(t.type,t.keyValue).Observe().Select(o => o).WhenDefault()))
                .Assert().To<Frame>();

        public static IObservable<Frame> AssertCreateNewObject(this IObservable<Frame> source)
            => source.SelectMany(window => window.AssertCreateNewObject());
        public static IObservable<(Frame frame, Frame source)> AssertCreateNewObject(this IObservable<(Frame frame,Frame source)> source)
            => source.SelectMany(t => t.frame.AssertCreateNewObject().Select(frame => (frame,t.source)));
        
        public static IObservable<Frame> AssertExistingObjectDetailView(this XafApplication application,Func<Frame,IObservable<Unit>> assertDetailview,Type objectType=null)
            => application.WhenExistingObjectRootDetailViewFrame(objectType)
                .Assert(viewId => $"{nameof(AssertExistingObjectDetailView)} {objectType?.Name} {viewId}")
                .ConcatIgnored(assertDetailview);
        
        public static IObservable<Frame> AssertExistingObjectDetailView(this XafApplication application,Type objectType=null) 
            => application.AssertExistingObjectDetailView(_ => Observable.Empty<Unit>(),objectType);

        public static IObservable<DashboardViewItem> AssertDashboardViewItems(this Frame frame,ViewType viewType,params Type[] objectTypes) 
            => frame.AssertDashboardViewItems(viewType,_ => true,objectTypes);
        public static IObservable<DashboardViewItem> AssertDashboardViewItems(this Frame frame,ViewType viewType,Func<DashboardViewItem,bool> match,params Type[] objectTypes) 
            => frame.DashboardViewItems(viewType,objectTypes).Where(match).ToNowObservable()
                .Assert(item => $"{viewType} {item?.View} {item?.InnerView}");

        public static IObservable<Frame> AssertListView(this XafApplication application,string navigation, string viewVariant=null) 
            => application.AssertNavigation(navigation)
                .AssertChangeViewVariant(viewVariant)
                .AssertListView()
                .Assert($"{nameof(AssertListView)} {navigation} {viewVariant}");

        public static IObservable<Frame> AssertListView(this DashboardViewItem[] items, Type objectType,
            Func<Frame, IObservable<Unit>> assertExistingObjectDetailview = null, Assert assert = Assert.All) 
            => items.ToNowObservable().SelectMany(item => item.InnerView.ToDetailView().NestedListViews(objectType))
                .Select(editor => editor.Frame).TakeAndReplay(1).RefCount().AssertListView(assertExistingObjectDetailview, assert);

        public static IObservable<Frame> AssertListView(this IObservable<Frame> source, Func<Frame, IObservable<Unit>> assertExistingObjectDetailview = null, Assert assert = Assert.All){
            var listViewHasObjects = source.AssertListViewHasObjects().TakeAndReplay(1).AutoConnect();
            var processSelectedObject = listViewHasObjects.ToFirst().If(_ => assert.HasFlag(Assert.Process),AssertProcessSelectedObject);
            var existingObjectRootDetailView = processSelectedObject.AssertExistingObjectRootDetailView(assertExistingObjectDetailview);
            var newSaveDeleteObject = source.ConcatIgnored(existingObjectRootDetailView)
                .If(assert.HasFlag(Assert.New),AssertCreateNewObject)
                .If(assert.HasFlag(Assert.Save),AssertSaveNewObject)
                .If(assert.HasFlag(Assert.Delete),AssertDeleteObject);
            var gridControlDetailViewObjects = source.ConcatIgnored(newSaveDeleteObject)
                .If(assert.HasFlag(Assert.GridControlDetailView),obs => obs.AssertGridControlDetailViewObjects().To<Frame>());

            return source
                .Merge(listViewHasObjects.ToSecond())
                .MergeToUnit(processSelectedObject)
                .MergeToUnit(existingObjectRootDetailView)
                .ConcatToUnit(newSaveDeleteObject)
                .ConcatToUnit(gridControlDetailViewObjects)
                .IgnoreElements().To<Frame>()
                .Concat(source).TakeAndReplay(1).RefCount();
        }

        private static IObservable<Window> AssertExistingObjectRootDetailView(this IObservable<(Frame listViewFrame, Frame detailViewFrame)> source,Func<Window,IObservable<Unit>> assert) 
            => source.Where(t => !t.detailViewFrame.View.ToDetailView().IsNewObject())
                .If(t => t.listViewFrame.View is ListView,t => t.listViewFrame.Action<SimpleAction>(ListViewProcessCurrentObjectController.ListViewShowObjectActionId)
                        .WhenExecuteCompleted().To(t.detailViewFrame).Cast<Window>()
                        .ConcatIgnored(window => assert?.Invoke(window)??Observable.Empty<Unit>())
                        .Assert(window => $"{window?.View}")
                        .Do(frame => frame.Close()), t => t.detailViewFrame.Observe().Assert(window => $"{window?.View}")).Cast<Window>()
                .TakeAndReplay(1).RefCount();
        
        public static IObservable<Frame> AssertDashboardMasterDetail(this XafApplication application, string navigationView,
            string viewVariant, Func<Frame, IObservable<Frame>> detailViewFrameSelector=null, Func<DashboardViewItem, bool> listViewFrameSelector=null,Func<Frame, IObservable<Unit>> assertExistingObjectDetailview=null) 
            => application.Defer(() => {
                var changeViewVariant = application.AssertNavigation(navigationView).AssertChangeViewVariant(viewVariant);
                return changeViewVariant
                    .If(true,source => source.AssertDashboardDetailView(detailViewFrameSelector, listViewFrameSelector).IgnoreElements())
                    .Concat(changeViewVariant.AssertDashboardListView(listViewFrameSelector,assertExistingObjectDetailview)).IgnoreElements()
                    .Concat(changeViewVariant)
                    .TakeAndReplay(1).RefCount();
            });

        public static IObservable<Frame> AssertDashboardListView(this XafApplication application, string navigationView, string viewVariant,
            Func<Frame, IObservable<Frame>> detailViewFrameSelector=null, Func<DashboardViewItem, bool> listViewFrameSelector=null,
            Func<Frame, IObservable<Unit>> assertExistingObjectDetailview = null) 
            => application.AssertDashboardMasterDetail( navigationView, viewVariant,detailViewFrameSelector, listViewFrameSelector, assertExistingObjectDetailview);
        
        private static IObservable<Frame> AssertDashboardListView(this IObservable<Frame> changeViewVariant,Func<DashboardViewItem,bool> listViewFrameSelector,Func<Frame, IObservable<Unit>> assertExistingObjectDetailview,Assert assert=Assert.All) 
            => changeViewVariant.AssertMasterFrame(listViewFrameSelector).ToFrame()
                .AssertListView(assertExistingObjectDetailview, assert)
                .TakeAndReplay(1).RefCount();

        public static IObservable<Frame> AssertDashboardDetailView(this XafApplication application,string navigationId,string viewVariant) 
            => application.AssertNavigation(navigationId).AssertChangeViewVariant(viewVariant).AssertDashboardDetailView(null);

        private static IObservable<Frame> AssertDashboardDetailView(this IObservable<Frame> source, Func<Frame, IObservable<Frame>> detailViewFrameSelector=null,
            Func<DashboardViewItem, bool> listViewFrameSelector=null){
            var detailViewFrame = source.SelectMany(detailViewFrameSelector??(frame =>frame.DashboardDetailViewFrame()) ).TakeAndReplay(1).AutoConnect();
            var detailViewDoesNotDisplayData = detailViewFrame.AssertDetailViewNotHaveObject();
            var selectListViewObject = source.Cast<Window>().ConcatIgnored(detailViewDoesNotDisplayData)
                .AssertSelectListViewObject(listViewFrameSelector??(item =>item.MasterViewItem()) );
            var detailViewDisplaysData = selectListViewObject.SelectMany(_ => detailViewFrame).AssertDetailViewHasObject();
            
            return detailViewDisplaysData
                .IgnoreElements().To<Frame>()
                .Concat(source);
        }
        private static IObservable<Frame> AssertDashboardDetailView(this IObservable<Frame> source,Func<DashboardViewItem,bool> masterItem){
            var detailViewItem = source.SelectMany(frame => frame.AssertDashboardViewItems(ViewType.DetailView, item => !item.MasterViewItem(masterItem)))
                .TakeAndReplay(1).AutoConnect();
            var detailViewDoesNotDisplayData = detailViewItem.AssertDetailViewNotHaveObject();
            var selectListViewObject = source.Cast<Window>().ConcatIgnored(detailViewDoesNotDisplayData)
                .AssertSelectListViewObject(item => item.MasterViewItem(masterItem));
            var detailViewDisplaysData = selectListViewObject.SelectMany(_ => detailViewItem).AssertDetailViewHasObject();
            return detailViewDisplaysData
                .IgnoreElements().To<Frame>()
                .Concat(source);
        }

        public static IObservable<object> AssertObjectsCount(this View view, int objectsCount) 
            => view.WhenDataSourceChanged().FirstAsync(o => o is CollectionSourceBase collectionSourceBase
                ? collectionSourceBase.GetCount() == objectsCount : ((GridControl)o).MainView.DataRowCount == objectsCount)
                .Assert($"{nameof(AssertObjectsCount)} {view.Id}");

        public static IObservable<object> WhenDataSourceChanged(this View view) 
            => view is ListView listView ? listView.CollectionSource.WhenCriteriaApplied()
                : view.ToDetailView().WhenControlViewItemGridControl().SelectMany(control => control.WhenDataSourceChanged().To(control));

        private static IObservable<DashboardViewItem> AssertMasterFrame(this IObservable<Frame> source,Func<DashboardViewItem, bool> masterItem=null) 
            => source.MasterDashboardViewItem( masterItem).Assert(item => $"{item?.Id}");

        public static IObservable<Window> AssertNavigation(this XafApplication application, string viewId)
            => application.Navigate(viewId).Assert($"{viewId}");

        public static IObservable<Unit> AssertSelectListViewObject(this IObservable<Window> source, Func<DashboardViewItem, bool> itemSelector = null)
            => source.SelectListViewObject(itemSelector).Assert();

        public static IObservable<TTabbedControl> AssertTabControl<TTabbedControl>(this XafApplication application) 
            => application.WhenDetailViewCreated().ToDetailView()
                .SelectMany(detailView => detailView.WhenTabControl()).Cast<TTabbedControl>()
                .Assert();

        public static IObservable<TTabbedControl> AssertTabControl<TTabbedControl>(this IObservable<DashboardViewItem> source) 
            => source.WhenTabControl<TTabbedControl>().Assert();

        public static IObservable<object> AssertWindowHasObjects(this IObservable<Frame> source)
            => source.If(window => window.DashboardViewItems<DetailView>().Any(),window => window.Observe().WhenObjects().ToSecond()
                    .Assert($"{nameof(AssertWindowHasObjects)} {window.View.Id}"),
                window => window.DashboardViewItems<ListView>().ToNowObservable().BufferUntilCompleted()
                    .SelectMany(listViews => listViews.ToNowObservable().SelectMany(listView => listView.WhenObjects().Take(1))
                        .Take(listViews.Length))
                    .Assert($"{nameof(AssertWindowHasObjects)} {window.View.Id}"));
        
        public static IObservable<object> AssertGridControlDetailViewObjects(this IObservable<Frame> source)
            => source.WhenGridControlDetailViewObjects().Assert();

        public static IObservable<DetailView> AssertDetailViewNotHaveObject(this IObservable<DashboardViewItem> source)
            =>  source.AsView<DetailView>().WhenDefault(detailView => detailView.CurrentObject)
                .Assert(view => $"{view}");
        public static IObservable<DetailView> AssertDetailViewNotHaveObject(this IObservable<Frame> source)
            =>  source.Select(frame => frame).ToDetailView()
                .WhenDefault(detailView => detailView.CurrentObject)
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
                    .Merge(detailView.WhenCurrentObjectChanged()).To(detailView).StartWith(detailView))
                .WhenNotDefault(detailView => detailView.CurrentObject)
                .Assert(view => $"{view}").ToUnit();
        
        public static IObservable<Unit> AssertDetailViewHasObject(this IObservable<Frame> source) 
            => source.ToDetailView()
                .SelectMany(detailView => detailView.WhenSelectionChanged()
                    .Merge(detailView.WhenCurrentObjectChanged()).To(detailView).StartWith(detailView))
                .WhenNotDefault(detailView => detailView.CurrentObject)
                .Assert(view => $"{view}").ToUnit();

        public static IObservable<(Frame frame, object o)> AssertListViewHasObjects(this IObservable<Frame> source)
            => source.AssertObjectViewHasObjects();
            
        public static IObservable<(Frame frame, object o)> AssertObjectViewHasObjects(this IObservable<Frame> source,[CallerMemberName]string caller="")
            => source.WhenObjects().Take(1).Select(t => (msg:$"{t.frame.View.Id} {t.o}", t)).Assert(t => t.msg,caller:caller).ToSecond();

        public static IObservable<Frame> AssertNestedListView(this Frame frame,Type objectType,Func<Frame, IObservable<Unit>> assertExistingObjectDetailview = null,Assert  assert=Assert.All) 
            => frame.NestedListViews(objectType).Select(editor => editor)
                .Assert($"{nameof(AssertNestedListView)} {objectType.Name}")
                .Select(editor => editor.Frame).AssertListView(assertExistingObjectDetailview,assert);
        

        public static IObservable<(Frame frame, object o)> AssertListViewHasObjects(this XafApplication application,Type objectType) 
            => application.WhenFrame(objectType,ViewType.ListView).AssertListViewHasObjects()
                .Assert($"{nameof(AssertListViewHasObjects)} {objectType.Name}");

    }
    
    [Flags]
    public enum Assert{
        Delete = 1 << 0,
        New = 1 << 1,
        Save = 1 << 2,
        Process = 1 << 3,
        GridControlDetailView  = 1 << 4,
        All = Delete | New | Save | Process|GridControlDetailView
    }
}