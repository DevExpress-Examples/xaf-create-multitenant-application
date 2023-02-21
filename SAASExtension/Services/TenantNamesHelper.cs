using DevExpress.ExpressApp.Core;
using SAASExtension.BusinessObjects;
using SAASExtension.Interfaces;
using System.Collections.Generic;

namespace SAASExtension.Services;
public class TenantNamesHelper<TType> : ITenantNamesHelper
    where TType : class, ITenant {
    private INonSecuredObjectSpaceFactory factory;
    private bool isCalculating = false;
    private Dictionary<string, string> connectionStrings;
    public TenantNamesHelper(INonSecuredObjectSpaceFactory factory) {
        this.factory = factory;
    }
    public IDictionary<string, string> GetTenantNamesMap() {
        if (isCalculating) {
            return connectionStrings;
        }
        try {
            isCalculating = true;
            connectionStrings = new Dictionary<string, string>();
            using var objectSpace = factory.CreateNonSecuredObjectSpace<TType>();
            foreach (ITenant tenant in objectSpace.CreateCollection(typeof(TType))) {
                string connectionString = tenant is IConnectionString ? ((IConnectionString)tenant).ConnectionString : "";
                connectionStrings.Add(tenant.Name, connectionString);
            }
        } finally {
            isCalculating = false;
        }
        return connectionStrings;
    }
}

