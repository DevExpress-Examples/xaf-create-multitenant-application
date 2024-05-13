using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Model;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Caching;
using DevExpress.XtraReports.Services;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.Extensions;
using DevExpress.XtraReports.Web.WebDocumentViewer;
using XAF.Testing.XAF;
using ListView = DevExpress.ExpressApp.ListView;

namespace XAF.Testing.Blazor.XAF{
    public static class PlatformImplementations{
        public static void AddPlatformServices(this IServiceCollection serviceCollection){
            serviceCollection.AddSingleton<DevExpress.XtraReports.Web.WebDocumentViewer.WebDocumentViewerOperationLogger,WebDocumentViewerOperationLogger>();
            serviceCollection.AddScoped<DefaultWebDocumentViewerReportResolver>();
            serviceCollection.AddScoped<IWebDocumentViewerReportResolver>(sp => sp.GetRequiredService<DefaultWebDocumentViewerReportResolver>());
            serviceCollection.AddScoped<IReportResolver>(sp => sp.GetRequiredService<DefaultWebDocumentViewerReportResolver>());
            serviceCollection.AddScoped<IRichEditControlAsserter, RichEditControlAsserter>();
            serviceCollection.AddScoped<IDashboardViewGridControlDetailViewObjectsAsserter, DashboardViewGridControlDetailViewObjectsAsserter>();
            serviceCollection.AddScoped<IFilterClearer, FilterClearer>();
            serviceCollection.AddScoped<IDocumentActionAssertion, DocumentActionAssertion>();
            serviceCollection.AddScoped<ITabControlObserver, TabControlObserver>();
            serviceCollection.AddScoped<IFrameObjectObserver, FrameObjectObserver>();
            serviceCollection.AddScoped<IActiveWindowResolver, ActiveWindowResolver>();
            serviceCollection.AddScoped<IAssertReport, AssertReport>();
            serviceCollection.AddScoped<ISelectedObjectProcessor, SelectedObjectProcessor>();
            serviceCollection.AddScoped<IWindowMaximizer, WindowMaximizer>();
            serviceCollection.AddScoped(typeof(IObjectSelector<>), typeof(ObjectSelector<>));
        }
    }

    public class WebDocumentViewerOperationLogger:DevExpress.XtraReports.Web.WebDocumentViewer.WebDocumentViewerOperationLogger{
        private readonly ISubject<XtraReport> _cachedReportReleasedSubject = Subject.Synchronize(new Subject<XtraReport>());
        public override void CachedReportReleased(ReportCacheItem cacheItem){
            base.CachedReportReleased(cacheItem);
            _cachedReportReleasedSubject.PushNext(cacheItem.Report);
        }


        public override void CachedDocumentSourceSerializing(string documentId, CachedDocumentSource cachedDocumentSource,
            GeneratedDocumentDetails documentDetails, DocumentStorage documentStorage, PrintingSystemBase printingSystemSource){
            base.CachedDocumentSourceSerializing(documentId, cachedDocumentSource, documentDetails, documentStorage, printingSystemSource);
            _cachedReportReleasedSubject.PushNext(null);
        }
        
        public IObservable<XtraReport> WhenCachedReportReleased() 
            => _cachedReportReleasedSubject.AsObservable();
        
    }


    public interface IReportResolver{
        IObservable<XtraReport> WhenResolved();
    }
    class DefaultWebDocumentViewerReportResolver:DevExpress.XtraReports.Web.WebDocumentViewer.Native.Services.DefaultWebDocumentViewerReportResolver, IReportResolver{
        public DefaultWebDocumentViewerReportResolver(ReportStorageWebExtension reportStorageWebExtension, IReportProvider reportProvider) : base(reportStorageWebExtension, reportProvider){
        }

        private readonly Subject<XtraReport> _resolvedSubject = new();

        public IObservable<XtraReport> WhenResolved()
            => _resolvedSubject.AsObservable();
                
        public override XtraReport Resolve(string reportEntry) 
            => _resolvedSubject.PushNext(base.Resolve(reportEntry));
    }
    
    
    public class ObjectSelector<T> : IObjectSelector<T> where T : class{
        public IObservable<T> SelectObject(ListView view, params T[] objects) 
            => view.SelectObject(objects);
    }

    public class ActiveWindowResolver:IActiveWindowResolver{
        private static readonly PropertyInfo ActiveWindowMember;

        static ActiveWindowResolver() => ActiveWindowMember = typeof(BlazorWindow).GetProperty("ActiveWindow");

        public Window GetWindow(Window window) => (Window)ActiveWindowMember.GetValue(window);
    }
    public class FrameObjectObserver : IFrameObjectObserver{
        IObservable<object> IFrameObjectObserver.WhenObjects(Frame frame, int count) 
            => frame.View.Observe().OfType<DetailView>().SelectMany(view => view.WhenGridControl()
                    .SelectMany(o => frame.Application.GetRequiredService<IUserControlObjects>().WhenObjects(o,1)))
                .SwitchIfEmpty(Observable.Defer(() => frame.View.Observe().SelectMany(view => view.WhenObjectViewObjects(count))));
    }

    public class AssertReport : IAssertReport{
        public IObservable<Unit> Assert(Frame frame, string item) => frame.AssertReport(item).ToUnit();
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
        public IObservable<Frame> AssertDashboardViewGridControlDetailViewObjects(Frame frame, params string[] relationNames) => frame.Observe();
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

        public void SelectTab(int pageIndex){
            var tabPagesModel = ((DxFormLayoutTabPagesModel)TabControl);
            tabPagesModel.ActiveTabIndex = pageIndex;
            tabPagesModel.ForceRaiseChanged();
        }
    }

    public class TabControlObserver:ITabControlObserver{
        public IObservable<ITabControlProvider> WhenTabControl(DetailView detailView, IModelViewLayoutElement element) 
            => detailView.WhenTabControl(element);
    }
    

}