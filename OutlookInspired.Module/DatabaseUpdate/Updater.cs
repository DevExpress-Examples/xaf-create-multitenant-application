using Aqua.EnumerableExtensions;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Module.DatabaseUpdate;
// For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
public class Updater : ModuleUpdater {
    public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
        base(objectSpace, currentDBVersion) {
    }
    
    
    public override void UpdateDatabaseAfterUpdateSchema() {
        base.UpdateDatabaseAfterUpdateSchema();
        //string name = "MyName";
        //EntityObject1 theObject = ObjectSpace.FirstOrDefault<EntityObject1>(u => u.Name == name);
        //if(theObject == null) {
        //    theObject = ObjectSpace.CreateObject<EntityObject1>();
        //    theObject.Name = name;
        //}
#if !RELEASE
        // if (!ObjectSpace.Any<Customer>()){
        //     throw new NotSupportedException(
        //         "Run the OutlookInspired.Tests.ImportData Test to import existing data from the SqlLite database");
        // }
        var sampleUser = ObjectSpace.FirstOrDefault<ApplicationUser>(u => u.UserName == "User");
        if(sampleUser == null) {
            sampleUser = ObjectSpace.CreateObject<ApplicationUser>();
            sampleUser.UserName = "User";
            // Set a password if the standard authentication type is used
            sampleUser.SetPassword("");

            // The UserLoginInfo object requires a user object Id (Oid).
            // Commit the user object to the database before you create a UserLoginInfo object. This will correctly initialize the user key property.
            ObjectSpace.CommitChanges(); //This line persists created object(s).
            ((ISecurityUserWithLoginInfo)sampleUser).CreateUserLoginInfo(SecurityDefaults.PasswordAuthentication, ObjectSpace.GetKeyValueAsString(sampleUser));
        }
        var defaultRole = CreateDefaultRole();
        sampleUser.Roles.Add(defaultRole);

        var userAdmin = ObjectSpace.FirstOrDefault<ApplicationUser>(u => u.UserName == "Admin");
        if(userAdmin == null) {
            userAdmin = ObjectSpace.CreateObject<ApplicationUser>();
            userAdmin.UserName = "Admin";
            // Set a password if the standard authentication type is used
            userAdmin.SetPassword("");

            // The UserLoginInfo object requires a user object Id (Oid).
            // Commit the user object to the database before you create a UserLoginInfo object. This will correctly initialize the user key property.
            ObjectSpace.CommitChanges(); //This line persists created object(s).
            ((ISecurityUserWithLoginInfo)userAdmin).CreateUserLoginInfo(SecurityDefaults.PasswordAuthentication, ObjectSpace.GetKeyValueAsString(userAdmin));
        }
		// If a role with the Administrators name doesn't exist in the database, create this role
        var adminRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(r => r.Name == "Administrators");
        if(adminRole == null) {
            adminRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
            adminRole.Name = "Administrators";
        }
        adminRole.IsAdministrative = true;
		userAdmin.Roles.Add(adminRole);
        if (ObjectSpace.ModifiedObjects.Any()){
            CreateViewFilters();
            CreateMailMergeTemplates();
        }
        ObjectSpace.CommitChanges(); //This line persists created object(s).


#endif
    }

    private void CreateMailMergeTemplates() 
        => new[]{
                (type: typeof(Order), name: "FollowUp"), (type: typeof(Order), name: "Order"), (type: typeof(OrderItem), name: "OrderItem"),
                (type: typeof(Employee), name: "Probation Notice"),(type: typeof(Employee), name: "Service Excellence"),(type: typeof(Employee), name: "Thank You Note")
                ,(type: typeof(Employee), name: "Welcome to DevAV"),(type: typeof(Employee), name: "Month Award"),
            }
            .ForEach(t => ObjectSpace.NewMailMergeData(t.name,t.type,GetType().Assembly
                .GetManifestResourceStream(s => s.Contains("MailMerge")  && s.EndsWith($"{t.name}.docx")).Bytes()));

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
        viewFilter = ObjectSpace.CreateObject<ViewFilter>();
        viewFilter.SetCriteria<Order>(order => order.TotalAmount>5000);
        viewFilter.Name = "Sales > $5000";
        viewFilter = ObjectSpace.CreateObject<ViewFilter>();
        viewFilter.SetCriteria<Order>(order => order.TotalAmount<5000);
        viewFilter.Name = "Sales < $5000";
        new[]{ "Jim Packard", "Harv Mudd", "Clark Morgan", "Todd Hofman" }
            .Do(name => {
                viewFilter = ObjectSpace.CreateObject<ViewFilter>();
                viewFilter.SetCriteria<Order>(order => order.Employee.FullName == name);
                viewFilter.Name = $"Sales by {name}";
            }).Enumerate();
    }

    private void DateFilters<T>(string dateProperty) where T:IViewFilter{
        var viewFilter = ObjectSpace.CreateObject<ViewFilter>();
        viewFilter.SetCriteria<T>($"IsOutlookIntervalToday([{dateProperty}])");
        viewFilter.Name = "Today";
        viewFilter = ObjectSpace.CreateObject<ViewFilter>();
        viewFilter.SetCriteria<T>($"IsOutlookIntervalYesterday([{dateProperty}])");
        viewFilter.Name = "Yesterday";
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

    private void EmployeeFilters(){
        Enum.GetValues<EmployeeStatus>().Do(status => {
            var viewFilter = ObjectSpace.CreateObject<ViewFilter>();
            viewFilter.SetCriteria<Employee>(employee => employee.Status == status);
            viewFilter.Name = status.ToString();
        }).Enumerate();
    }

    private PermissionPolicyRole CreateDefaultRole() {
        PermissionPolicyRole defaultRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(role => role.Name == "Default");
        if(defaultRole == null) {
            defaultRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
            defaultRole.Name = "Default";

            defaultRole.AddObjectPermissionFromLambda<ApplicationUser>(SecurityOperations.Read, cm => cm.ID == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
            defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails", SecurityPermissionState.Allow);
            defaultRole.AddMemberPermissionFromLambda<ApplicationUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", cm => cm.ID == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
            defaultRole.AddMemberPermissionFromLambda<ApplicationUser>(SecurityOperations.Write, "StoredPassword", cm => cm.ID == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
            defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
			defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);
        }
        return defaultRole;
    }
}
