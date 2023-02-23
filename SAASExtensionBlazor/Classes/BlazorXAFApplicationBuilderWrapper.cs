using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.ApplicationBuilder;
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
