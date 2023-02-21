using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.Security;
using SAASExtension.BusinessObjects;
using SAASExtension.Interfaces;
using SAASExtension.Security;

namespace SAASExtension.Services {
    public class UserTenantNamesHelper : ITenantNamesHelper {
        readonly ILogonParameterProvider logonParameterProvider;
        private INonSecuredObjectSpaceFactory factory;
        Dictionary<string, string> connectionStrings = new Dictionary<string, string>();
        public UserTenantNamesHelper(INonSecuredObjectSpaceFactory factory, ILogonParameterProvider logonParameterProvider) {
            this.logonParameterProvider = logonParameterProvider;
            this.factory = factory;
        }
        public IDictionary<string, string> GetTenantNamesMap() {
            var logonParameters = logonParameterProvider.GetLogonParameters<IAuthenticationStandardLogonParameters>();
            if (logonParameters.UserName != null) {
                connectionStrings.Clear();
                using var objectSpace = factory.CreateNonSecuredObjectSpace<SAASPermissionPolicyUserWithTenants>();
                SAASPermissionPolicyUserWithTenants user = objectSpace.FindObject<SAASPermissionPolicyUserWithTenants>(CriteriaOperator.Parse($"[UserName] = '{logonParameters.UserName}'"));
                if (user != null) {
                    foreach (TenantWithConnectionStringWithUsersObject tenant in user.Tenants) {
                        connectionStrings.Add(tenant.Name, tenant.ConnectionString);
                    }
                }
            }
            return connectionStrings;
        }
    }
}
