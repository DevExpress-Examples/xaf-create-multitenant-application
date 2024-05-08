using System.Reactive.Linq;
using NUnit.Framework;
using OutlookInspired.Tests.Common;
using OutlookInspired.Tests.Services;
using XAF.Testing;
using XAF.Testing.Blazor.XAF;
using static OutlookInspired.Module.ModelUpdaters.NavigationItemsModelUpdater;
using TestBase = OutlookInspired.Blazor.Tests.Common.TestBase;

namespace OutlookInspired.Blazor.Tests{
    [Order(-1)][Ignore("support DxReportViewer t1228848")]
    public class ReportsTests : TestBase{
        
        [RetryTestCaseSource(nameof(CustomerVariants),MaxTries=MaxTries)]
        [Category(Tests)]
        public async Task Customer(string user, string view, string viewVariant) 
            => await StartTest(user, application => application.AssertCustomerReports(view, viewVariant));

        [RetryTestCaseSource(nameof(ProductVariants),MaxTries=MaxTries)]
        [Category(Tests)]
        public async Task Product(string user, string view, string viewVariant) 
            => await StartTest(user, application => application.AssertProductReports(view, viewVariant));

        [RetryTestCaseSource(nameof(OrderVariants),MaxTries=MaxTries)]
        [Category(Tests)]
        public async Task Order(string user, string view, string viewVariant)
            => await StartTest(user, application => application.AssertOrderReports(view, viewVariant));
        
        [RetryTestCaseSource(nameof(Users),MaxTries=MaxTries)]
        [Category(Tests)][Order(-1)]
        public async Task Reports(string user) 
            => await StartTest(user,application => application.AssertReports(application.CanNavigate(ReportDataV2ListView).ToUnit()));

    }
}