using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ApplicationBuilder;
using Microsoft.Extensions.DependencyInjection;

namespace SAASExtension.Interfaces {
    public interface IXAFApplicationBuilderWrapper {
        IXAFApplicationBuilderWrapper ConfigureOptions<TOptions>(Action<TOptions> configureOptions) where TOptions : class;
        IXAFApplicationBuilderWrapper AddService<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;
        IXAFApplicationBuilderWrapper AddLogonController<TLogonController>(Action<TLogonController> configure = null) where TLogonController : Controller, new();
        IXAFApplicationBuilderWrapper AddBuildStep(Action<XafApplication> configureApplication);
        void AddOptions<TOptions>() where TOptions : class;
        void AddModule<TModule>(Func<IServiceProvider, TModule> createModuleDelegate) where TModule : ModuleBase;
    }
}
