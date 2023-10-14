using System.Reactive;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model;
using DevExpress.XtraLayout;
using Microsoft.Extensions.DependencyInjection;
using XAF.Testing.RX;
using XAF.Testing.XAF;
using ListView = DevExpress.ExpressApp.ListView;
using View = DevExpress.ExpressApp.View;

namespace XAF.Testing.Win.XAF{
    public static class PlatformImplementations{
        public static void AddPlatformServices(this IServiceCollection serviceCollection){
            serviceCollection.AddSingleton<IRichEditControlAsserter, RichEditControlAsserter>();
            serviceCollection.AddSingleton<IPdfViewerAsserter, PdfViewerAsserter>();
            serviceCollection.AddSingleton<IDashboardViewGridControlDetailViewObjectsAsserter, DashboardViewGridControlDetailViewObjectsAsserter>();
            serviceCollection.AddSingleton<IFilterClearer, FilterClearer>();
            serviceCollection.AddSingleton<IDashboardDocumentActionAsserter, DashboardDocumentActionAsserter>();
            serviceCollection.AddSingleton<ITabControlObserver, TabControlObserver>();
            serviceCollection.AddSingleton<ITabControlAsserter, TabControlAsserter>();
            serviceCollection.AddSingleton<IObjectCountAsserter, ObjectCountAsserter>();
            serviceCollection.AddSingleton<IDashboardColumnViewObjectSelector, DashboardColumnViewObjectSelector>();
            serviceCollection.AddSingleton<IFrameObjectObserver, FrameObjectObserver>();
            serviceCollection.AddSingleton<INewObjectController, NewObjectController>();
            serviceCollection.AddSingleton<INewRowAdder, NewRowAdder>();
            serviceCollection.AddSingleton<IReportAsserter, ReportAsserter>();
            serviceCollection.AddSingleton<ISelectedObjectProcessor, SelectedObjectProcessor>();
            serviceCollection.AddSingleton<IWindowMaximizer, WindowMaximizer>();
            serviceCollection.AddSingleton<IMapControlAsserter, MapControlAsserter>();
            serviceCollection.AddSingleton(typeof(IObjectSelector<>), typeof(ObjectSelector<>));
        }
    }
    
    public class NewObjectController : INewObjectController{
        public IObservable<Frame> CreateNewObjectController(Frame frame) 
            => frame.CreateNewObjectController();
    }
    
    public class NewRowAdder : INewRowAdder{
        public void AddNewRowAndCloneMembers(Frame frame, object existingObject) 
            => frame.AddNewRowAndCloneMembers(existingObject);
    }

    public class ObjectSelector<T> : IObjectSelector<T> where T : class{
        public IObservable<T> SelectObject(ListView view, params T[] objects) 
            => view.SelectObject(objects);
    }
    
    public class ReportAsserter : IReportAsserter{
        public IObservable<Unit> AssertReport(Frame frame, string item) 
            => frame.AssertReport(item).ToUnit();
    }

    public class MapControlAsserter : IMapControlAsserter{
        public IObservable<Unit> AssertMapControl(DetailView detailView) 
            => detailView.AssertMapsControl().ToUnit();
    }

    public class WindowMaximizer : IWindowMaximizer{
        public IObservable<Window> WhenMaximized(Window window) 
            => window.WhenMaximized();
    }

    public class SelectedObjectProcessor : ISelectedObjectProcessor{
        public IObservable<(Frame frame, Frame detailViewFrame)> ProcessSelectedObject(Frame listViewFrame) 
            => listViewFrame.ProcessSelectedObject();
    }

    class TabControlProvider:ITabControlProvider{
        public TabControlProvider(TabbedControlGroup tabControl) => TabControl = tabControl;

        public object TabControl{ get; }
        public int TabPages => ((TabbedControlGroup)TabControl).TabPages.Count;
        public void SelectTab(int pageIndex) => ((TabbedControlGroup)TabControl).SelectedTabPageIndex = pageIndex;
    }

    public class DashboardDocumentActionAsserter : IDashboardDocumentActionAsserter{
        public IObservable<Frame> AssertDashboardViewShowInDocumentAction(SingleChoiceAction action, ChoiceActionItem item) 
            => action.AssertDashboardViewShowInDocumentAction(item);
    }

    class RichEditControlAsserter:IRichEditControlAsserter{
        public IObservable<Unit> Assert(DetailView detailView, bool assertMailMerge) 
            => detailView.AssertRichEditControl(assertMailMerge).ToUnit();
    }
    class PdfViewerAsserter:IPdfViewerAsserter{
        public IObservable<Unit> Assert(DetailView detailView) 
            => detailView.AssertPdfViewer().ToUnit();
    }
    class DashboardViewGridControlDetailViewObjectsAsserter:IDashboardViewGridControlDetailViewObjectsAsserter{
        public IObservable<Frame> AssertDashboardViewGridControlDetailViewObjects(Frame frame, params string[] relationNames) 
            => frame.Observe().AssertDashboardViewGridControlDetailViewObjects(relationNames);
    }
    public class FilterClearer : IFilterClearer{
        public void Clear(ListView listView) => listView.ClearFilter();
    }

    public class TabControlObserver:ITabControlObserver{
        public IObservable<ITabControlProvider> WhenTabControl(DetailView detailView, IModelViewLayoutElement element) 
            => detailView.WhenTabControl(element);
    }
    public class ObjectCountAsserter : IObjectCountAsserter{
        public IObservable<object> AssertObjectsCount(View view, int objectsCount) 
            => view.AssertObjectsCount(objectsCount);
    }

    class TabControlAsserter:ITabControlAsserter{
        public IObservable<ITabControlProvider> AssertTabbedGroup(Type objectType, int tabPages){
            throw new NotImplementedException("Check employee");
        }
    }


}