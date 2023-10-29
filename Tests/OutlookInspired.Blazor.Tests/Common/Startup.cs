using System.Reactive.Linq;
using System.Reactive.Subjects;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.ApplicationBuilder;
using DevExpress.ExpressApp.Blazor.Services;
using OutlookInspired.Tests.Services;
using XAF.Testing.Blazor.XAF;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Blazor.Tests.Common{
    // public class XafApplicationMonitor : IXafApplicationFactory
    // {
    //     private readonly IXafApplicationFactory _innerFactory;
    //
    //     public XafApplicationMonitor(IXafApplicationFactory innerFactory) => _innerFactory = innerFactory;
    //
    //     private static readonly ISubject<BlazorApplication> _applicationSubject = new Subject<BlazorApplication>();
    //     
    //     public IObservable<BlazorApplication> Application => _applicationSubject.AsObservable();
    //     public BlazorApplication CreateApplication() => _applicationSubject.PushNext(_innerFactory.CreateApplication());
    //
    //     public static IObservable<BlazorApplication> WhenAApplication(){
    //         return _applicationSubject.AsObservable();
    //     }
    // }

    // public class Startup:Server.Startup, IApplicationStartup{
    //     public Startup(IConfiguration configuration) : base(configuration){
    //     }
    //
    //     
    //     public string User{ get; set; }
    //
    //     protected override void Configure(IBlazorApplicationBuilder builder){
    //         base.Configure(builder);
    //         builder.ConfigureXafApplication(this, application => _whenApplicationSubject.OnNext(application));
    //         builder.Services.AddPlatformServices();
    //         builder.Services.AddScoped<IPdfViewerAssertion,PdfViewerAssertion>();
    //         builder.Services.AddScoped<IAssertMapControl,AssertMapControl>();
    //         builder.Services.AddScoped<IAssertFilterView,AssertAssertFilterView>();
    //         builder.Services.AddScoped<IFilterViewManager,FilterViewManager>();
    //         builder.Services.AddScoped<IDashboardColumnViewObjectSelector,DashboardColumnViewObjectSelector>();
    //         builder.Services.AddScoped<IUserControlProvider, UserControlProvider>();
    //         builder.Services.AddScoped<IUserControlProperties, UserControlProperties>();
    //         
    //     }
    // }
    

}