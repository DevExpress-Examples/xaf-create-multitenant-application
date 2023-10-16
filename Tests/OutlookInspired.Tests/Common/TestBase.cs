using System.Collections;
using DevExpress.ExpressApp;
using NUnit.Framework;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Tests.Assert;
using XAF.Testing;
using XAF.Testing.RX;
using XAF.Testing.XAF;
#pragma warning disable CS8974 // Converting method group to non-delegate type

namespace OutlookInspired.Tests.Common{
    public class TestBase{
        protected readonly string ConnectionString = "Integrated Security=SSPI;Pooling=true;MultipleActiveResultSets=true;Data Source=(localdb)\\mssqllocaldb;Initial Catalog=OutlookInspired";
        private static readonly Dictionary<EmployeeDepartment, string> Roles = new(){
            { EmployeeDepartment.Sales, "clarkm"},{EmployeeDepartment.HumanResources,"gretas"},
            {EmployeeDepartment.Support,"jamesa"},{EmployeeDepartment.Shipping,"dallasl"},
            {EmployeeDepartment.Engineering,"barta"},{EmployeeDepartment.Management,"johnh"},{EmployeeDepartment.IT,"bradleyj"},
            {(EmployeeDepartment)(-1),"Admin"}
        };

        public static IEnumerable TestCases 
            => Users().Where(s => s=="Admin").SelectMany(TestCaseData);

        private static IEnumerable<TestCaseData> TestCaseData(string user){
            yield return new TestCaseData("EmployeeListView","EmployeeListView",user, AssertEmployeeListView);
            // yield return new TestCaseData("EmployeeListView","EmployeeCardListView",user, AssertEmployeeListView);
            // yield return new TestCaseData("CustomerListView","CustomerListView",user,AssertCustomerListView);
            // yield return new TestCaseData("CustomerListView","CustomerCardListView",user, AssertCustomerListView);
            // yield return new TestCaseData("ProductListView","ProductCardView",user, AssertProductListView);
            // yield return new TestCaseData("ProductListView","ProductListView",user, AssertProductListView);
            // yield return new TestCaseData("OrderListView","OrderListView",user, AssertOrderListView);
            // yield return new TestCaseData("OrderListView","Detail",user, AssertOrderListView);
            // yield return new TestCaseData("Evaluation_ListView",null,user, AssertEvaluation);
            // yield return new TestCaseData("Opportunities",null,user,AssertOpportunitiesView);
        }

        private static IEnumerable<string> Users(){
            var roleStr = $"{Environment.GetEnvironmentVariable("TEST_ROLE")}".Split(' ').Last();
            return Enum.TryParse(roleStr, out EmployeeDepartment department) && Roles.TryGetValue(department, out var user) ? user.YieldItem() :
                roleStr == "Admin" ? "Admin".YieldItem() : Roles.Values;
        }


        protected void DeleteModelDiffs(XafApplication application) 
            => application.DeleteModelDiffs(ConnectionString, nameof(OutlookInspiredEFCoreDbContext.ModelDifferences),
                nameof(OutlookInspiredEFCoreDbContext.ModelDifferenceAspects));

        public IObservable<Frame> AssertNewUser(XafApplication application, string navigationView, string viewVariant){
            throw new NotImplementedException();
        }
        static IObservable<Frame> AssertEvaluation(XafApplication application, string navigationView, string viewVariant) 
            => application.AssertNavigationItems((action, item) => action.AssertNavigationItems(item))
                .If(action => action.CanNavigate(navigationView), _ => application.AssertListView(navigationView, viewVariant));
        
        static IObservable<Frame> AssertOpportunitiesView(XafApplication application,string navigationView,string viewVariant) 
            => application.AssertOpportunitiesView( navigationView, viewVariant);

        static IObservable<Frame> AssertProductListView(XafApplication application,string navigationView,string viewVariant) 
            => application.AssertProductListView( navigationView, viewVariant);

        public static IObservable<Frame> AssertOrderListView(XafApplication application,string navigationView,string viewVariant) 
            => application.AssertOrderListView( navigationView, viewVariant);

        static IObservable<Frame> AssertEmployeeListView(XafApplication application,string navigationView,string viewVariant) 
            => application.AssertEmployeeListView(navigationView, viewVariant);

        static IObservable<Frame> AssertCustomerListView(XafApplication application,string navigationView,string viewVariant) 
            => application.AssertCustomerListView(navigationView, viewVariant);
    }
}