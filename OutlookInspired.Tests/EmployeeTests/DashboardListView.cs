using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Testing.DevExpress.ExpressApp;
using DevExpress.ExpressApp.Testing.RXExtensions;
using NUnit.Framework;
using OutlookInspired.Tests.ImportData.Extensions;

namespace OutlookInspired.Tests.ImportData.EmployeeTests{
    [Apartment(ApartmentState.STA)]
    public class DashboardListView:TestBase{
        [TestCase("list")]
        [TestCase("Card")]
        public async Task Test(string view){
            var card = view == "Card";
            using var application = await SetupWinApplication();
            application.Model.Options.UseServerMode = false;
            var dashboardViewFrame = application.WhenFrame(ViewType.DashboardView).Cast<Window>().TakeAndReplay(1).RefCount();
            
            var navigate = application.AssertNavigate("EmployeeListView");
            var changeViewVariant = card?application.WhenFrame(ViewType.DashboardView).AssertChangeViewVariant("EmployeeCardListView"):Observable.Empty<Unit>();
            var hasRecords = dashboardViewFrame.AssertWindowHasObjects();
            var processSelectedObject = dashboardViewFrame.AssertProcessSelectedObject();
            var employeeRootDetailView = application.AssertExistingObjectDetailView();
            var newEmployee = dashboardViewFrame.AssertCreateNewObject();
            var saveNewEmployee = application.AssertSaveNewObject();
            var deleteEmployee = application.AssertDeleteCurrentObject();

            application.StartWinTest(navigate
                .Merge(changeViewVariant)
                .Merge(hasRecords)
                .Merge(processSelectedObject)
                .Merge(employeeRootDetailView)
                .Concat(newEmployee)
                .Merge(deleteEmployee)
                .Merge(saveNewEmployee)
                
                // .DoNotComplete()
            );
        }

        
    }
}