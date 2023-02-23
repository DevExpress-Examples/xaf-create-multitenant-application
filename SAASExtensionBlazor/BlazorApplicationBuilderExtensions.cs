using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.ApplicationBuilder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SAASExtension;
using SAASExtension.Controllers;
using SAASExtension.Interfaces;
using SAASExtension.Modules;
using SAASExtension.Options;
using SAASExtension.Services;
using SAASExtensionBlazor.Classes;

namespace SAASExtensionBlazor {
    public static class BlazorApplicationBuilderExtensions {
        public static ISAASApplicationBuilder MakeSAAS(this IBlazorApplicationBuilder builder, Action<PublicExtensionModuleOptions> configureOptions = null) {
            return new SAASApplicationBuilder(new BlazorXAFApplicationBuilderWrapper(builder), configureOptions);
        }
        public static IBlazorApplicationBuilder AddService<TService, TImplementation>(this IBlazorApplicationBuilder builder)
            where TService : class
            where TImplementation : class, TService {
            IServiceCollection services = builder.Get();
            services.AddScoped<TService, TImplementation>();
            return builder;
        }
        public static IBlazorApplicationBuilder ConfigureOptions<TOptions>(this IBlazorApplicationBuilder builder, Action<TOptions> configureOptions)
            where TOptions : class {
            IServiceCollection services = builder.Get();
            services.Configure<TOptions>(configureOptions);
            return builder;
        }
        public static IBlazorApplicationBuilder AddSAASTenantModelDifferenceStore(this IBlazorApplicationBuilder builder, Action<ModelDifferencesOptions> setupOptions) {
            return AddSAASTenantModelDifferenceStore<TenantModelDifferenceStore>(builder, setupOptions);
        }
        public static IBlazorApplicationBuilder AddSAASTenantModelDifferenceStore<TTenantModelDifferenceStore>(this IBlazorApplicationBuilder builder, Action<ModelDifferencesOptions> setupOptions)
            where TTenantModelDifferenceStore : class, ITenantModelDifferenceStore {
            ModelDifferencesOptions options = new ModelDifferencesOptions();
            setupOptions.Invoke(options);
            if (options.UseTenantSpecificModel) {
                builder.AddService<ITenantModelDifferenceStore, TTenantModelDifferenceStore>();
            }
            builder.AddBuildStep(application => {
                ApplicationExtensions.AddExtraDiffStore(application, options.Assembly, options.ServiceModelResourceName, options.ProductionModelResourceName);
            });
            return builder;
        }
        public static IBlazorApplicationBuilder AddLogonController<TLogonController>(this IBlazorApplicationBuilder builder, Action<TLogonController> configure = null)
            where TLogonController : Controller, new() {
            builder.AddBuildStep(application => {
                ApplicationExtensions.CreateCustomLogonWindowController<TLogonController>(application, configure);
            });
            return builder;
        }
    }
}