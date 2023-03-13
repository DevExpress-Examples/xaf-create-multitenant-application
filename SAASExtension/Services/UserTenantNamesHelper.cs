using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.EFCore;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using Microsoft.EntityFrameworkCore;
using SAASExtension.BusinessObjects;
using SAASExtension.Interfaces;
using SAASExtension.Security;

namespace SAASExtension.Services {
    public class UserTenantNamesHelper<TContext> : ITenantNamesHelper
        where TContext : DbContext {
        readonly ILogonParameterProvider logonParameterProvider;
        private IServiceProvider serviceProvider;
        static Dictionary<string, Dictionary<string, string>> connectionStrings = new Dictionary<string, Dictionary<string, string>>();
        public UserTenantNamesHelper(IServiceProvider serviceProvider, ILogonParameterProvider logonParameterProvider) {
            this.logonParameterProvider = logonParameterProvider;
            this.serviceProvider = serviceProvider;
        }
        public void ClearTenantNamesMap() {
            connectionStrings.Clear();
        }
        public IDictionary<string, string> GetTenantNamesMap() {
            Dictionary<string, string> result = new Dictionary<string, string>();
            var logonParameters = logonParameterProvider.GetLogonParameters<IAuthenticationStandardLogonParameters>();
            if (logonParameters.UserName != null) {
                if(!connectionStrings.TryGetValue(logonParameters.UserName, out result)) {
                    result = new Dictionary<string, string>();
                    using (var provider = new EFCoreObjectSpaceProvider<TContext>((builder, cs) => builder.UseServiceSQLServerOptions(serviceProvider))) {
                        using (var objectSpace = provider.CreateObjectSpace()) {
                            SAASPermissionPolicyUserWithTenants user = objectSpace.FindObject<SAASPermissionPolicyUserWithTenants>(CriteriaOperator.Parse($"[UserName] = '{logonParameters.UserName}'"));
                            if (user != null) {
                                foreach (TenantObjectWithUsers tenant in user.Tenants) {
                                    result[tenant.Name] = tenant.ConnectionString;
                                }
                                connectionStrings[logonParameters.UserName] = result;
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
