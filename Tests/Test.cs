using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Core.ModelEditor;
using Humanizer;
using NUnit.Framework;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services;
using Tests.Extensions;

namespace Tests{
    public class Test:TestBase{
                [Test][Apartment(ApartmentState.STA)]
        public async Task WhenFrameCreated_ShouldEmitValues(){
            using var application = await SetupWinApplication();
            using var frameCreatedChannel = new EventChannel<(XafApplication, FrameCreatedEventArgs)>();
            using var frameViewChangedChannel = new EventChannel<(Frame, ViewChangedEventArgs)>();
            using var frameCreatedHandler = frameCreatedChannel.Subscribe<EventHandler<FrameCreatedEventArgs>>(
                (s, e) => frameCreatedChannel.Post((application, e)),
                handler => application!.FrameCreated += handler,
                handler => application!.FrameCreated -= handler);
            
            
            
            
            // var frameCreatedObserver = new TestObserver<Frame>(application.WhenFrameCreated().Select(frame => frame)
            //     .Select(frame => frame.WhenViewChanged()).Merge()
            //     // .WhenViewChanged()
            //     
            //     // .Select(frame => frame.WhenEventFired(nameof(frame.TemplateChanged))).Merge()
            //     .Select(frame => frame)
            //     .Take(2)
            // );
            var disposables = new List<IDisposable>();
            var task = Task.Run(async () => {
                
                await foreach (var t in frameCreatedChannel.ToAsyncEnumerable(CancellationToken.None)){
                    disposables.Add(frameViewChangedChannel.Subscribe<EventHandler<ViewChangedEventArgs>>(
                        (s, e) => frameViewChangedChannel.Post((t.Item2.Frame, e)),
                        handler => t.Item2.Frame!.ViewChanged += handler,
                        handler => t.Item2.Frame!.ViewChanged -= handler));
                    await foreach (var x in frameViewChangedChannel.ToAsyncEnumerable(CancellationToken.None)){
                        
                    }
                }
            });
            
            var frame1 = application.CreateFrame(TemplateContext.View);
            await Task.Delay(1.Seconds());
            var frame2 = application.CreateFrame(TemplateContext.View);
            await Task.Delay(1.Seconds());
            
            frame1.SetView(application.CreateListView(typeof(Crest), true));
            await Task.Delay(1.Seconds());
            frame2.SetView(application.CreateListView(typeof(Customer), true));
            await Task.Delay(1.Seconds());
            // var frame1ViewChaingedObserver = new TestObserver<Frame>(frame1.WhenViewChanged().Select(frame => frame).Take(2));
            // var task1 = Task.Run(async () => await frame1ViewChaingedObserver.EnumerateAsync());
            // frame1.SetView(application.CreateListView(typeof(Crest),true));
            
            
            // frame2.SetTemplate(new LocalizationForm());
            // var frame2ViewChaingedObserver = new TestObserver<Frame>(frame1.WhenViewChanged().Select(frame => frame).Take(2));
            // var task2 = Task.Run(async () => await frame2ViewChaingedObserver.EnumerateAsync());
            // frame1.SetView(application.CreateListView(typeof(Customer),true));

            
            await task.ConfigureAwait(false);
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

    }
    
}