using Aqua.EnumerableExtensions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Features.ViewFilter;
using OutlookInspired.Module.Services;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Module.DatabaseUpdate;
// For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
public class Updater : ModuleUpdater {
    

    public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
        base(objectSpace, currentDBVersion){
    }

    public override void UpdateDatabaseBeforeUpdateSchema(){
        base.UpdateDatabaseBeforeUpdateSchema();
        SynchronizeDatesWithToday();
    }

    private void SynchronizeDatesWithToday(){
        new[]{
                (table: nameof(OutlookInspiredEFCoreDbContext.Orders), column: nameof(Order.OrderDate)),
                (table: nameof(OutlookInspiredEFCoreDbContext.Quotes), column: nameof(Quote.Date))
            }
            .Do(t => CreateCommand($@"
DECLARE @MostRecentDate DATE = (SELECT MAX({t.column}) FROM {t.table});
DECLARE @DaysDifference INT = DATEDIFF(DAY, @MostRecentDate, GETDATE());

UPDATE {t.table}
SET {t.column} = DATEADD(DAY, @DaysDifference, {t.column});
").ExecuteNonQuery()).Enumerate();
    }

    public override void UpdateDatabaseAfterUpdateSchema() {
        base.UpdateDatabaseAfterUpdateSchema();
        
        var defaultRole = ObjectSpace.EnsureDefaultRole();
        CreateAdminObjects();
        if (ObjectSpace.ModifiedObjects.Any()){
            CreateDepartmentRoles();
            CreateViewFilters();
            ObjectSpace.CreateMailMergeTemplates();
            ObjectSpace.GetObjectsQuery<Employee>().ToArray()
                .Do(employee => {
                    employee.User = ObjectSpace.EnsureUser(employee.FirstName.ToLower()
                        .Concat(employee.LastName.ToLower().Take(1)).StringJoin(""),user => user.Employee=employee);
                    employee.User.Roles.Add(defaultRole);
                    employee.User.Roles.Add(ObjectSpace.FindRole(employee.Department));
                })
                .Enumerate();
        }
        ObjectSpace.CommitChanges();
        // NewMethod();
    }

    [Obsolete("check if it works without it")]
    private void NewMethod(){
        // ObjectSpace.GetObjectsQuery<Employee>().Where(employee =>employee.FullName=="Clark Morgan"&& !employee.UserLogins.Any()).ToArray()
        //     .Do(employee => ((ISecurityUserWithLoginInfo)employee).CreateUserLoginInfo(
        //         SecurityDefaults.PasswordAuthentication,
        //         ObjectSpace.GetKeyValueAsString(employee)))
        //     .Finally(ObjectSpace.CommitChanges)
        //     .Enumerate();
    }

    private void CreateDepartmentRoles() 
        => Enum.GetValues<EmployeeDepartment>()
            .Do(department => ObjectSpace.EnsureRole(department))
            .Enumerate();

    private void CreateAdminObjects() 
        => ObjectSpace.EnsureUser("Admin")
            .Roles.Add(ObjectSpace.EnsureRole("Administrators",isAdmin:true));
    

    private void CreateViewFilters(){
        EmployeeFilters();
        CustomerFilters();
        ProductFilters();
        OrderFilters();
        DateFilters<Quote>(nameof(Quote.Date));
    }

    private void OrderFilters(){
        DateFilters<Order>(nameof(Order.OrderDate));
        var viewFilter = ObjectSpace.CreateObject<ViewFilter>();
        viewFilter.SetCriteria<Order>(order => order.PaymentTotal==0);
        viewFilter.Name = "Unpaid Orders";
        viewFilter = ObjectSpace.CreateObject<ViewFilter>();
        viewFilter.SetCriteria<Order>(order => order.RefundTotal==order.TotalAmount);
        viewFilter.Name = "Refunds";
        // viewFilter = ObjectSpace.CreateObject<ViewFilter>();
        // viewFilter.SetCriteria<Order>(order => order.TotalAmount>5000);
        // viewFilter.Name = "Sales > $5000";
        // viewFilter = ObjectSpace.CreateObject<ViewFilter>();
        // viewFilter.SetCriteria<Order>(order => order.TotalAmount<5000);
        // viewFilter.Name = "Sales < $5000";
        // new[]{ "Jim Packard", "Harv Mudd", "Clark Morgan" }
        //     .Do(name => {
        //         viewFilter = ObjectSpace.CreateObject<ViewFilter>();
        //         viewFilter.SetCriteria<Order>(order => order.Employee.FullName == name);
        //         viewFilter.Name = $"Sales by {name}";
        //     }).Enumerate();
    }

    private void DateFilters<T>(string dateProperty) where T:IViewFilter{
        var viewFilter = ObjectSpace.CreateObject<ViewFilter>();
        viewFilter.SetCriteria<T>($"IsOutlookIntervalToday([{dateProperty}])");
        viewFilter.Name = "Today";
        viewFilter = ObjectSpace.CreateObject<ViewFilter>();
        viewFilter.SetCriteria<T>($"IsThisMonth([{dateProperty}])");
        viewFilter.Name = "This Month";
        viewFilter = ObjectSpace.CreateObject<ViewFilter>();
        viewFilter.SetCriteria<T>($"IsOutlookIntervalEarlierThisYear([{dateProperty}])");
        viewFilter.Name = "This Year";
    }

    private void ProductFilters(){
        Enum.GetValues<ProductCategory>().Do(category => {
            var viewFilter = ObjectSpace.CreateObject<ViewFilter>();
            viewFilter.SetCriteria<Product>(product => product.Category == category);
            viewFilter.Name = category.ToString();
        }).Enumerate();
        var viewFilter = ObjectSpace.CreateObject<ViewFilter>();
        viewFilter.SetCriteria<Product>(product => !product.Available);
        viewFilter.Name = "Discontinued";
        viewFilter = ObjectSpace.CreateObject<ViewFilter>();
        viewFilter.SetCriteria<Product>(product => product.CurrentInventory == 0);
        viewFilter.Name = "Out Of Stock";
    }

    private void CustomerFilters(){
        Enum.GetValues<CustomerStatus>().Do(status => {
            var viewFilter = ObjectSpace.CreateObject<ViewFilter>();
            viewFilter.SetCriteria<Customer>(customer => customer.Status == status);
            viewFilter.Name = status.ToString();
        }).Enumerate();
        var viewFilter = ObjectSpace.CreateObject<ViewFilter>();
        viewFilter.SetCriteria<Customer>(customer => customer.TotalEmployees > 10000);
        viewFilter.Name = "Employess > 10000";
        viewFilter = ObjectSpace.CreateObject<ViewFilter>();
        viewFilter.SetCriteria<Customer>(customer => customer.TotalStores > 10);
        viewFilter.Name = "Stores > 10 Location";
        viewFilter = ObjectSpace.CreateObject<ViewFilter>();
        viewFilter.SetCriteria<Customer>(customer => customer.AnnualRevenue > 100000000000);
        viewFilter.Name = "Revenues > 100 Billion";
    }

    private void EmployeeFilters() 
        => Enum.GetValues<EmployeeStatus>().Do(status => {
            var viewFilter = ObjectSpace.CreateObject<ViewFilter>();
            viewFilter.SetCriteria<Employee>(employee => employee.Status == status);
            viewFilter.Name = status.ToString();
        }).Enumerate();

}
