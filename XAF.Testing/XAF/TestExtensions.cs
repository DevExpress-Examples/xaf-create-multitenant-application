using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win;
using DevExpress.Persistent.BaseImpl.EF;
using XAF.Testing.RX;

namespace XAF.Testing.XAF{
    public static class TestExtensions{
        public static TestObserver<T> StartWinTest<T>(this WinApplication application, IObservable<T> test) 
            => application.Start( test, new WindowsFormsSynchronizationContext());

        private static TestObserver<T> Start<T>(this WinApplication application, IObservable<T> test, WindowsFormsSynchronizationContext context) 
            => application.Start(application.WhenFrameCreated().Take(1)
                .Do(_ => SynchronizationContext.SetSynchronizationContext(context))
                .IgnoreElements().To<T>()
                .Merge(test.BufferUntilCompleted().Do(_ => application.Exit()).SelectMany())
                .Merge(application.ThrowWhenHandledExceptions().To<T>())
                .Catch<T, Exception>(exception => {
                    exception=exception.AddScreenshot();
                    context.Send(_ => application.Exit(),null);
                    return exception.Throw<T>();
                })
            );
        
        public static void DeleteModelDiffs(this WinApplication application){
            using var objectSpace = application.CreateObjectSpace(typeof(ModelDifference));
            objectSpace.Delete(objectSpace.GetObjectsQuery<ModelDifference>().ToArray());
            objectSpace.CommitChanges();
        }

        public static IObservable<Form> MoveToInactiveMonitor(this IObservable<Form> source) 
            => source.DoWhen(_ => Screen.AllScreens.Length>1, form => {
                var currentScreen = Screen.FromControl(form);
                var inactiveScreen = Screen.AllScreens.FirstOrDefault(screen => !Equals(screen, currentScreen));
                if (inactiveScreen != null){
                    form.StartPosition = FormStartPosition.Manual;
                    form.Location = new Point(inactiveScreen.Bounds.Left, inactiveScreen.Bounds.Top);
                }            
            });

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