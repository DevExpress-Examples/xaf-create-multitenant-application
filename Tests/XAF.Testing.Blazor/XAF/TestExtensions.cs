using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.ApplicationBuilder;
using DevExpress.ExpressApp.Blazor.Services;
using Microsoft.EntityFrameworkCore;
using XAF.Testing.RX;
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

        public static IObservable<Unit> StartTest<TStartup, TDBContext>(this IHostBuilder builder, string url,
            string contentRoot,string user, Func<BlazorApplication, IObservable<Unit>> test,Action<IServiceCollection> configure=null, string browser = null)
            where TStartup : class where TDBContext : DbContext 
            => builder.ConfigureWebHostDefaults<TStartup>( url, contentRoot,configure).Build()
                .Observe().SelectMany(host => Application.DeleteModelDiffs<TDBContext>()
                    .SelectMany(application => application.WhenLoggedOn(user).IgnoreElements()
                        .Merge(application.WhenMainWindowCreated().To(application))
                        .TakeUntilDisposed(application).Cast<BlazorApplication>()
                        .SelectMany(xafApplication => test(xafApplication).To(xafApplication)))
                    .Select(application => application.ServiceProvider)
                    .Do(serviceProvider => serviceProvider.StopApplication())
                    .DoOnError(_ => host.Services.StopApplication()).Take(1)
                    .MergeToUnit(host.Run(url, browser)))
                .LogError();

        

        private static IObservable<Unit> Run(this IHost host,string url, string browser) 
            => host.Services.WhenApplicationStarted().SelectMany(_ => new Uri(url).Start(browser)
                    .SelectMany(process => host.Services.WhenApplicationStopping().Do(_ => process.Kill()).Take(1)))
                .MergeToUnit(Observable.Start(() => host.RunAsync().ToObservable()).Merge());

        public static IObservable<BlazorApplication> DeleteModelDiffs<T>(this IObservable<BlazorApplication> source) where T : DbContext 
            => source.Do(application => {
                application.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
                application.ConnectionString = application.GetRequiredService<IConfiguration>()
                    .GetConnectionString("ConnectionString");
                application.DeleteModelDiffs<T>();
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



        public static IObservable<Unit> Run(this IHostBuilder builder,string address,string contentRoot, Action<IWebHostBuilder, ISubject<Unit>> config,string browser){
            return Subject.Synchronize(new Subject<Unit>()).Observe()
                .Do(_ => builder.Initialize(address, browser))
                .SelectMany(subject => builder.ConfigureWebHostDefaults(address, contentRoot, config, subject)
                    .Build().Use(host => host.StartAsync().ToObservable().IgnoreElements().DoNotComplete().Select(unit => unit)
                    .FirstOrDefaultAsync())
                );
        }

        private static IHostBuilder ConfigureWebHostDefaults(this IHostBuilder builder, string address, string contentRoot, Action<IWebHostBuilder, ISubject<Unit>> config, ISubject<Unit> innerCompleted) 
            => builder.ConfigureWebHostDefaults(webBuilder => {
                config(webBuilder, innerCompleted);
                webBuilder.UseUrls(address);
                webBuilder.UseContentRoot(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, contentRoot)));
            });

        public static void Initialize(this IHostBuilder builder, string address, string browser){
            SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            CurrentDomain.KillAll(browser ?? "chrome");
            builder.ConfigureAppConfiguration((_, configurationBuilder) 
                => configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>{ { "Urls", address } }));
        }

        public static TStartup Use<TStartup,TDBContext>(this WebHostBuilderContext context,
            Func<BlazorApplication, IObservable<Unit>> test,string user, ISubject<Unit> whenCompleted,string browser,WindowPosition inactiveMonitorLocation=WindowPosition.None) where TStartup:IApplicationStartup where TDBContext:DbContext{
            var startup = (IApplicationStartup)typeof(TStartup).CreateInstance(context.Configuration);
            startup.User=user;
            new Uri(context.Configuration["Urls"]).Start(browser)
                .MergeIgnored(process => whenCompleted.Catch<Unit,Exception>(_ => Unit.Default.Observe().Do(_ => process.Kill())))
                .Do(process => process.MoveToInactiveMonitor(inactiveMonitorLocation))
                .SelectMany(process => startup.WhenApplication
                    .DoOnFirst(application => {
                        application.ConnectionString= application.GetRequiredService<IConfiguration>().GetConnectionString("ConnectionString");
                        application.DeleteModelDiffs<TDBContext>();
                    })
                    .Select(application => application)
                    .TakeUntil(process.WhenExited().Select(process1 => process1))
                    .Select(application => test(application).TakeUntil(application.WhenDisposed().Select(blazorApplication => blazorApplication))
                        .LogError()
                        .SelectMany(_ => {
                            return application.WhenLoggedOff()
                                .MergeToUnit(application.DeferAction(application.LogOff))
                                .SelectMany(_ => {
                                    var stopped = new Subject<Unit>();
                                    var hostApplicationLifetime = application.GetRequiredService<IHostApplicationLifetime>();
                                    hostApplicationLifetime.ApplicationStopped.Register(_ => stopped.OnNext(),null);
                                    hostApplicationLifetime.StopApplication();
                                    return stopped.Take(1).Select(unit2 => unit2);
                                    
                                    
                                    
                                });

                        })
                        // .Do(_ => process.CloseMainWindow(),_ => process.CloseMainWindow(),() => process.CloseMainWindow())
                    )
                    .Switch()
                    .ToUnit()
                    .DoOnComplete(whenCompleted.OnNext) 
                ).Take(1).Subscribe();
            return (TStartup)startup;
        }
    
        public static void ConfigureXafApplication(this IBlazorApplicationBuilder builder,IApplicationStartup startup, Action<BlazorApplication> whenApplication){
            startup.WhenApplication.SelectMany(application => application.WhenLoggedOn(startup.User).To<Frame>().IgnoreElements()
                .Merge(application.WhenLoggedOn().ToFirst().To<Frame>())).Take(1).Subscribe();
            builder.AddBuildStep(application => {
                // application.DatabaseUpdateMode=DatabaseUpdateMode.UpdateDatabaseAlways;
                // application.DatabaseVersionMismatch += (sender, e) => {
                //     e.Updater.Update();
                //     e.Handled = true;
                // };
                whenApplication((BlazorApplication)application);
            });
        }
    }

    public interface IApplicationStartup{
        IObservable<BlazorApplication> WhenApplication{ get; }
        string User{ get; set; }
    }
}