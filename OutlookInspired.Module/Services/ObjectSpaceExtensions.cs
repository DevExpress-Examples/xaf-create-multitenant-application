using System.Linq.Expressions;
using DevExpress.Data.Linq;
using DevExpress.Data.Linq.Helpers;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.EFCore;
using DevExpress.ExpressApp.EFCore.Internal;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using OutlookInspired.Module.BusinessObjects;
using static OutlookInspired.Module.DatabaseUpdate.Updater;
using static OutlookInspired.Module.Services.ReportsExtensions;
using Type = System.Type;

namespace OutlookInspired.Module.Services{
    internal static class ObjectSpaceExtensions{
        const string DefaultRoleName = "Default";

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
            => new[]{typeof(ViewFilter), typeof(Customer),typeof(Crest), typeof(Order), typeof(Product),  typeof(Quote), }.AddFullAccess(role)
                .Concat(new []{typeof(Employee),typeof(EmployeeTask),typeof(Picture)}.AddReadOnlyAccess(role)).To<string>()
                .Concat(new[]{ "Order", "CustomerListView", "Product","Opportunities","EmployeeListView"}.AddNavigationAccess(role))
                .Finally(() => {
                    role.AddMailMergePermission(data => new []{MailMergeOrder,MailMergeOrderItem,FollowUp,MonthAward,ServiceExcellence,ThankYouNote}.Contains(data.Name));
                    role.AddReportPermission(data => new[]{
                            RevenueAnalysis, RevenueReport, Sales, Contacts, LocationsReport, OrdersReport, ProductProfile, TopSalesPerson
                        }.Contains(data.DisplayName));
                })
                .Enumerate();

        private static void AddMailMergePermission(this PermissionPolicyRole role,Expression<Func<RichTextMailMergeData, bool>> lambda){
            role.AddTypePermission<RichTextMailMergeData>(SecurityOperations.FullAccess, SecurityPermissionState.Deny);
            role.AddObjectPermissionFromLambda(SecurityOperations.Read, lambda, SecurityPermissionState.Allow);
        }
        private static void AddReportPermission(this PermissionPolicyRole role,Expression<Func<ReportDataV2, bool>> lambda){
            role.AddTypePermission<ReportDataV2>(SecurityOperations.FullAccess, SecurityPermissionState.Deny);
            role.AddObjectPermissionFromLambda(SecurityOperations.Read, lambda, SecurityPermissionState.Allow);
        }

        [Obsolete("add an Updater for navigation items")]
        private static void AddHRPermissions(this PermissionPolicyRole role) 
            => new[]{typeof(ViewFilter), typeof(Employee),typeof(Picture),typeof(EmployeeTask),typeof(Evaluation),typeof(Probation) }.AddFullAccess(role)
                .Concat(new []{typeof(Customer),typeof(Crest)}.AddReadOnlyAccess(role)).To<string>()
                .Concat(new[]{ "EmployeeListView","Evaluation","CustomerListView"}.AddNavigationAccess(role))
                .Enumerate();

        private static void AddEngineeringPermissions(this PermissionPolicyRole role) 
            => new[]{typeof(ViewFilter), typeof(Employee),typeof(Picture),typeof(EmployeeTask),typeof(TaskAttachedFile) }.AddFullAccess(role)
                .Concat(new []{typeof(Customer),typeof(Crest)}.AddReadOnlyAccess(role)).To<string>()
                .Concat(new[]{ "EmployeeListView","CustomerListView"}.AddNavigationAccess(role))
                .Enumerate();

        private static void AddManagementPermissions(this PermissionPolicyRole role) 
            => new[]{typeof(ViewFilter), typeof(Employee),typeof(Picture),typeof(EmployeeTask),typeof(Evaluation) }.AddFullAccess(role)
                .Concat(new []{typeof(Customer),typeof(Crest)}.AddReadOnlyAccess(role)).To<string>()
                .Concat(new[]{ "EmployeeListView","Evaluation","CustomerListView"}.AddNavigationAccess(role))
                .Enumerate();
        private static void AddSupportPermissions(this PermissionPolicyRole role) 
            => new[]{typeof(ViewFilter), typeof(Quote),typeof(QuoteItem) }.AddFullAccess(role)
                .Concat(new []{typeof(Customer),typeof(Crest)}.AddReadOnlyAccess(role)).To<string>()
                .Concat(new[]{"CustomerListView", "Opportunities"}.AddNavigationAccess(role))
                .Enumerate();
        private static void AddITPermissions(this PermissionPolicyRole role) 
            => new[]{ typeof(ViewFilter),typeof(Order),typeof(OrderItem) }.AddFullAccess(role)
                .Concat(new []{typeof(Customer),typeof(Crest)}.AddReadOnlyAccess(role)).To<string>()
                .Concat(new[]{"Order","CustomerListView"}.AddNavigationAccess(role))
                .Enumerate();
        private static void AddShippingPermissions(this PermissionPolicyRole role) 
            => new[]{ typeof(ViewFilter),typeof(Order),typeof(OrderItem) }.AddFullAccess(role)
                .Concat(new []{typeof(Customer),typeof(Crest)}.AddReadOnlyAccess(role)).To<string>()
                .Concat(new[]{"Order","CustomerListView"}.AddNavigationAccess(role))
                .Enumerate();

        private static IEnumerable<string> AddNavigationAccess(this IEnumerable<string> source, PermissionPolicyRole role)
            => source.Do(navigationId => role.AddNavigationPermission($"Application/NavigationItems/Items/Default/Items/{navigationId}", SecurityPermissionState.Allow));
            
        private static IEnumerable<Type> AddFullAccess(this IEnumerable<Type> source,PermissionPolicyRole role) 
            => source.AddAccess( role,SecurityOperations.FullAccess);
        private static IEnumerable<Type> AddReadOnlyAccess(this IEnumerable<Type> source,PermissionPolicyRole role) 
            => source.AddAccess( role,SecurityOperations.ReadOnlyAccess);

        private static IEnumerable<Type> AddAccess(this IEnumerable<Type> source, PermissionPolicyRole role,string operation) 
            => source.Do(type => role.AddTypePermissionsRecursively(type, operation, SecurityPermissionState.Allow));

        public static PermissionPolicyRole EnsureDefaultRole(this IObjectSpace objectSpace) 
            => objectSpace.EnsureRole(DefaultRoleName, defaultRole => {
                defaultRole.AddObjectPermissionFromLambda<ApplicationUser>(SecurityOperations.Read, cm => cm.ID == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
                defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails", SecurityPermissionState.Allow);
                defaultRole.AddMemberPermissionFromLambda<ApplicationUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", cm => cm.ID == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
                defaultRole.AddMemberPermissionFromLambda<ApplicationUser>(SecurityOperations.Write, "StoredPassword", cm => cm.ID == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
                defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);
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
        

        public static T EnsureObject<T>(this IObjectSpace objectSpace,
            Expression<Func<T, bool>> criteriaExpression = null, Action<T> initialize = null, Action<T> update = null,
            bool inTransaction = false) where T : class{
            var o = objectSpace.FirstOrDefault(criteriaExpression??(arg =>true) ,inTransaction);
            if (o != null) {
                update?.Invoke(o);
                return o;
            }
            var ensureObject = objectSpace.CreateObject<T>();
            initialize?.Invoke(ensureObject);
            update?.Invoke(ensureObject);
            return ensureObject;
        }
        
        public static RichTextMailMergeData NewMailMergeData(this IObjectSpace objectSpace, string name ,Type dataType,byte[] bytes ){
            var richTextMailMergeData = objectSpace.CreateObject<RichTextMailMergeData>();
            richTextMailMergeData.Name = name;
            richTextMailMergeData.Template = bytes;
            richTextMailMergeData.DataType = dataType;
            return richTextMailMergeData;
        }
        
        public static EntityServerModeSource NewEntityServerModeSource(this EFCoreObjectSpace objectSpace,Type objectType,string criteria){
            // return new EFCoreServerCollection(objectSpace, objectType, CriteriaOperator.Parse(criteria),
                // new List<SortProperty>());
            return new EntityServerModeSource{
            KeyExpression = objectSpace.TypesInfo.FindTypeInfo(objectType).KeyMember.Name,
            QueryableSource = objectSpace.Query(objectType, criteria)
            };
        }

        public static IQueryable Query(this EFCoreObjectSpace objectSpace,Type objectType, string criteria){
            var queryable = objectSpace.GetQueryable(objectType.FullName);
            return criteria != null ? queryable.AppendWhere(
                    new CriteriaToEFCoreExpressionConverter(queryable.Provider.GetType(), objectSpace.GetQueryable),
                    objectSpace.ParseCriteria(criteria)) : queryable;
        }
        
        public static IEnumerable<IObjectSpace> YieldAll(this IObjectSpace objectSpace)
            => objectSpace is not CompositeObjectSpace compositeObjectSpace
                ? objectSpace.YieldItem()
                : objectSpace.YieldItem().Concat(compositeObjectSpace.AdditionalObjectSpaces);

        
        public static void DeleteObject(this IObjectSpace objectSpace, object obj)
            => objectSpace.Delete(objectSpace.GetObject(obj));
        
        public static bool Any<T>(this IObjectSpace objectSpace) 
            => objectSpace.GetObjectsQuery<T>().Any();
        public static int Count<T>(this IObjectSpace objectSpace, Expression<Func<T, bool>> expression=null)
            => objectSpace.GetObjectsQuery<T>().Where(expression??(arg =>true) ).Count();
        public static T FindObject<T>(this IObjectSpace objectSpace, Expression<Func<T,bool>> expression,bool inTransaction=false) 
            => objectSpace.GetObjectsQuery<T>(inTransaction).FirstOrDefault(expression);

        public static RichTextMailMergeData MailMergeData(this IObjectSpace space,string name) 
            => space.FindObject<RichTextMailMergeData>(data => data.Name==name);
    }
}