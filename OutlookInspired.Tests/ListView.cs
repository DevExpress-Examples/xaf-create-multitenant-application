using NUnit.Framework;
using OutlookInspired.Tests.ImportData.Extensions;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.ImportData{
    [Apartment(ApartmentState.STA)]
    public class ListView:TestBase{
        [TestCase("Evaluation_ListView")]
        public async Task Test(string navigation){
            using var application = await SetupWinApplication();
            application.Model.Options.UseServerMode = false;
            var assertListView = application.AssertListView(navigation);
            application.StartWinTest(assertListView
                    
                // .DoNotComplete()
            );
        }

    }
}