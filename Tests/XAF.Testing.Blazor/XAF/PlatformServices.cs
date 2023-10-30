using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using DevExpress.Blazor;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.XtraReports.Services;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.Extensions;
using DevExpress.XtraReports.Web.WebDocumentViewer;
using XAF.Testing.RX;
using XAF.Testing.XAF;
using ListView = DevExpress.ExpressApp.ListView;
using View = DevExpress.ExpressApp.View;

namespace XAF.Testing.Blazor.XAF{
    public static class PlatformImplementations{
        public static void AddPlatformServices(this IServiceCollection serviceCollection){
            serviceCollection.AddScoped<DefaultWebDocumentViewerReportResolver>();
            serviceCollection.AddScoped<IWebDocumentViewerReportResolver>(sp => sp.GetRequiredService<DefaultWebDocumentViewerReportResolver>());
            serviceCollection.AddScoped<IReportResolver>(sp => sp.GetRequiredService<DefaultWebDocumentViewerReportResolver>());

            serviceCollection.AddScoped<IRichEditControlAsserter, RichEditControlAsserter>();
            serviceCollection.AddScoped<IDashboardViewGridControlDetailViewObjectsAsserter, DashboardViewGridControlDetailViewObjectsAsserter>();
            serviceCollection.AddScoped<IFilterClearer, FilterClearer>();
            serviceCollection.AddScoped<IDocumentActionAssertion, DocumentActionAssertion>();
            serviceCollection.AddScoped<ITabControlObserver, TabControlObserver>();
            // serviceCollection.AddSingleton<ITabControlAsserter, TabControlAsserter>();
            
            // serviceCollection.AddSingleton<IDashboardColumnViewObjectSelector, DashboardColumnViewObjectSelector>();
            serviceCollection.AddScoped<IFrameObjectObserver, FrameObjectObserver>();
            
            // serviceCollection.AddSingleton<INewObjectController, NewObjectController>();
            serviceCollection.AddScoped<INewRowAdder, NewRowAdder>();
            serviceCollection.AddScoped<IAssertReport, AssertReport>();
            serviceCollection.AddScoped<ISelectedObjectProcessor, SelectedObjectProcessor>();
            serviceCollection.AddScoped<IWindowMaximizer, WindowMaximizer>();
            serviceCollection.AddScoped<IDataSourceChanged, DataSourceChanged>();
            serviceCollection.AddScoped(typeof(IObjectSelector<>), typeof(ObjectSelector<>));
        }
    }

    
    public interface IReportResolver{
        IObservable<XtraReport> WhenResolved(Frame frame);
    }
    class DefaultWebDocumentViewerReportResolver:DevExpress.XtraReports.Web.WebDocumentViewer.Native.Services.DefaultWebDocumentViewerReportResolver, IReportResolver{
        public DefaultWebDocumentViewerReportResolver(ReportStorageWebExtension reportStorageWebExtension, IReportProvider reportProvider) : base(reportStorageWebExtension, reportProvider){
        }

        private readonly Subject<XtraReport> _resolvedSubject = new();

        public IObservable<XtraReport> WhenResolved(Frame frame) => _resolvedSubject.AsObservable();

        public override XtraReport Resolve(string reportEntry) 
            => _resolvedSubject.PushNext(base.Resolve(reportEntry));
    }

    
    public class DataSourceChanged:IDataSourceChanged{
        IObservable<EventPattern<object>> IDataSourceChanged.WhenDatasourceChanged(object editor){
            if (editor is DxGridListEditor dxGridListEditor){
                return dxGridListEditor.WhenEvent(nameof(DxGridListEditor.DataSourceChanged));
            }
            throw new NotImplementedException(editor.GetType().Namespace);
        }
    }

    // public class DashboardColumnViewObjectSelector : IDashboardColumnViewObjectSelector{
    //     public IObservable<Unit> SelectDashboardColumnViewObject(DashboardViewItem item) 
    //         => item.Observe().SelectDashboardColumnViewObject().ToUnit();
    // }

    
    // public class NewObjectController : INewObjectController{
    //     public IObservable<Frame> CreateNewObjectController(Frame frame) 
    //         => frame.CreateNewObjectController();
    // }
    
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

    

    // class ControlPropertiesProperties:IControlProperties{
    //     public int ObjectsCount(object control){
    //         
    //     }
    // }
    public class FrameObjectObserver : IFrameObjectObserver{
        IObservable<object> IFrameObjectObserver.WhenObjects(Frame frame, int count) 
            => frame.View.Observe().OfType<DetailView>().SelectMany(view => view.WhenGridControl()
                    .SelectMany(o => frame.Application.GetRequiredService<IUserControlObjects>().WhenObjects(o,1)))
                .SwitchIfEmpty(Observable.Defer(() => frame.View.Observe().SelectMany(view => view.WhenObjectViewObjects(count))));
    }

    public class AssertReport : IAssertReport{
        public IObservable<Unit> Assert(Frame frame, string item){
            return frame.AssertReport(item).ToUnit();
        }
    }


    public class WindowMaximizer : IWindowMaximizer{
        public IObservable<Window> WhenMaximized(Window window) => window.Observe();
    }

    public class SelectedObjectProcessor : ISelectedObjectProcessor{
        public IObservable<(Frame frame, Frame detailViewFrame)> ProcessSelectedObject(Frame listViewFrame) 
            => listViewFrame.ProcessSelectedObject();
    }

    public interface IUserControlProcessSelectedObject{
        IObservable<Frame> Process(Frame frame, object gridControl);
    }
    
    public class DocumentActionAssertion : IDocumentActionAssertion{
        public IObservable<Frame> Assert(SingleChoiceAction action, ChoiceActionItem item) 
            => action.AssertShowInDocumentAction();
    }

    class RichEditControlAsserter:IRichEditControlAsserter{
        public IObservable<Unit> Assert(DetailView detailView, bool assertMailMerge) 
            => detailView.AssertRichEditControl(assertMailMerge).ToUnit();
    }
    class DashboardViewGridControlDetailViewObjectsAsserter:IDashboardViewGridControlDetailViewObjectsAsserter{
        public IObservable<Frame> AssertDashboardViewGridControlDetailViewObjects(Frame frame, params string[] relationNames){
            return Observable.Empty<Frame>();
            // throw new NotImplementedException();
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

    class TabControlAsserter:ITabControlAsserter{
        public IObservable<ITabControlProvider> AssertTabbedGroup(Type objectType, int tabPages){
            throw new NotImplementedException("Check employee");
        }
    }


}