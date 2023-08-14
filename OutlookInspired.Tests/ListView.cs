using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Testing.DevExpress.ExpressApp;
using DevExpress.ExpressApp.Testing.RXExtensions;
using NUnit.Framework;
using OutlookInspired.Tests.ImportData.Extensions;

namespace OutlookInspired.Tests.ImportData{
    [Apartment(ApartmentState.STA)]
    public class ListView:TestBase{
        [TestCase("Evaluation_ListView")]
        public async Task Test(string navigation){
            using var application = await SetupWinApplication();
            application.Model.Options.UseServerMode = false;
            var navigate = application.AssertNavigation(navigation);
            var changeViewVariant = navigate.AssertChangeViewVariant(null).Cast<Window>();
            var listViewHasObjects = navigate.AssertListViewHasObjects();
            var processSelectedObject = navigate.AssertProcessSelectedObject();
            var existingObjectRootDetailView = application.AssertExistingObjectDetailView();
            var newSaveDeleteObject = navigate.AssertCreateNewObject()
                .AssertSaveNewObject().AssertDeleteObject();
            var gridControlDetailViewObjects = navigate.AssertGridControlDetailViewObjects();
            
            application.StartWinTest(navigate
                    .Merge(changeViewVariant)
                    .Merge(listViewHasObjects)
                    .MergeToUnit(processSelectedObject)
                    .MergeToUnit(existingObjectRootDetailView)
                    .ConcatToUnit(newSaveDeleteObject)
                    .ConcatToUnit(gridControlDetailViewObjects)
                    
                // .DoNotComplete()
            );
        }

    }
}