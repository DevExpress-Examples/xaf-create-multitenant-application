using System.Reflection;

namespace SAASExtension.Options {
    public class ModelDifferencesOptions {
        public Assembly Assembly { get; set; }
        public string ServiceModelResourceName { get; set; } = "ServiceModel";
        public string ProductionModelResourceName { get; set; } = "ProductionModel";
        public bool UseTenantSpecificModel { get; set; } = true;
    }
}
