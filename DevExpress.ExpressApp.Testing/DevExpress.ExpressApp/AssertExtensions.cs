using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Testing.RXExtensions;
using DevExpress.XtraGrid;
using DevExpress.XtraPdfViewer;

namespace DevExpress.ExpressApp.Testing.DevExpress.ExpressApp{
    public static class AssertExtensions{
        public static IObservable<Frame> AssertFrame(this XafApplication application, params ViewType[] viewTypes) 
            => application.WhenFrame(viewTypes).Assert($"{nameof(AssertFrame)} {string.Join(", ",viewTypes)}");
        
        public static IObservable<Frame> AssertChangeViewVariant(this IObservable<Frame> source,string variantId) 
            => source.If(_ => variantId!=null,frame => frame.Observe().ChangeViewVariant(variantId),frame => frame.Observe()).Assert($"{nameof(AssertChangeViewVariant)} {variantId}");
        
        public static IObservable<Unit> AssertProcessSelectedObject(this IObservable<Frame> source)
            => source.SelectMany(window => window.AssertProcessSelectedObject());
        public static IObservable<Unit> AssertProcessSelectedObject(this Frame frame) 
            => frame.ProcessSelectedObject().Assert($"{nameof(AssertProcessSelectedObject)} {frame.View.Id}");
        
        public static IObservable<Frame> AssertCreateNewObject(this Frame window)
            => window.CreateNewObject().Assert();

        public static IObservable<Frame> AssertSaveNewObject(this IObservable<Frame> source)
            => source.WhenSaveNewObject().Assert();
        
        public static IObservable<Unit> AssertDeleteObject(this IObservable<Frame> source)
            => source.WhenDeleteObject().SelectMany(t => t.application.CreateObjectSpace()
                .Use(space => space.GetObjectByKey(t.type,t.keyValue).Observe().Select(o => o).WhenDefault())).Assert().ToUnit();

        public static IObservable<Frame> AssertCreateNewObject(this IObservable<Frame> source)
            => source.SelectMany(window => window.AssertCreateNewObject());
        
        
        public static IObservable<Unit> AssertExistingObjectDetailView(this XafApplication application,Type objectType=null) 
            => application.WhenExistingObjectRootDetailViewFrame(objectType).CloseWindow()
                .Assert($"{nameof(AssertExistingObjectDetailView)} {objectType?.Name}").ToUnit();

        public static IObservable<DashboardViewItem> AssertDashboardViewItems(this Frame frame,ViewType viewType,params Type[] objectTypes) 
            => frame.AssertDashboardViewItems(viewType,_ => true,objectTypes);
        public static IObservable<DashboardViewItem> AssertDashboardViewItems(this Frame frame,ViewType viewType,Func<DashboardViewItem,bool> match,params Type[] objectTypes) 
            => frame.DashboardViewItems(viewType,objectTypes).Where(match).ToNowObservable().Assert();

        public static IObservable<Frame> AssertListView(this XafApplication application,string navigation, string viewVariant=null) 
            => application.AssertNavigation(navigation)
                .AssertChangeViewVariant(viewVariant)
                .AssertListView()
                .Assert($"{nameof(AssertListView)} {navigation} {viewVariant}");

        private static IObservable<Frame> AssertListView(this IObservable<Frame> listView){
            var listViewHasObjects = listView.AssertListViewHasObjects();
            var processSelectedObject = listView.AssertProcessSelectedObject();
            var existingObjectRootDetailView = listView.SelectMany(window => window.Application.AssertExistingObjectDetailView());
            var newSaveDeleteObject = listView.AssertCreateNewObject()
                .AssertSaveNewObject().AssertDeleteObject();
            var gridControlDetailViewObjects = listView.AssertGridControlDetailViewObjects();

            return listView
                .Merge(listView)
                .Merge(listViewHasObjects.ToSecond())
                .MergeToUnit(processSelectedObject)
                .MergeToUnit(existingObjectRootDetailView)
                .ConcatToUnit(newSaveDeleteObject)
                .ConcatToUnit(gridControlDetailViewObjects)
                .IgnoreElements().To<Frame>()
                .Concat(listView);
        }
        
        public static IObservable<Frame> AssertDashboardListView(this XafApplication application,string navigationView, string viewVariant=null) 
            => application.AssertNavigation(navigationView)
                .AssertChangeViewVariant(viewVariant)
                .AssertMasterFrame().Select(item => item.Frame)
                .AssertListView()
                .Assert($"{nameof(AssertDashboardListView)} {navigationView} {viewVariant}");

        public static IObservable<object> AssertObjectsCount(this View view, int objectsCount) 
            => view.WhenDataSourceChanged().FirstAsync(o => o is CollectionSourceBase collectionSourceBase
                ? collectionSourceBase.GetCount() == objectsCount : ((GridControl)o).MainView.DataRowCount == objectsCount)
                .Assert($"{nameof(AssertObjectsCount)} {view.Id}");

        public static IObservable<object> WhenDataSourceChanged(this View view) 
            => view is ListView listView ? listView.CollectionSource.WhenCriteriaApplied()
                : view.ToDetailView().WhenControlViewItemGridControl().SelectMany(control => control.WhenDataSourceChanged().To(control));

        private static IObservable<DashboardViewItem> AssertMasterFrame(this IObservable<Frame> source) 
            => source.SelectMany(frame => frame.DashboardViewItems(ViewType.DetailView)
                .Where(item => item.Model.ActionsToolbarVisibility!=ActionsToolbarVisibility.Hide).ToNowObservable()
                    .SwitchIfEmpty(frame.AssertDashboardViewItems(ViewType.ListView))).Assert();

        public static IObservable<DashboardViewItem> AssertDashboardDetailView(this XafApplication application,string navigationId,string viewVariant){
            var navigate = application.AssertNavigation(navigationId);
            var changeViewVariant = application.AssertFrame(ViewType.DashboardView).Cast<Window>().AssertChangeViewVariant(viewVariant);
            var detailViewItem = changeViewVariant.Cast<Window>()
                .SelectMany(window => window.AssertDashboardViewItems(ViewType.DetailView,item => item.Model.ActionsToolbarVisibility==ActionsToolbarVisibility.Hide));
            var detailViewDoesNotDisplayData = detailViewItem.AssertDetailViewNotHaveObject();
            var selectListViewObject = changeViewVariant.Cast<Window>()
                .AssertSelectListViewObject(item => item.Model.ActionsToolbarVisibility!=ActionsToolbarVisibility.Hide);
            var detailViewDisplaysData = detailViewItem.AssertDetailViewHasObject();
            return navigate.MergeToUnit(changeViewVariant)
                .Concat(detailViewDoesNotDisplayData)
                .Concat(selectListViewObject.Merge(detailViewDisplaysData))
                .IgnoreElements().To<DashboardViewItem>()
                .Concat(detailViewItem);
        }

        public static IObservable<Window> AssertNavigation(this XafApplication application, string viewId)
            => application.Navigate(viewId).Assert($"{nameof(AssertNavigation)} {viewId}");

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

        public static IObservable<Unit> AssertDetailViewNotHaveObject(this IObservable<DashboardViewItem> source)
            =>  source.ToView<DetailView>().WhenDefault(detailView => detailView.CurrentObject)
                .Assert().ToUnit();

        public static IObservable<Unit> AssertDetailViewGridControlHasObjects(this IObservable<DashboardViewItem> source)
            => source.SelectMany(item => item.InnerView.ToDetailView().WhenControlViewItemGridControl().HasObjects()).ToUnit().Assert();
        public static IObservable<Unit> AssertDetailViewPdfViewerHasPages(this IObservable<DashboardViewItem> source)
            => source.SelectMany(item => item.InnerView.ToDetailView().WhenViewItemWinControl<PropertyEditor>(typeof(PdfViewer)))
                .SelectMany(t => t.item.WhenEvent(nameof(PropertyEditor.ValueRead))
                    .WhenNotDefault(_ => ((PdfViewer)t.control).PageCount))
                .ToUnit().Assert();
        
        public static IObservable<Unit> AssertDetailViewHasObject(this IObservable<DashboardViewItem> source) 
            => source.ToView<DetailView>()
                .SelectMany(detailView => detailView.WhenSelectionChanged().To(detailView))
                .WhenNotDefault(detailView => detailView.CurrentObject)
                .Assert().ToUnit();

        public static IObservable<(Frame frame, object o)> AssertListViewHasObjects(this IObservable<Frame> source)
            => source.AssertObjectViewHasObjects();
            // => source.SelectMany(frame => frame.View.ToListView().WhenObjects().To(frame))
                // .Assert($"{nameof(AssertListViewHasObjects)}");
        public static IObservable<(Frame frame, object o)> AssertObjectViewHasObjects(this IObservable<Frame> source)
            => source.WhenObjects().Assert();
        
        public static IObservable<(Frame frame, object o)> AssertListViewHasObjects(this XafApplication application,Type objectType) 
            => application.WhenFrame(objectType,ViewType.ListView).AssertListViewHasObjects()
                .Assert($"{nameof(AssertListViewHasObjects)} {objectType.Name}");
        
        

        
    }
}