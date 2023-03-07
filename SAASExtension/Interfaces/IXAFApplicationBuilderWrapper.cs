using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.ApplicationBuilder.Internal;
using DevExpress.ExpressApp.MiddleTier;
using Microsoft.EntityFrameworkCore;
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
        void AddSinleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;
        void RemoveService<TService>();
        void AddTenantObjectObjectSpaceProvider<TDbContext>(bool removeAfterLogon = true) where TDbContext : DbContext;
        void SetupMultipleDatabaseProviders(Action<object> setupProviders);
    }
}
