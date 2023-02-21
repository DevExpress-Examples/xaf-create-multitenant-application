using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.Security;
using Microsoft.Extensions.Configuration;
using SAASExtension.Interfaces;
using System.Linq;

namespace SAASExtension.Services;
public class ConnectionStringProvider : IConnectionStringProvider {
    readonly ILogonParameterProvider logonParameterProvider;
    readonly ITenantNamesHelper tenantNamesHelper;
    readonly IConfigurationConnectionStringProvider provider;

    public ConnectionStringProvider(ILogonParameterProvider logonParameterProvider, ITenantNamesHelper tenantNamesHelper, IConfigurationConnectionStringProvider provider) {
        this.logonParameterProvider = logonParameterProvider;
        this.tenantNamesHelper = tenantNamesHelper;
        this.provider = provider;
    }

    public string GetConnectionString() {
        ITenantName logonParameters = logonParameterProvider.GetLogonParameters<ITenantName>();
        string? targetTenantName = logonParameters?.TenantName;
        if (targetTenantName != null) {
            IDictionary<string, string> map = tenantNamesHelper.GetTenantNamesMap();
            string connectionString;
            if (map.TryGetValue(targetTenantName, out connectionString)) {
                return connectionString;
            }
        }
        return provider.GetConnectionString();
    }
}

