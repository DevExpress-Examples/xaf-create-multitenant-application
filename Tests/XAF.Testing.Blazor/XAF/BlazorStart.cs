using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.ApplicationBuilder;
using Microsoft.EntityFrameworkCore;
using XAF.Testing.RX;
using XAF.Testing.XAF;
using static System.AppDomain;
using static System.Environment;
using static XAF.Testing.Monitor;
using Uri = System.Uri;

namespace XAF.Testing.Blazor.XAF{
    public static class BlazorStart{
        static BlazorStart() => CurrentDomain.Await(async () => await Tracing.Use());

        public static IObservable<Unit> Run(this IHostBuilder builder,string address,string contentRoot, Action<IWebHostBuilder, ISubject<Unit>> config,string browser){
            return Subject.Synchronize(new Subject<Unit>()).Observe()
                .Do(_ => builder.Initialize(address, browser))
                .SelectMany(subject => builder.ConfigureWebHostDefaults(address, contentRoot, config, subject)
                    .Build().Use(host => host.RunAsync().ToObservable().DoNotComplete())
                    .TakeUntil(subject.Select(unit => unit)).FirstOrDefaultAsync()
                );
        }

        private static IHostBuilder ConfigureWebHostDefaults(this IHostBuilder builder, string address, string contentRoot, Action<IWebHostBuilder, ISubject<Unit>> config, ISubject<Unit> innerCompleted) 
            => builder.ConfigureWebHostDefaults(webBuilder => {
                config(webBuilder, innerCompleted);
                webBuilder.UseUrls(address);
                webBuilder.UseContentRoot(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, contentRoot)));
            });

        private static void Initialize(this IHostBuilder builder, string address, string browser){
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
                        .Do(unit => application.LogOff())
                        .Do(_ => process.CloseMainWindow(),_ => process.CloseMainWindow(),() => process.CloseMainWindow()))
                    .Switch()
                    .ToUnit()
                    .DoOnComplete(() => whenCompleted.OnNext()) 
                ).Subscribe();
            return (TStartup)startup;
        }
    
        public static void ConfigureXafApplication(this IBlazorApplicationBuilder builder,IApplicationStartup startup, Action<BlazorApplication> whenApplication){
            startup.WhenApplication.SelectMany(application => application.WhenLoggedOn(startup.User).To<Frame>().IgnoreElements()
                .Merge(application.WhenLoggedOn().ToFirst().To<Frame>())).Subscribe();
            builder.AddBuildStep(application => {
                application.DatabaseUpdateMode=DatabaseUpdateMode.UpdateDatabaseAlways;
                application.DatabaseVersionMismatch += (sender, e) => {
                    e.Updater.Update();
                    e.Handled = true;
                };
                whenApplication((BlazorApplication)application);
            });
        }
    }

    public interface IApplicationStartup{
        IObservable<BlazorApplication> WhenApplication{ get; }
        string User{ get; set; }
    }
}