using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Core.ModelEditor;
using Humanizer;
using NUnit.Framework;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services;
using Shouldly;
using Tests.Extensions;

namespace Tests{
    public class EmployeeTests:TestBase{
        [Test][Apartment(ApartmentState.STA)]
        public async Task WhenFrameCreated_ShouldEmitValues(){
            using var application = await SetupWinApplication();
            // using var eventChannel = new EventChannel<(XafApplication, FrameCreatedEventArgs)>();
            // using var clickHandler = eventChannel.Subscribe<EventHandler<FrameCreatedEventArgs>>(
            //     (s, e) => eventChannel.Post((application, e)),
            //     handler => application!.FrameCreated += handler,
            //     handler => application!.FrameCreated -= handler);
            //
            // await ReadEventStreamAsync(eventChannel.ToAsyncEnumerable(CancellationToken.None),CancellationToken.None);
            //
            var frameCreatedObserver = new TestObserver<Frame>(application.WhenFrameCreated().Select(frame => frame)
                .Select(frame => frame.WhenViewChanged()).Merge()
                // .WhenViewChanged()
                
                // .Select(frame => frame.WhenEventFired(nameof(frame.TemplateChanged))).Merge()
                .Select(frame => frame)
                .Take(2)
            );
            var task = Task.Run(async () => await frameCreatedObserver.EnumerateAsync());
            
            var frame1 = application.CreateFrame(TemplateContext.View);
            await Task.Delay(1.Seconds());
            var frame2 = application.CreateFrame(TemplateContext.View);
            await Task.Delay(1.Seconds());
            frame1.SetTemplate(new LocalizationForm());
            // var frame1ViewChaingedObserver = new TestObserver<Frame>(frame1.WhenViewChanged().Select(frame => frame).Take(2));
            // var task1 = Task.Run(async () => await frame1ViewChaingedObserver.EnumerateAsync());
            // frame1.SetView(application.CreateListView(typeof(Crest),true));
            
            
            frame2.SetTemplate(new LocalizationForm());
            // var frame2ViewChaingedObserver = new TestObserver<Frame>(frame1.WhenViewChanged().Select(frame => frame).Take(2));
            // var task2 = Task.Run(async () => await frame2ViewChaingedObserver.EnumerateAsync());
            // frame1.SetView(application.CreateListView(typeof(Customer),true));

            
            // await task.ConfigureAwait(false);
            //
            // Assert.True(frameCreatedObserver.ItemsCount > 0, "No frames were created.");
        }

        static async Task ReadEventStreamAsync(
            IAsyncEnumerable<(XafApplication, FrameCreatedEventArgs)> source, 
            CancellationToken token)
        {
            await foreach (var (component, text) in source)
            {
                // e.g., delay processing
                await Task.Delay(100, token);
                Console.WriteLine($"{component.GetType().Name}: {text}");
            }
        }

        [Test][Apartment(ApartmentState.STA)]
        public async Task Navigate_To_DashboardView(){
            using var application = await SetupWinApplication();

            // var testObserver = await application.StartAsync(application
            //     .WhenFrame(typeof(Crest)).Take(1)
            // );
            // var testObserver = await application.StartAsync(application.Navigate("Employee"));
            var testObserver = await application.StartAsync(application.WhenFrame().Select(frame => frame).Take(20));

            testObserver.Items.Count.ShouldBe(1);
            
        }
    
    }
}