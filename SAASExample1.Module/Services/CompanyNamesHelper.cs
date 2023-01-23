using DevExpress.ExpressApp.Core;
using Microsoft.Extensions.Configuration;
using SAASExample1.Module.BusinessObjects;
using System.Collections.Generic;

namespace SAASExample1.Module.Services;
public class CompanyNamesHelper : ICompanyNamesHelper {
    private INonSecuredObjectSpaceFactory factory;
    Dictionary<string, string> connectionStrings;
    public CompanyNamesHelper(INonSecuredObjectSpaceFactory factory) {
        this.factory = factory;
    }
    public IDictionary<string, string> GetCompanyNamesMap() {
        if (connectionStrings == null) {
            connectionStrings = new Dictionary<string, string>();
            using var objectSpace = factory.CreateNonSecuredObjectSpace<Company>();
            var businessObject = objectSpace.CreateCollection(typeof(Company));
            foreach (Company company in objectSpace.CreateCollection(typeof(Company))) {
                connectionStrings.Add(company.Name, company.ConnectionString);
            }
        }
        return connectionStrings;
    }
}

