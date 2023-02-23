using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.ApplicationBuilder;
using Microsoft.Extensions.DependencyInjection;
using SAASExtension.Interfaces;

namespace SAASExtensionBlazor.Classes {
    public class BlazorXAFApplicationBuilderWrapper : IXAFApplicationBuilderWrapper {
        IBlazorApplicationBuilder applicationBuilder;
        public BlazorXAFApplicationBuilderWrapper(IBlazorApplicationBuilder builder) {
            this.applicationBuilder = builder;
        }
        public IXAFApplicationBuilderWrapper AddBuildStep(Action<XafApplication> configureApplication) {
            applicationBuilder.AddBuildStep(configureApplication);
            return this;
        }
        public IXAFApplicationBuilderWrapper AddLogonController<TLogonController>(Action<TLogonController> configure = null)
             where TLogonController : Controller, new() {
            applicationBuilder.AddLogonController<TLogonController>(configure);
            return this;
        }
        public void AddModule<TModule>(Func<IServiceProvider, TModule> createModuleDelegate) where TModule : ModuleBase {
            applicationBuilder.Modules.Add(createModuleDelegate);
        }
        public void AddOptions<TOptions>() where TOptions : class {
            applicationBuilder.Get().AddOptions<TOptions>();
        }
        public IXAFApplicationBuilderWrapper AddService<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService {
            applicationBuilder.AddService<TService, TImplementation>();
            return this;
        }
        public IXAFApplicationBuilderWrapper ConfigureOptions<TOptions>(Action<TOptions> configureOptions)
             where TOptions : class {
            applicationBuilder.ConfigureOptions<TOptions>(configureOptions);
            return this;
        }
    }
}
