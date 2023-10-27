using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.XtraGrid;
using DevExpress.XtraLayout;
using Microsoft.Extensions.DependencyInjection;
using XAF.Testing.RX;
using XAF.Testing.XAF;
using ListView = DevExpress.ExpressApp.ListView;

namespace XAF.Testing.Win.XAF{
    public static class PlatformImplementations{
        public static void AddPlatformServices(this IServiceCollection serviceCollection){
            
            serviceCollection.AddSingleton<IRichEditControlAsserter, RichEditControlAsserter>();
            serviceCollection.AddSingleton<IPdfViewerAssertion, PdfViewerAssertion>();
            serviceCollection.AddSingleton<IDashboardViewGridControlDetailViewObjectsAsserter, DashboardViewGridControlDetailViewObjectsAsserter>();
            serviceCollection.AddSingleton<IFilterClearer, FilterClearer>();
            serviceCollection.AddSingleton<IDocumentActionAssertion, DocumentActionAssertion>();
            serviceCollection.AddSingleton<ITabControlObserver, TabControlObserver>();
            serviceCollection.AddSingleton<ITabControlAsserter, TabControlAsserter>();
            serviceCollection.AddSingleton<IDashboardColumnViewObjectSelector, DashboardColumnViewObjectSelector>();
            serviceCollection.AddSingleton<IUserControlProvider, UserControlProvider>();
            serviceCollection.AddSingleton<IUserControlProperties, UserControlProperties>();
            serviceCollection.AddSingleton<IFrameObjectObserver, FrameObjectObserver>();
            // serviceCollection.AddSingleton<INewObjectController, NewObjectController>();
            serviceCollection.AddSingleton<INewRowAdder, NewRowAdder>();
            serviceCollection.AddSingleton<IReportAssertion, ReportAssertion>();
            serviceCollection.AddSingleton<ISelectedObjectProcessor, SelectedObjectProcessor>();
            serviceCollection.AddSingleton<IWindowMaximizer, WindowMaximizer>();
            serviceCollection.AddSingleton<IMapsControlAssertion, MapControlAsserter>();
            serviceCollection.AddSingleton<IDataSourceChanged, DataSourceChanged>();
            serviceCollection.AddSingleton(typeof(IObjectSelector<>), typeof(ObjectSelector<>));
        }
    }

    public class DataSourceChanged:IDataSourceChanged{
        IObservable<EventPattern<object>> IDataSourceChanged.WhenDatasourceChanged(object editor) => editor.WhenEvent("DataSourceChanged");
    }
    public class DashboardColumnViewObjectSelector : IDashboardColumnViewObjectSelector{
        public IObservable<Unit> SelectDashboardColumnViewObject(DashboardViewItem item) 
            => item.Observe().SelectDashboardColumnViewObject().ToUnit();
    }

    // public class NewObjectController : INewObjectController{
    //     public IObservable<Frame> CreateNewObjectController(Frame frame) 
    //         => frame.CreateNewObjectController();
    // }
    
    public class NewRowAdder : INewRowAdder{
        public void AddNewRowAndCloneMembers(Frame frame, object existingObject) 
            => frame.AddNewRowAndCloneMembers(existingObject);
    }

    public class ObjectSelector<T> : IObjectSelector<T> where T : class{
        public IObservable<T> SelectObject(ListView view, params T[] objects) 
            => view.SelectObject(objects);
    }
    class UserControlProperties:IUserControlProperties{
        public int ObjectsCount(object control) => ((GridControl)control).MainView.DataRowCount;
    }

    class UserControlProvider:IUserControlProvider{
        public IObservable<object> WhenGridControl(DetailView detailView) 
            => detailView.WhenGridControl();
    }
    public class FrameObjectObserver : IFrameObjectObserver{
        IObservable<(Frame frame, object o)> IFrameObjectObserver.WhenObjects(Frame frame, int count ) 
            => frame.WhenColumnViewObjects(count).SwitchIfEmpty(Observable.Defer(() =>
                    frame.View.Observe().SelectMany(view => view.WhenObjectViewObjects(count))))
                .Select(obj => (frame, o: obj));
    }

    
    public class ReportAssertion : IReportAssertion{
        public IObservable<Unit> Assert(Frame frame, string item) 
            => frame.AssertReport(item).ToUnit();
    }

    public class MapControlAsserter : IMapsControlAssertion{
        public IObservable<Unit> Assert(DetailView detailView) 
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

    public class DocumentActionAssertion : IDocumentActionAssertion{
        public IObservable<Frame> Assert(SingleChoiceAction action, ChoiceActionItem item) 
            => action.AssertShowInDocumentAction(item);
    }

    class RichEditControlAsserter:IRichEditControlAsserter{
        public IObservable<Unit> Assert(DetailView detailView, bool assertMailMerge) 
            => detailView.AssertRichEditControl(assertMailMerge).ToUnit();
    }
    class PdfViewerAssertion:IPdfViewerAssertion{
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

    class TabControlAsserter:ITabControlAsserter{
        public IObservable<ITabControlProvider> AssertTabbedGroup(Type objectType, int tabPages){
            throw new NotImplementedException("Check employee");
        }
    }


}