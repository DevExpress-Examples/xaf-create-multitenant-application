using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Services.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAASExtension.Services {
    public class ExtraObjectSpaceProviders : IExtraObjectSpaceProviders {
        private IServiceProvider serviceProvider;
        public ExtraObjectSpaceProviders(IServiceProvider serviceProvider) {
            this.serviceProvider = serviceProvider;
        }
        public Dictionary<Func<IObjectSpaceProvider>, Func<IServiceProvider, bool>> Factories { get ; set ; } = new Dictionary<Func<IObjectSpaceProvider>, Func<IServiceProvider, bool>>();

        public IEnumerable<IObjectSpaceProvider> CreateObjectSpaceProviders() {
            List<IObjectSpaceProvider> result = new List<IObjectSpaceProvider>();
            foreach (var factory in Factories.Keys) {
                if (Factories[factory].Invoke(serviceProvider)) {
                    result.Add(factory.Invoke());
                }
            }
            return result;
        }
    }
}
