using System.Reactive.Linq;
using System.Reactive.Subjects;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.ApplicationBuilder;
using OutlookInspired.Tests.Services;
using XAF.Testing.Blazor.XAF;
using XAF.Testing.XAF;

namespace OutlookInspired.Blazor.Tests.Common{
    public class Startup:Server.Startup, IApplicationStartup{
        public Startup(IConfiguration configuration) : base(configuration){
        }

        private readonly ISubject<BlazorApplication> _whenApplicationSubject = Subject.Synchronize(new Subject<BlazorApplication>());


        public IObservable<BlazorApplication> WhenApplication => _whenApplicationSubject.AsObservable();
        public string User{ get; set; }

        protected override void Configure(IBlazorApplicationBuilder builder){
            base.Configure(builder);
            builder.ConfigureXafApplication(this, application => _whenApplicationSubject.OnNext(application));
            builder.Services.AddPlatformServices();
            builder.Services.AddSingleton<IPdfViewerAssertion,PdfViewerAssertion>();
            builder.Services.AddSingleton<IAssertMapControl,AssertMapControl>();
            builder.Services.AddSingleton<IAssertFilterView,AssertAssertFilterView>();
            builder.Services.AddSingleton<IFilterViewManager,FilterViewManager>();
            builder.Services.AddSingleton<IDashboardColumnViewObjectSelector,DashboardColumnViewObjectSelector>();
            builder.Services.AddSingleton<IUserControlProvider, UserControlProvider>();
            builder.Services.AddSingleton<IUserControlProperties, UserControlProperties>();
            
        }
    }
    

}