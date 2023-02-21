using DevExpress.ExpressApp;

namespace SAASExtension.Interfaces {
    public interface ITenantModelDifferenceStore {
        ModelStoreBase GetTenantModelDifferenceStore(XafApplication application, string tenantName);
    }
}
