using DevExpress.ExpressApp.Testing.DevExpress.ExpressApp;
using NUnit.Framework;
using OutlookInspired.Tests.ImportData.Extensions;

namespace OutlookInspired.Tests.ImportData.DashboardTests{
    [Apartment(ApartmentState.STA)]
    public class DashboardListView:TestBase{
        [TestCase("Oppurtunities", null)]
        [TestCase("OrderListView", "Detail")]
        [TestCase("OrderListView", "OrderListView")]
        [TestCase("ProductListView", "ProductCardView")]
        [TestCase("ProductListView", "ProductListView")]
        [TestCase("CustomerListView", "CustomerCardListView")]
        [TestCase("CustomerListView", "CustomerListView")]
        [TestCase("EmployeeListView", "EmployeeCardListView")]
        [TestCase("EmployeeListView", "EmployeeListView")]
        public async Task Test(string navigationView, string viewVariant) {
            using var application = await SetupWinApplication();
            application.Model.Options.UseServerMode = false;
            
            application.StartWinTest(application.AssertDashboardListView(navigationView, viewVariant)
                // .DoNotComplete()
            );
        }

    }
}