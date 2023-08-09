using System.Reactive.Linq;
using DevExpress.ExpressApp.Testing.RXExtensions;
using DevExpress.ExpressApp.Win;
using DevExpress.Persistent.BaseImpl.EF;

namespace DevExpress.ExpressApp.Testing.DevExpress.ExpressApp{
    public static class TestExtensions{
        public static TestObserver<T> StartWinTest<T>(this WinApplication application, IObservable<T> test) 
            => application.Start( test, new WindowsFormsSynchronizationContext());

        private static TestObserver<T> Start<T>(this WinApplication application, IObservable<T> test, WindowsFormsSynchronizationContext context) 
            => application.Start(application.WhenFrameCreated().Take(1)
                .Do(_ => SynchronizationContext.SetSynchronizationContext(context))
                .IgnoreElements().To<T>()
                .Merge(test.BufferUntilCompleted().Do(_ => application.Exit()).SelectMany())
                .Catch<T, Exception>(exception => {
                    context.Send(_ => application.Exit(),null);
                    return Observable.Throw<T>(exception);
                })
            );

        public static void DeleteModelDiffs(this WinApplication application){
            using var objectSpace = application.CreateObjectSpace(typeof(ModelDifference));
            objectSpace.Delete(objectSpace.GetObjectsQuery<ModelDifference>().ToArray());
            objectSpace.CommitChanges();
        }
        public static void ChangeStartupState(this WinApplication application,FormWindowState windowState) 
            => application.WhenFrameCreated(TemplateContext.ApplicationWindow)
                .TemplateChanged().Select(frame => frame.Template)
                .Cast<Form>()
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