using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win;
using DevExpress.Mvvm.Native;
using Humanizer;
using OutlookInspired.Module.Services;

namespace Tests{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IAsyncEnumerable<T> asyncEnumerable;
        private IAsyncEnumerator<T> asyncEnumerator;

        public TestAsyncEnumerator(IAsyncEnumerable<T> asyncEnumerable)
        {
            this.asyncEnumerable = asyncEnumerable;
        }

        public ValueTask DisposeAsync()
        {
            return new ValueTask();
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return asyncEnumerator.MoveNextAsync();
        }

        public T Current => asyncEnumerator.Current;

        public async Task EnumerateAsync()
        {
            asyncEnumerator = asyncEnumerable.GetAsyncEnumerator();
            try
            {
                while (await asyncEnumerator.MoveNextAsync())
                {
                    // Consume the elements from the async enumerator
                }
            }
            finally
            {
                await asyncEnumerator.DisposeAsync();
            }
        }
    }


    public static class XafApplicationExtensions
    {
        public static async Task<TestAsyncEnumerator<T>> StartAsync<T>(this WinApplication application, IAsyncEnumerable<T> test, int delay = 200){
            var testEnumerator = test.GetAsyncEnumerator();
            Task TestTask(){
                try{
                    application.Start();
                }
                finally{
                    application.DelayAndExit(delay.Milliseconds()).Wait();
                }
                return Task.CompletedTask;
            }

            await Task.WhenAll(TestTask(), testEnumerator.EnumerateAsync());

            return new TestAsyncEnumerator<T>(test);
        }
    }

    public static class TestExtensions{
        
        public static async Task<TestAsyncEnumerator<T>> StartWinTest<T>(this XafApplication application, IAsyncEnumerable<T> test, int delay = 200){
            // var frame = await application.WhenFrame(Nesting.Root)
            // var frame =  application.WhenEventFired<FrameCreatedEventArgs>(nameof(XafApplication.FrameCreated))
            //     .Select(e => e.Frame).Take(1)
            //     .Do(_ => SynchronizationContext.SetSynchronizationContext((SynchronizationContext)AppDomain.CurrentDomain
            //         .GetAssemblyType("System.Windows.Forms.WindowsFormsSynchronizationContext").CreateInstance()))
            //     .SelectAwait(async frame =>
            //     {
            //         await (frame.Template).WhenEventFired("Activated")
            //             .Take(1)
            //             .WaitUntilInactive(2.Seconds())
            //             .Take(1)
            //             .ObserveOnContext(SynchronizationContext.Current)
            //             .ToListAsync();
            //         return frame;
            //     })
            //     .DoNotComplete();
            //
            // await Task.Delay(delay); // Delay before starting the test
            //
            // return new TestAsyncEnumerator<T>(test);
            throw new InvalidOperationException();
        }





    }
}