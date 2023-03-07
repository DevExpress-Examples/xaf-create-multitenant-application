using DevExpress.ExpressApp.Services.Core;
using Microsoft.Extensions.DependencyInjection;
using SAASExtension.Interfaces;
using System.Collections.Concurrent;

namespace SAASExtension.Services {
    public class MultipleDatabaseCheckCompatibilityHelper : ICheckCompatibilityHelper {
        IServiceProvider serviceProvider;
        private static ConcurrentDictionary<string, bool> isCompatibilityChecked = new ConcurrentDictionary<string, bool>();
        public MultipleDatabaseCheckCompatibilityHelper(IServiceProvider serviceProvider) {
            this.serviceProvider = serviceProvider;
        }
        public bool IsCompatibilityChecked {
            get {
                return isCompatibilityChecked.ContainsKey(serviceProvider.GetRequiredService<IConnectionStringProvider>().GetConnectionString());
            }
            set {
                isCompatibilityChecked.TryAdd(serviceProvider.GetRequiredService<IConnectionStringProvider>().GetConnectionString(), value);
            }
        }
    }
}
