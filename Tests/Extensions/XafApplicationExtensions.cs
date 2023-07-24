using DevExpress.ExpressApp.Win;
using Humanizer;
using OutlookInspired.Module.Services;

namespace Tests.Extensions{
    public static class XafApplicationExtensions{
        public static async Task<TestObserver<T>> StartAsync<T>(this WinApplication application, IAsyncEnumerable<T> test, int delay = 200){
            var testObserver = new TestObserver<T>(test);
            await Task.WhenAll(application.RunTestAsync( delay, testObserver), application.StartApplicationAsync());
            return testObserver;
        }

        private static Task<Task> RunTestAsync<T>(this WinApplication application, int delay, TestObserver<T> testObserver) 
            => Task.Run(async () => {
                // await application.WhenFrameCreated().TakeAsync(1);
                await testObserver.EnumerateAsync();
            }).ContinueWith(async task => await task.Delay(delay.Milliseconds())
                    .ContinueWith(_ => ((Form)application.MainWindow.Template).Invoke(application.Exit)));

        static Task StartApplicationAsync(this WinApplication application){
            application.Start();
            return Task.CompletedTask;
        }
    }
}