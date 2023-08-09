using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Testing.RXExtensions;
using DevExpress.XtraPdfViewer;

namespace DevExpress.ExpressApp.Testing.DevExpress.ExpressApp{
    public static class AssertExtensions{
        public static IObservable<Frame> AssertFrame(this XafApplication application, params ViewType[] viewTypes) 
            => application.WhenFrame(viewTypes).Assert($"{nameof(AssertFrame)} {string.Join(", ",viewTypes)}");
        
        public static IObservable<Frame> AssertChangeViewVariant(this IObservable<Frame> source,string variantId) 
            => source.If(_ => variantId!=null,frame => frame.Observe().ChangeViewVariant(variantId),frame => frame.Observe()).Assert($"{nameof(AssertChangeViewVariant)} {variantId}");
        
        public static IObservable<Unit> AssertProcessSelectedObject(this IObservable<Window> source)
            => source.SelectMany(window => window.AssertProcessSelectedObject());
        public static IObservable<Unit> AssertProcessSelectedObject(this Window window) 
            => window.ProcessSelectedObject().Assert($"{nameof(AssertProcessSelectedObject)} {window.View.Id}");
        
        public static IObservable<Frame> AssertCreateNewObject(this Window window)
            => window.CreateNewObject().Assert();

        public static IObservable<Frame> AssertSaveNewObject(this IObservable<Frame> source)
            => source.WhenSaveNewObject().Assert();
        
        public static IObservable<Unit> AssertDeleteObject(this IObservable<Frame> source)
            => source.WhenDeleteObject().SelectMany(t => t.application.CreateObjectSpace()
                .Use(space => space.GetObjectByKey(t.type,t.keyValue).Observe().Select(o => o).WhenDefault())).Assert().ToUnit();

        public static IObservable<Frame> AssertCreateNewObject(this IObservable<Window> source)
            => source.SelectMany(window => window.AssertCreateNewObject());
        
        
        public static IObservable<Unit> AssertExistingObjectDetailView(this XafApplication application,Type objectType=null) 
            => application.WhenExistingObjectRootDetailViewFrame(objectType).CloseWindow()
                .Assert($"{nameof(AssertExistingObjectDetailView)} {objectType?.Name}").ToUnit();

        public static IObservable<DashboardViewItem> AssertDashboardViewItems(this Window frame,ViewType viewType,params Type[] objectTypes) 
            => frame.AssertDashboardViewItems(viewType,_ => true,objectTypes);
        public static IObservable<DashboardViewItem> AssertDashboardViewItems(this Window frame,ViewType viewType,Func<DashboardViewItem,bool> match,params Type[] objectTypes) 
            => frame.DashboardViewItems(viewType,objectTypes).Where(match).ToNowObservable().Assert();

        public static IObservable<Unit> AssertDashboardListView(this XafApplication application,string navigationView, string viewVariant){
            var dashboardViewFrame = application.AssertFrame(ViewType.DashboardView).Cast<Window>();
            var navigate = application.AssertNavigation(navigationView);
            var changeViewVariant = dashboardViewFrame.AssertChangeViewVariant(viewVariant);
            var hasRecords = dashboardViewFrame.AssertWindowHasObjects();
            var processSelectedObject = dashboardViewFrame.AssertProcessSelectedObject();
            var existingObjectRootDetailView = application.AssertExistingObjectDetailView();
            var newSaveDeleteObject = dashboardViewFrame.AssertCreateNewObject()
                .AssertSaveNewObject().AssertDeleteObject();

            var gridControlDetailViewObjects = dashboardViewFrame.AssertGridControlDetailViewObjects();
            var testDashboardListView = navigate
                .Merge(changeViewVariant)
                .Merge(hasRecords)
                .MergeToUnit(processSelectedObject)
                .Merge(existingObjectRootDetailView)
                .ConcatToUnit(newSaveDeleteObject)
                .ConcatToUnit(gridControlDetailViewObjects);
            return testDashboardListView;
        }

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

        public static IObservable<object> AssertWindowHasObjects(this IObservable<Window> source)
            => source.If(window => window.DashboardViewItems<DetailView>().Any(),window => window.Observe().WhenObjects()
                    .Assert($"{nameof(AssertWindowHasObjects)} {window.View.Id}"),
                window => window.DashboardViewItems<ListView>().ToNowObservable().BufferUntilCompleted()
                    .SelectMany(listViews => listViews.ToNowObservable().SelectMany(listView => listView.WhenObjects().Take(1))
                        .Take(listViews.Length))
                    .Assert($"{nameof(AssertWindowHasObjects)} {window.View.Id}"));
        
        public static IObservable<object> AssertGridControlDetailViewObjects(this IObservable<Window> source)
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

        public static IObservable<Unit> AssertListViewHasObjects(this XafApplication application,Type objectType) 
            => application.WhenFrame(objectType,ViewType.ListView).ToListView()
                .SelectMany(listView => listView.WhenObjects())
                .Assert($"{nameof(AssertListViewHasObjects)} {objectType.Name}").ToUnit();
        
        

        
    }
}