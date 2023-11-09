using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.Services;
using DevExpress.ExpressApp.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using XAF.Testing.XAF;
using static System.AppDomain;
using static System.Environment;
using static XAF.Testing.Blazor.XAF.TestExtensions.XafApplicationMonitor;
using Uri = System.Uri;

namespace XAF.Testing.Blazor.XAF{
    public static class TestExtensions{
        static TestExtensions(){
            SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            CurrentDomain.Await(async () => await Tracing.Use());
        }

        public static IObservable<Unit> StartTest<TStartup, TDBContext>(this IObservable<IHostBuilder> source, string url,
            string contentRoot, string user, Func<BlazorApplication, IObservable<Unit>> test,
            Action<IServiceCollection> configure = null, string browser = null,WindowPosition inactiveWindowBrowserPosition=WindowPosition.None,
            LogContext logContext=default,WindowPosition inactiveWindowLogContextPosition=WindowPosition.None)
            where TStartup : class where TDBContext : DbContext 
            => source.SelectMany(builder => builder.StartTest<TStartup, TDBContext>(url, contentRoot, user, test,
                configure, browser, inactiveWindowBrowserPosition, logContext, inactiveWindowLogContextPosition));

        public static IObservable<Unit> StartTest<TStartup, TDBContext>(this IHostBuilder builder, string url,
            string contentRoot, string user, Func<BlazorApplication, IObservable<Unit>> test, Action<IServiceCollection> configure = null, string browser = null,
            WindowPosition inactiveWindowBrowserPosition = WindowPosition.None,LogContext logContext=default,WindowPosition inactiveWindowLogContextPosition=WindowPosition.None)
            where TStartup : class where TDBContext : DbContext 
            => builder.ConfigureWebHostDefaults<TStartup>( url, contentRoot,configure).Build()
                .Observe().SelectMany(host => Application.DeleteModelDiffs<TDBContext>()
                    .TakeUntil(host.Services.WhenApplicationStopping())
                    .SelectMany(application => application.WhenLoggedOn(user).IgnoreElements()
                        .Merge(application.WhenMainWindowCreated().To(application))
                        .TakeUntilDisposed(application).Cast<BlazorApplication>()
                        .SelectMany(xafApplication => test(xafApplication).To(xafApplication)))
                    .Select(application => application.ServiceProvider)
                    .DoAlways(() => host.Services.StopTest()).Take(1)
                    .MergeToUnit(host.Run(url, browser,inactiveWindowBrowserPosition)))
                .LogError()
                .Log(logContext,inactiveWindowLogContextPosition,true);

        private static void StopTest(this IServiceProvider serviceProvider){
            serviceProvider.StopApplication();
            Logger.Exit();
        }

        private static IObservable<Unit> Run(this IHost host,string url, string browser,WindowPosition inactiveWindowPosition=WindowPosition.None) 
            => host.Services.WhenApplicationStopping().Publish(whenHostStop => whenHostStop
                .Merge(host.Services.WhenApplicationStarted().SelectMany(_ => new Uri(url).Start(browser)
                        .MoveToMonitor(inactiveWindowPosition)
                        .SelectMany(process => whenHostStop.Do(_ => CurrentDomain.KillAll(process.ProcessName))))
                    .MergeToUnit(Observable.Start(() => host.RunAsync().ToObservable().Select(unit => unit)).Merge())));

        public static IObservable<BlazorApplication> DeleteModelDiffs<T>(this IObservable<BlazorApplication> source) where T : DbContext 
            => source.Do(application => {
                application.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
                var tenantProvider = application.GetService<ITenantProvider>();
                if (tenantProvider == null){
                    application.ConnectionString = application.GetRequiredService<IConfiguration>().GetConnectionString("ConnectionString");
                    application.DeleteModelDiffs<T>();
                }
                else{
                    if (tenantProvider.TenantName == null) return;
                    application.ConnectionString = application.ServiceProvider.GetService<IConnectionStringProvider>().GetConnectionString();
                    application.DeleteModelDiffs<T>();
                }
            });


        private static IHostBuilder ConfigureWebHostDefaults<TStartup>(this IHostBuilder builder,string url, string contentRoot,Action<IServiceCollection> configure=null) where TStartup : class 
            => builder.ConfigureWebHostDefaults(webBuilder => {
                webBuilder.UseContentRoot(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, contentRoot)));
                webBuilder.UseUrls(url);
                webBuilder.UseStartup<TStartup>();
                webBuilder.ConfigureServices(services => {
                    services.AddTransient<XafApplicationMonitor>();
                    services.Decorate<IXafApplicationFactory, XafApplicationMonitor>();
                    services.AddPlatformServices();
                    configure?.Invoke(services);
                });
            });

        public class XafApplicationMonitor : IXafApplicationFactory{
            private readonly IXafApplicationFactory _innerFactory;
            public XafApplicationMonitor(IXafApplicationFactory innerFactory) => _innerFactory = innerFactory;
            private static readonly ISubject<BlazorApplication> ApplicationSubject = new Subject<BlazorApplication>();
            public static IObservable<BlazorApplication> Application => ApplicationSubject.AsObservable();
            public BlazorApplication CreateApplication() => ApplicationSubject.PushNext(_innerFactory.CreateApplication());
        }
    }

}