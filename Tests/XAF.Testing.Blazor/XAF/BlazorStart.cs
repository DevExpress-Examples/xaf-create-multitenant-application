using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.ApplicationBuilder;
using DevExpress.Persistent.Base;
using XAF.Testing.RX;
using XAF.Testing.XAF;
using static System.AppDomain;
using static System.Environment;
using static XAF.Testing.Monitor;

namespace XAF.Testing.Blazor.XAF{
    public static class BlazorStart{
        private class Tracing:DevExpress.Persistent.Base.Tracing{
            public static IObservable<Unit> ThrowOnError() 
                => Observable.FromEventPattern<EventHandler<CreateCustomTracerEventArgs>, CreateCustomTracerEventArgs>(
                        h => CreateCustomTracer += h, h => CreateCustomTracer -= h)
                    .Do(p => p.EventArgs.Tracer = new Tracing()).To<Unit>().Take(1)
                    .IgnoreElements();

            public override void LogError(Exception exception){
                base.LogError(exception);
                Throw(exception);
            }

            private void Throw(Exception exception){
                MoveActiveWindowToMainMonitor();
                throw exception.AddScreenshot();
            }

            public override void LogVerboseError(Exception exception){
                base.LogVerboseError(exception);
                Throw(exception);
            }
        }

        public static IObservable<Unit> Run(this IHostBuilder builder,Uri uri,string contentRoot, Action<IWebHostBuilder, ISubject<Unit>> config,string browser) 
            => Tracing.ThrowOnError().Merge(Subject.Synchronize(new Subject<Unit>()).Observe()
                .Do(_ => {
                    SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
                    CurrentDomain.KillAll(browser??"chrome");
                })
                .Do(_ => builder.ConfigureAppConfiguration((_, configurationBuilder) =>
                    configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>{ { "Urls", uri.ToString() } })))
                .SelectMany(innerCompleted => builder.ConfigureWebHostDefaults(webBuilder => {
                        config(webBuilder, innerCompleted);
                        webBuilder.UseUrls(uri.ToString());
                        webBuilder.UseContentRoot(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, contentRoot)));
                    })
                    .Build().Use(host => host.RunAsync().ToObservable())
                    .TakeUntil(innerCompleted).FirstOrDefaultAsync()));

        public static TStartup Use<TStartup>(this WebHostBuilderContext context,
            Func<BlazorApplication, IObservable<Unit>> test,string user, ISubject<Unit> whenCompleted, bool moveToInactiveWindow) where TStartup:IApplicationStartup{
            var startup = (IApplicationStartup)typeof(TStartup).CreateInstance(context.Configuration);
            startup.User=user;
            new Uri(context.Configuration["Urls"]).Start()
                .If(_ => moveToInactiveWindow,process => process.Observe().MoveToInactiveMonitor(),process => process.Observe())
                .SelectMany(process => startup.WhenApplication.DoNotComplete()
                    .TakeUntil(process.WhenExited().Select(process1 => process1))
                    .Select(test).Switch()
                    .DelayOnContext(3)
                    .Do(_ => process.Kill())
                    .ToUnit()
                    .DoOnComplete(() => whenCompleted.OnNext(Unit.Default)) 
                ).Subscribe();
            return (TStartup)startup;
        }
    
        public static void ConfigureXafApplication(this IBlazorApplicationBuilder builder,IApplicationStartup startup, Action<BlazorApplication> whenApplication){
            startup.WhenApplication.SelectMany(application => application.WhenLoggedOn(startup.User).To<Frame>().IgnoreElements()
                .Merge(application.WhenLoggedOn().ToFirst().To<Frame>())).Subscribe();
            builder.AddBuildStep(application => whenApplication((BlazorApplication)application));
        }
    }

    public interface IApplicationStartup{
        IObservable<BlazorApplication> WhenApplication{ get; }
        string User{ get; set; }
    }
}