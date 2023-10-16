using System.Data.SqlClient;
using System.Reactive.Linq;
using Aqua.EnumerableExtensions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win;
using XAF.Testing.RX;
using XAF.Testing.XAF;
using static XAF.Testing.Monitor;

namespace XAF.Testing.Win.XAF{
    public static class TestExtensions{
        public static TestObserver<T> StartWinTest<T>(this WinApplication application, IObservable<T> test,string user=null) 
            => application.Start( test, new WindowsFormsSynchronizationContext(),user);

        private static TestObserver<T> Start<T>(this WinApplication application, IObservable<T> test, WindowsFormsSynchronizationContext context,string user =null) 
            => application.Start(application.WhenLoggedOn(user).Take(1).IgnoreElements().To<T>()
                .Merge(application.WhenFrameCreated().Take(1)
                    .Do(_ => SynchronizationContext.SetSynchronizationContext(context))
                    .IgnoreElements().To<T>())
                .Merge(test.BufferUntilCompleted().Do(_ => application.Terminate()).SelectMany())
                .Merge(application.ThrowWhenHandledExceptions().To<T>())
                .Catch<T, Exception>(exception => {
                    MoveActiveWindowToMainMonitor();
                    exception=exception.AddScreenshot();
                    context.Send(_ => application.Terminate(),null);
                    return Observable.Throw<T>(exception);
                })
            );

        private static void Terminate(this XafApplication application){
            Console.WriteLine(Logger.ExitSignal);
            application.Exit();
        }


        public static IObservable<Form> MoveToInactiveMonitor(this IObservable<Form> source) 
            => source.Do( form => form.Handle.UseInactiveMonitorBounds(bounds => {
                form.StartPosition = FormStartPosition.Manual;
                form.Location = new Point(bounds.Left, bounds.Top);    
            }));

        public static void ChangeStartupState(this WinApplication application,FormWindowState windowState,bool moveToInactiveMonitor=true) 
            => application.WhenFrameCreated(TemplateContext.ApplicationWindow)
                .TemplateChanged().Select(frame => frame.Template)
                .Cast<Form>()
                .If(_ => moveToInactiveMonitor,form => form.Observe().MoveToInactiveMonitor(),form => form.Observe())
                .Do(form => form.WindowState = windowState).Take(1)
                .Subscribe();

        public static TestObserver<T> Start<T>(this WinApplication application, IObservable<T> test){
            var testObserver = test.Test();
            application.Start();
            testObserver.Error?.ThrowCaptured();
            return testObserver;
        }
    

    }
}