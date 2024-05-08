using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.Services;
using DevExpress.ExpressApp.MultiTenancy;
using DevExpress.ExpressApp.SystemModule;
using Microsoft.EntityFrameworkCore;
using XAF.Testing.XAF;
using static System.AppDomain;
using static System.Environment;
using static XAF.Testing.Blazor.XAF.TestExtensions.XafApplicationMonitor;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
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
                .Observe().SelectMany(host => Application
                    .EnsureMultiTenantMainDatabase()
                    .DeleteModelDiffs<TDBContext>(application => application.GetRequiredService<IConfiguration>().GetConnectionString("ConnectionString"),user).Cast<BlazorApplication>()
                    .TakeUntil(host.Services.WhenApplicationStopping())
                    .MergeIgnored(application => application.WhenLoggedOn(user).TakeUntil(host.Services.WhenApplicationStopping()))
                    .SelectMany(application => test(application).TakeUntil(host.Services.WhenApplicationStopping()).BufferUntilCompleted().WhenNotEmpty().To(application))
                    .Take(1)
                    .DoOnError(_ => host.Services.StopTest())
                    .Finally(() => host.Services.StopTest())
                    .MergeToUnit(host.Run(url, browser,inactiveWindowBrowserPosition)))
                .LogError()
                .Log(logContext,inactiveWindowLogContextPosition,true);

        private static void StopTest(this IServiceProvider serviceProvider){
            try{
                serviceProvider.StopApplication();
                // Logger.Exit();
            }
            catch {
                // ignored
            }
        }

        private static IObservable<Unit> Run(this IHost host,string url, string browser,WindowPosition inactiveWindowPosition=WindowPosition.None) 
            => host.Services.WhenApplicationStopping().Publish(whenHostStop => whenHostStop
                .Merge(host.Services.WhenApplicationStarted().SelectMany(_ => new Uri(url).Start(browser)
                        .MoveToMonitor(inactiveWindowPosition)
                        .SelectMany(process => whenHostStop.Do(_ => CurrentDomain.KillAll(process.ProcessName))))
                    .MergeToUnit(Observable.Start(() => host.RunAsync().ToObservable().Select(unit => unit)).Merge())));

        public static IObservable<BlazorApplication> EnsureMultiTenantMainDatabase(this IObservable<BlazorApplication> source){
            var subscribed = new BehaviorSubject<bool>(false);
            return source.SelectMany(application => {
                if (application.GetService<ITenantProvider>() == null){
                    return application.Observe();
                }
                if (application.DbExist(application.GetRequiredService<IConfiguration>().GetConnectionString("ConnectionString"))){
                    return application.WhenMainWindowCreated().DoNotComplete()
                        .TakeUntil(subscribed.WhenDefault())
                        .SelectMany(window => window.Application.MainWindow.GetController<LogoffController>().LogoffAction.Trigger().To(application))
                        .TakeUntil(application.WhenDisposed().Do(_ => subscribed.OnNext(false))).Take(1)
                        .Merge(subscribed.WhenDefault().To(application).WhenDefault(blazorApplication => blazorApplication.IsDisposed()));
                }
                subscribed.OnNext(true);
                return application.WhenLoggedOn("Admin").IgnoreElements().To<BlazorApplication>();
            });
        }

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