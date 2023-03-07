using DevExpress.ExpressApp.EFCore;
using Microsoft.EntityFrameworkCore;
using SAASExtension.Interfaces;

namespace SAASExtension.Services;
public class TenantNamesHelper<TType, TContext> : ITenantNamesHelper
    where TContext : DbContext
    where TType : class, ITenant {
    private IServiceProvider serviceProvider;
    private static Dictionary<string, string> connectionStrings;
    public TenantNamesHelper(IServiceProvider serviceProvider) {
        this.serviceProvider = serviceProvider;
    }
    public IDictionary<string, string> GetTenantNamesMap() {
        if (connectionStrings == null) {
            connectionStrings = new Dictionary<string, string>();
            using (var provider = new EFCoreObjectSpaceProvider<TContext>((builder, cs) => builder.UseServiceSQLServerOptions(serviceProvider))) {
                using (var objectSpace = provider.CreateObjectSpace()) {
                    foreach (ITenant tenant in objectSpace.CreateCollection(typeof(TType))) {
                        string connectionString = tenant is IConnectionString ? ((IConnectionString)tenant).ConnectionString : "";
                        connectionStrings.Add(tenant.Name, connectionString);
                    }
                }
            }
        }
        return connectionStrings;
    }
    public void ClearTenantNamesMap() {
        connectionStrings = null;
    }
}
