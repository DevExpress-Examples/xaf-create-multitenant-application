using System.Collections;
using Aqua.EnumerableExtensions;
using DevExpress.ExpressApp;
using NUnit.Framework;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Tests.Assert;
using OutlookInspired.Tests.Services;
using XAF.Testing;
using XAF.Testing.RX;
using XAF.Testing.XAF;
using static OutlookInspired.Module.ModelUpdaters.DashboardViewsModelUpdater;
using static OutlookInspired.Module.ModelUpdaters.NavigationItemsModelUpdater;

#pragma warning disable CS8974 // Converting method group to non-delegate type

namespace OutlookInspired.Tests.Common{
    public class TestBase{
        protected const int MaxTries = 3;
        protected readonly string ConnectionString = "Integrated Security=SSPI;Pooling=true;MultipleActiveResultSets=true;Data Source=(localdb)\\mssqllocaldb;Initial Catalog=OutlookInspired";
        public static IEnumerable<object> EmployeeVariants 
            => ViewVariants(EmployeeListView, EmployeeListView, EmployeeCardListView);
        public static IEnumerable<object> CustomerVariants 
            => ViewVariants(CustomerListView, CustomerListView, CustomerCardListView);
        public static IEnumerable<object> ProductVariants 
            => ViewVariants(ProductListView, ProductListView, ProductCardView);
        public static IEnumerable<object> OrderVariants 
            => ViewVariants(OrderListView, OrderListView, OrderGridView);
        public static IEnumerable<object> OpportunityVariants 
            => ViewVariants(Opportunities, null,null);

        private static IEnumerable<object[]> ViewVariants(string view,params string[] variants) 
            => Users.SelectMany(user => variants.Select(viewVariant => new object[]{ user, view, viewVariant }))
                .DistinctBy(objects => objects.StringJoin(""));
        
        public static IEnumerable<string> Users{
            get{
                var roleStr = $"{Environment.GetEnvironmentVariable("TEST_ROLE")}".Split(' ').Last();
                return Enum.TryParse(roleStr, out EmployeeDepartment department) && Roles.TryGetValue(department, out var user) ? user.YieldItem() :
                    roleStr == "Admin" ? "Admin".YieldItem() : Roles.Values;
            }
        }

        private static readonly Dictionary<EmployeeDepartment, string> Roles = new(){
            { EmployeeDepartment.Sales, "clarkm"},{EmployeeDepartment.HumanResources,"gretas"},
            {EmployeeDepartment.Support,"jamesa"},{EmployeeDepartment.Shipping,"dallasl"},
            {EmployeeDepartment.Engineering,"barta"},{EmployeeDepartment.Management,"johnh"},{EmployeeDepartment.IT,"bradleyj"},
            {(EmployeeDepartment)(-1),"Admin"}
        };
        
        public static IEnumerable<string> NavigationViews
            => new[]{"EmployeeListView"};
         
        public static IEnumerable TestCases 
            => Users.Where(s => s=="Admin").SelectMany(TestCaseData);

        private static IEnumerable<TestCaseData> TestCaseData(string user){
            yield return new TestCaseData(EmployeeListView,EmployeeListView,user, AssertEmployeeListView);
            yield return new TestCaseData(EmployeeListView,EmployeeCardListView,user, AssertEmployeeListView);
            yield return new TestCaseData(CustomerListView,"CustomerListView",user,AssertCustomerListView);
            yield return new TestCaseData(CustomerListView,"CustomerCardListView",user, AssertCustomerListView);
            yield return new TestCaseData(ProductListView,"ProductCardView",user, AssertProductListView);
            yield return new TestCaseData(ProductListView,"ProductListView",user, AssertProductListView);
            yield return new TestCaseData(OrderListView,"OrderListView",user, AssertOrderListView);
            yield return new TestCaseData(OrderListView,"Detail",user, AssertOrderListView);
            yield return new TestCaseData(EvaluationListView,null,user, AssertEvaluation);
            yield return new TestCaseData(Opportunities,null,user,AssertOpportunitiesView);
        }

        protected virtual LogContext LogContext{
            get{
#if TEST
                return null;
#else
                return LogContext.None;
#endif
            }
        }


        public IObservable<Frame> AssertNewUser(XafApplication application, string navigationView, string viewVariant){
            throw new NotImplementedException();
        }
        static IObservable<Frame> AssertEvaluation(XafApplication application, string navigationView, string viewVariant) 
            => application.AssertNavigationItems((action, item) => action.NavigationItems(item))
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