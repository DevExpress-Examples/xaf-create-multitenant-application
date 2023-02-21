using DevExpress.ExpressApp;
using DevExpress.Persistent.BaseImpl.EF;
using SAASExtension.Interfaces;
using System.Reflection;

namespace SAASExtension.Services {
    public class TenantModelDifferenceStore : ITenantModelDifferenceStore {
        public ModelStoreBase GetTenantModelDifferenceStore(XafApplication application, string tenantName) {
            return new ModelDifferenceDbStore(application, typeof(ModelDifference), true, tenantName);
        }
    }
}
