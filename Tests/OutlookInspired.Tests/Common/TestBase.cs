using System.Diagnostics;
using Aqua.EnumerableExtensions;
using Humanizer;
using OutlookInspired.Module.BusinessObjects;
using XAF.Testing;
using static OutlookInspired.Module.ModelUpdaters.DashboardViewsModelUpdater;

#pragma warning disable CS8974 // Converting method group to non-delegate type

namespace OutlookInspired.Tests.Common{
    public class TestBase{
        protected const string Tests = nameof(Tests);
#if TEST
        protected const int MaxTries = 3;
#else
        protected const int MaxTries = 1;
#endif
        static TestBase(){
            ReactiveExtensions.TimeoutInterval = (Debugger.IsAttached ? 500 : 60).Seconds();
            ReactiveExtensions.DelayOnContextInterval=TimeSpan.FromMilliseconds(250);
        }

        protected virtual bool RunInMainMonitor => false;
        protected readonly string ConnectionString = "Integrated Security=SSPI;Pooling=true;MultipleActiveResultSets=true;Data Source=(localdb)\\mssqllocaldb;Initial Catalog=OutlookInspired_Service";
        protected virtual TimeSpan Timeout => TimeSpan.FromMinutes(10);

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
                return (Enum.TryParse(roleStr, out EmployeeDepartment department) &&
                        Roles.TryGetValue(department, out var user) ? user.YieldItem() :
                    roleStr == "Admin" ? "Admin".YieldItem() : Roles.Values)
                    .Select(userName => $"{userName}@company1.com");
            }
        }

        private static readonly Dictionary<EmployeeDepartment, string> Roles = new(){
            { EmployeeDepartment.Sales, "clarkm"},{EmployeeDepartment.HumanResources,"gretas"},
            {EmployeeDepartment.Support,"jamesa"},{EmployeeDepartment.Shipping,"dallasl"},
            {EmployeeDepartment.Engineering,"barta"},{EmployeeDepartment.Management,"johnh"},{EmployeeDepartment.IT,"bradleyj"},
            {(EmployeeDepartment)(-1),"Admin"}
        };
        
        protected virtual LogContext LogContext{
#if TEST
            get{ return default; }
            #else
            get{ return LogContext.None; }
#endif
        }
        
    }
}