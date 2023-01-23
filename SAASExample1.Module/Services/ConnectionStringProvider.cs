using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.Security;
using Microsoft.Extensions.Configuration;
using SAASExample1.Module.Interfaces;
using SAASExample1.Module.Services;
using System.Linq;

namespace SAASExample1.Module.Services;
public class ConnectionStringProvider : IConnectionStringProvider {
    readonly ILogonParameterProvider logonParameterProvider;
    readonly ICompanyNamesHelper companyNamesHelper;
    readonly IConfigurationConnectionStringProvider provider;

    public ConnectionStringProvider(ILogonParameterProvider logonParameterProvider, ICompanyNamesHelper companyNamesHelper, IConfigurationConnectionStringProvider provider) {
        this.logonParameterProvider = logonParameterProvider;
        this.companyNamesHelper = companyNamesHelper;
        this.provider = provider;
    }

    public string GetConnectionString() {
        //Configure the connection string based on logon parameter values.
        ICompany logonParameters = logonParameterProvider.GetLogonParameters<ICompany>();
        string? targeCompanyName = logonParameters.CompanyName?.Name;
        if (targeCompanyName != null) {
            IDictionary<string, string> map = companyNamesHelper.GetCompanyNamesMap();
            string connectionString;
            if (map.TryGetValue(targeCompanyName, out connectionString)) {
                return connectionString;
            }
        }
        return provider.GetConnectionString();
    }
}

