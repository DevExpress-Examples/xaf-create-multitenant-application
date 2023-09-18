using System.Linq.Expressions;
using DevExpress.EntityFrameworkCore.Security.MiddleTier.ClientServer;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using OutlookInspired.Module.BusinessObjects;
using static DevExpress.ExpressApp.Security.SecurityOperations;
using static DevExpress.ExpressApp.SystemModule.CurrentUserIdOperator;
using static DevExpress.Persistent.Base.SecurityPermissionState;
using static OutlookInspired.Module.Services.MailMergeExtensions;
using static OutlookInspired.Module.Services.ReportsExtensions;


namespace OutlookInspired.Module.Services{
    static class SecurityExtensions{
        const string DefaultRoleName = "Default";

        public static bool CanRead(this XafApplication application,Type objectType) 
            => ((WebApiMiddleTierClientSecurity)application.Security).IsGranted(new PermissionRequest(objectType, Read));

        public static PermissionPolicyRole EnsureRole(this IObjectSpace objectSpace,string roleName,Action<PermissionPolicyRole> initialize = null,bool isAdmin=false) 
        => objectSpace.EnsureObject<PermissionPolicyRole>(r => r.Name == "Administrators", role => {
            role.Name = roleName;
            role.IsAdministrative = isAdmin;
            initialize?.Invoke(role);
        });
        public static PermissionPolicyRole EnsureRole(this IObjectSpace objectSpace,EmployeeDepartment department) 
            => objectSpace.EnsureRole(department.ToString(), role => {
                switch (department){
                    case EmployeeDepartment.Sales:
                        role.AddSalesPermissions();
                        break;
                    case EmployeeDepartment.Engineering:
                        role.AddEngineeringPermissions();
                        break;
                    case EmployeeDepartment.Support:
                        role.AddSupportPermissions();
                        break;
                    case EmployeeDepartment.Shipping:
                        role.AddShippingPermissions();
                        break;
                    case EmployeeDepartment.HumanResources:
                        role.AddHRPermissions();
                        break;
                    case EmployeeDepartment.Management:
                        role.AddManagementPermissions();
                        break;
                    case EmployeeDepartment.IT:
                        role.AddITPermissions();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(department), department, null);
                }
            });

        
        private static void AddSalesPermissions(this PermissionPolicyRole role) 
            => new[]{typeof(ViewFilter), typeof(Customer),typeof(Crest), typeof(Order), typeof(Product),  typeof(Quote), }.AddCRUDAccess(role)
                .Concat(new []{typeof(Employee),typeof(EmployeeTask),typeof(Picture)}.AddReadAccess(role)).To<string>()
                .Concat(new[]{ "Order", "CustomerListView", "Product","Opportunities","EmployeeListView"}.AddNavigationAccess(role))
                .Finally(() => {
                    role.AddMailMergePermission(data => new[]{
                        MailMergeOrder, MailMergeOrderItem, FollowUp, MonthAward, ServiceExcellence, ThankYouNote
                        }.Contains(data.Name));
                    role.AddReportPermission(data => new[]{
                        RevenueAnalysis, RevenueReport, Sales, 
                            Contacts, LocationsReport, OrdersReport, ProductProfile, TopSalesPerson
                        }.Contains(data.DisplayName));
                })
                .Enumerate();

        private static void AddMailMergePermission(this PermissionPolicyRole role,Expression<Func<RichTextMailMergeData, bool>> lambda){
            role.AddTypePermission<RichTextMailMergeData>(FullAccess, Deny);
            role.AddObjectPermissionFromLambda(Read, lambda, Allow);
        }
        private static void AddReportPermission(this PermissionPolicyRole role,Expression<Func<ReportDataV2, bool>> lambda){
            role.AddTypePermission<ReportDataV2>(FullAccess, Deny);
            role.AddObjectPermissionFromLambda(Read, lambda, Allow);
        }

        [Obsolete("add an Updater for navigation items")]
        private static void AddHRPermissions(this PermissionPolicyRole role) 
            => new[]{typeof(ViewFilter), typeof(Employee),typeof(Picture),typeof(EmployeeTask),typeof(Evaluation),typeof(Probation) }.AddCRUDAccess(role)
                .Concat(new []{typeof(Customer),typeof(Crest)}.AddReadAccess(role)).To<string>()
                .Concat(new[]{ "EmployeeListView","Evaluation","CustomerListView"}.AddNavigationAccess(role))
                .Finally(() => {
                    role.AddMailMergePermission(data => new[]{
                        MailMergeOrder, MailMergeOrderItem, ProbationNotice,WelcomeToDevAV
                    }.Contains(data.Name));
                    role.AddReportPermission(data => new[]{ Contacts, LocationsReport }.Contains(data.DisplayName));
                })
                .Enumerate();


        private static void AddManagementPermissions(this PermissionPolicyRole role) 
            => new[]{typeof(ViewFilter), typeof(Employee),typeof(Picture),typeof(EmployeeTask),typeof(Evaluation) }.AddCRUDAccess(role)
                .Concat(new []{typeof(Customer),typeof(Crest),typeof(OrderItem)}.AddReadAccess(role)).To<string>()
                .Concat(new[]{ "EmployeeListView","Evaluation","CustomerListView"}.AddNavigationAccess(role))
                .Finally(() => {
                    role.AddMailMergePermission(data => new[]{
                        MailMergeOrder, MailMergeOrderItem, ServiceExcellence
                    }.Contains(data.Name));
                    role.AddReportPermission(data => new[]{
                        RevenueReport, Contacts, LocationsReport, TopSalesPerson
                    }.Contains(data.DisplayName));
                })
                .Enumerate();
        
        private static void AddSupportPermissions(this PermissionPolicyRole role) 
            => new[]{typeof(ViewFilter), typeof(Quote),typeof(QuoteItem) }.AddCRUDAccess(role)
                .Concat(new []{typeof(Customer),typeof(Crest)}.AddReadAccess(role)).To<string>()
                .Concat(new[]{"CustomerListView", "Opportunities"}.AddNavigationAccess(role))
                .Finally(() => {
                    role.AddMailMergePermission(data => new[]{
                        MailMergeOrder, MailMergeOrderItem, FollowUp, MonthAward, ServiceExcellence, ThankYouNote
                    }.Contains(data.Name));
                    role.AddReportPermission(data => new[]{
                        Sales, OrdersReport
                    }.Contains(data.DisplayName));
                })
                .Enumerate();
        
        private static void AddITPermissions(this PermissionPolicyRole role) 
            => new[]{ typeof(ViewFilter),typeof(Order),typeof(OrderItem) }.AddCRUDAccess(role)
                .Concat(new []{typeof(Customer),typeof(Crest)}.AddReadAccess(role)).To<string>()
                .Concat(new[]{"Order","CustomerListView"}.AddNavigationAccess(role))
                .Finally(() => {
                    role.AddMailMergePermission(data => new[]{
                        MailMergeOrder, MailMergeOrderItem, FollowUp, MonthAward, ServiceExcellence
                    }.Contains(data.Name));
                    role.AddReportPermission(data => new[]{
                        OrdersReport,FedExGroundLabel
                    }.Contains(data.DisplayName));
                })
                .Enumerate();
        
        private static void AddEngineeringPermissions(this PermissionPolicyRole role) 
            => new[]{typeof(ViewFilter), typeof(Employee),typeof(Picture),typeof(EmployeeTask),typeof(TaskAttachedFile) }.AddCRUDAccess(role)
                .Concat(new []{typeof(Customer),typeof(Crest)}.AddReadAccess(role)).To<string>()
                .Concat(new[]{ "EmployeeListView","CustomerListView"}.AddNavigationAccess(role))
                .Finally(() => {
                    role.AddMailMergePermission(data => new[]{
                        MailMergeOrder, MailMergeOrderItem,  ServiceExcellence, ProbationNotice,WelcomeToDevAV
                    }.Contains(data.Name));
                    role.AddReportPermission(data => new[]{
                        ProductProfile
                    }.Contains(data.DisplayName));
                    
                })
                .Enumerate();

        private static void AddShippingPermissions(this PermissionPolicyRole role){
            new[]{ typeof(ViewFilter), typeof(Order), typeof(OrderItem) }.AddCRUDAccess(role)
                .Concat(new[]{ typeof(Customer),typeof(Picture), typeof(Crest) }.AddReadAccess(role))
                .To<string>()
                .Concat(new[]{ "Order", "CustomerListView" }.AddNavigationAccess(role))
                .Finally(() => {
                    role.AddMailMergePermission(data => new[]{
                        MailMergeOrder, MailMergeOrderItem, FollowUp, MonthAward, ServiceExcellence
                    }.Contains(data.Name));
                    role.AddReportPermission(data => new[]{
                        OrdersReport, FedExGroundLabel
                    }.Contains(data.DisplayName));
                })
                .Enumerate();
        }

        private static IEnumerable<string> AddNavigationAccess(this IEnumerable<string> source, PermissionPolicyRole role)
            => source.Do(navigationId => role.AddNavigationPermission($"Application/NavigationItems/Items/Default/Items/{navigationId}", Allow));
            
        private static IEnumerable<Type> AddCRUDAccess(this IEnumerable<Type> source,PermissionPolicyRole role,SecurityPermissionState? state=Allow) 
            => source.AddAccess( role,CRUDAccess,state);
        private static IEnumerable<Type> AddReadAccess(this IEnumerable<Type> source,PermissionPolicyRole role,SecurityPermissionState? state=Allow) 
            => source.AddAccess( role,Read,state);

        private static IEnumerable<Type> AddAccess(this IEnumerable<Type> source, PermissionPolicyRole role,string operation,SecurityPermissionState? state=Allow) 
            => source.Do(type => role.AddTypePermission(type, operation, state));

        public static PermissionPolicyRole EnsureDefaultRole(this IObjectSpace objectSpace) 
            => objectSpace.EnsureRole(DefaultRoleName, defaultRole => {
                defaultRole.AddObjectPermissionFromLambda<ApplicationUser>(Read, cm => cm.ID == (Guid)CurrentUserId(), Allow);
                defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails", Allow);
                defaultRole.AddMemberPermissionFromLambda<ApplicationUser>(Write, "ChangePasswordOnFirstLogon", cm => cm.ID == (Guid)CurrentUserId(), Allow);
                defaultRole.AddMemberPermissionFromLambda<ApplicationUser>(Write, "StoredPassword", cm => cm.ID == (Guid)CurrentUserId(), Allow);
                defaultRole.AddTypePermissionsRecursively<PermissionPolicyRole>(Read, Deny);
                defaultRole.AddTypePermissionsRecursively<ModelDifference>(ReadWriteAccess, Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(ReadWriteAccess, Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifference>(Create, Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(Create, Allow);
            });


        [Obsolete("Notes #16")]
        public static ApplicationUser EnsureUser(this IObjectSpace objectSpace,string userName) 
            => objectSpace.EnsureObject<ApplicationUser>(u => u.UserName == userName, user => {
                user.UserName = userName;
                objectSpace.CommitChanges();
                ((ISecurityUserWithLoginInfo)user).CreateUserLoginInfo(SecurityDefaults.PasswordAuthentication,
                    objectSpace.GetKeyValueAsString(user));
            });

        public static PermissionPolicyRole FindRole(this IObjectSpace objectSpace, EmployeeDepartment department)
            => objectSpace.GetObjectsQuery<PermissionPolicyRole>().First(role => role.Name == department.ToString());
         
    }
}