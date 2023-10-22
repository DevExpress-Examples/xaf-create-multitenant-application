using System.Reactive;
using System.Reactive.Linq;
using DevExpress.Blazor;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using XAF.Testing.RX;
using XAF.Testing.XAF;
using ListView = DevExpress.ExpressApp.ListView;
using View = DevExpress.ExpressApp.View;

namespace XAF.Testing.Blazor.XAF{
    public static class PlatformImplementations{
        public static void AddPlatformServices(this IServiceCollection serviceCollection){
            serviceCollection.AddSingleton<IRichEditControlAsserter, RichEditControlAsserter>();
            serviceCollection.AddSingleton<IPdfViewerAsserter, PdfViewerAsserter>();
            serviceCollection.AddSingleton<IDashboardViewGridControlDetailViewObjectsAsserter, DashboardViewGridControlDetailViewObjectsAsserter>();
            serviceCollection.AddSingleton<IFilterClearer, FilterClearer>();
            serviceCollection.AddSingleton<IDocumentActionAssertion, DocumentActionAssertion>();
            serviceCollection.AddSingleton<ITabControlObserver, TabControlObserver>();
            // serviceCollection.AddSingleton<ITabControlAsserter, TabControlAsserter>();
            serviceCollection.AddSingleton<IObjectCountAsserter, ObjectCountAsserter>();
            serviceCollection.AddSingleton<IDashboardColumnViewObjectSelector, DashboardColumnViewObjectSelector>();
            serviceCollection.AddSingleton<IFrameObjectObserver, FrameObjectObserver>();
            serviceCollection.AddSingleton<INewObjectController, NewObjectController>();
            serviceCollection.AddSingleton<INewRowAdder, NewRowAdder>();
            serviceCollection.AddSingleton<IReportAsserter, ReportAsserter>();
            serviceCollection.AddSingleton<ISelectedObjectProcessor, SelectedObjectProcessor>();
            serviceCollection.AddSingleton<IWindowMaximizer, WindowMaximizer>();
            serviceCollection.AddSingleton<IDataSourceChanged, DataSourceChanged>();
            serviceCollection.AddSingleton(typeof(IObjectSelector<>), typeof(ObjectSelector<>));
        }
    }
    
    public class DataSourceChanged:IDataSourceChanged{
        IObservable<EventPattern<object>> IDataSourceChanged.WhenDatasourceChanged(object editor){
            if (editor is DxGridListEditor dxGridListEditor){
                return dxGridListEditor.WhenEvent(nameof(DxGridListEditor.DataSourceChanged));
            }
            throw new NotImplementedException(editor.GetType().Namespace);
        }
    }

    public class DashboardColumnViewObjectSelector : IDashboardColumnViewObjectSelector{
        public IObservable<Unit> SelectDashboardColumnViewObject(Frame frame, Func<DashboardViewItem, bool> itemSelector = null) 
            => frame.DashboardViewItems(ViewType.DetailView).Where(itemSelector ?? (_ => true)).ToNowObservable().SelectDashboardColumnViewObject().ToUnit();
    }

    
    public class NewObjectController : INewObjectController{
        public IObservable<Frame> CreateNewObjectController(Frame frame) 
            => frame.CreateNewObjectController();
    }
    
    public class NewRowAdder : INewRowAdder{
        public void AddNewRowAndCloneMembers(Frame frame, object existingObject){
            throw new NotImplementedException();
            // frame.AddNewRowAndCloneMembers(existingObject);
        }
    }

    public class ObjectSelector<T> : IObjectSelector<T> where T : class{
        public IObservable<T> SelectObject(ListView view, params T[] objects) 
            => view.SelectObject(objects);
    }
    
    public class FrameObjectObserver : IFrameObjectObserver{
        IObservable<(Frame frame, object o)> IFrameObjectObserver.WhenObjects(Frame frame, int count ) 
            => frame.WhenColumnViewObjects(count).SwitchIfEmpty(Observable.Defer(() =>
                    frame.View.Observe().SelectMany(view => view.WhenObjectViewObjects(count))))
                .Select(obj => (frame, o: obj));
    }

    public class ReportAsserter : IReportAsserter{
        public IObservable<Unit> AssertReport(Frame frame, string item){
            throw new NotImplementedException();
            // return frame.AssertReport(item).ToUnit();
        }
    }


    public class WindowMaximizer : IWindowMaximizer{
        public IObservable<Window> WhenMaximized(Window window) => window.Observe();
    }

    public class SelectedObjectProcessor : ISelectedObjectProcessor{
        public IObservable<(Frame frame, Frame detailViewFrame)> ProcessSelectedObject(Frame listViewFrame) 
            => listViewFrame.ProcessSelectedObject();
    }
    
    public class DocumentActionAssertion : IDocumentActionAssertion{
        public IObservable<Frame> Assert(SingleChoiceAction action, ChoiceActionItem item) 
            => action.AssertShowInDocumentAction();
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
        public IObservable<Frame> AssertDashboardViewGridControlDetailViewObjects(Frame frame, params string[] relationNames){
            throw new NotImplementedException();
            // return frame.Observe().AssertDashboardViewGridControlDetailViewObjects(relationNames);
        }
    }
    public class FilterClearer : IFilterClearer{
        public void Clear(ListView listView) => listView.ClearFilter();
    }
    class TabControlProvider:ITabControlProvider{
        public TabControlProvider(DxFormLayoutTabPagesModel tabControl, int tabPages){
            TabPages = tabPages;
            TabControl = tabControl;
        }

        public object TabControl{ get; }
        public int TabPages{ get; }

        public void SelectTab(int pageIndex) => ((DxFormLayoutTabPagesModel)TabControl).ActiveTabIndex=pageIndex;
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