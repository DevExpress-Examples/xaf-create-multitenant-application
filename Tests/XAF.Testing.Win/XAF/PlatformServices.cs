using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.XtraGrid;
using DevExpress.XtraLayout;
using Microsoft.Extensions.DependencyInjection;
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
            serviceCollection.AddSingleton<IDashboardColumnViewObjectSelector, DashboardColumnViewObjectSelector>();
            serviceCollection.AddSingleton<IUserControlProvider, UserControlProvider>();
            serviceCollection.AddSingleton<IUserControlObjects, UserControlProperties>();
            serviceCollection.AddSingleton<IFrameObjectObserver, FrameObjectObserver>();
            serviceCollection.AddSingleton<INewRowAdder, NewRowAdder>();
            serviceCollection.AddSingleton<IAssertReport, AssertReport>();
            serviceCollection.AddSingleton<ISelectedObjectProcessor, SelectedObjectProcessor>();
            serviceCollection.AddSingleton<IAssertMapControl, AssertMapControl>();
            serviceCollection.AddSingleton<IWindowMaximizer, WindowMaximizer>();
            serviceCollection.AddSingleton(typeof(IObjectSelector<>), typeof(ObjectSelector<>));
        }
    }

    public class DashboardColumnViewObjectSelector : IDashboardColumnViewObjectSelector{
        public IObservable<Unit> SelectDashboardColumnViewObject(DashboardViewItem item) 
            => item.Observe().SelectDashboardColumnViewObject().ToUnit();
    }
    
    public class NewRowAdder : INewRowAdder{
        public void AddNewRowAndCloneMembers(Frame frame, object existingObject) 
            => frame.AddNewRowAndCloneMembers(existingObject);
    }

    public class ObjectSelector<T> : IObjectSelector<T> where T : class{
        public IObservable<T> SelectObject(ListView view, params T[] objects) 
            => view.SelectObject(objects);
    }
    class UserControlProperties:IUserControlObjects{
        public int ObjectsCount(object control) => ((GridControl)control).MainView.DataRowCount;
        public IObservable<object> WhenObjects(object control, int i) => ((GridControl)control).DataSource.ObserveItems(1);
    }

    class UserControlProvider:IUserControlProvider{
        public IObservable<object> WhenGridControl(DetailView detailView) 
            => detailView.WhenGridControl();
    }
    public class FrameObjectObserver : IFrameObjectObserver{
        IObservable<object> IFrameObjectObserver.WhenObjects(Frame frame, int count) 
            => frame.WhenColumnViewObjects(count).SwitchIfEmpty(Observable.Defer(() =>
                    frame.View.Observe().SelectMany(view => view.WhenObjectViewObjects(count))));
    }

    
    public class AssertReport : IAssertReport{
        public IObservable<Unit> Assert(Frame frame, string item) 
            => frame.AssertReport(item).ToUnit();
    }

    public class AssertMapControl : IAssertMapControl{
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


}