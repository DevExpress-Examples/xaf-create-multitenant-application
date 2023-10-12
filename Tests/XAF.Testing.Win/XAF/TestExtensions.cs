using System.Data.SqlClient;
using System.Reactive.Linq;
using Aqua.EnumerableExtensions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win;
using XAF.Testing.RX;
using XAF.Testing.XAF;

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
                    exception=exception.AddScreenshot();
                    context.Send(_ => application.Terminate(),null);
                    return Observable.Throw<T>(exception);
                })
            );

        private static void Terminate(this XafApplication application){
            Console.WriteLine(Logger.ExitSignal);
            application.Exit();
        }

        public static void DeleteModelDiffs(this WinApplication application,string connectionString,string modelDifferenceTableName,string modelDifferenceAspectTableName){
            application.ConnectionString=connectionString;
            if (application.DbExist()){
                using var sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                using var sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText=new[]{modelDifferenceTableName,modelDifferenceAspectTableName}
                    .Select(table => $"IF OBJECT_ID('{table}', 'U') IS NOT NULL Delete FROM {table};").StringJoin("");
                sqlCommand.ExecuteNonQuery();    
            }
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