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

namespace SAASExtensionBlazor {
    public static class BlazorApplicationBuilderExtensions {
        public static ISAASApplicationBuilder MakeSAAS(this IBlazorApplicationBuilder builder, Action<PublicExtensionModuleOptions> configureOptions = null) {
            var result = new SAASApplicationBuilder(builder);
            builder.Get().AddOptions<InternalExtensionModuleOptions>();
            builder.Get().AddOptions<PublicExtensionModuleOptions>();
            builder.Modules.Add((serviceProvider) => {
                InternalExtensionModuleOptions internalOptions = serviceProvider.GetRequiredService<IOptions<InternalExtensionModuleOptions>>().Value;
                PublicExtensionModuleOptions publicOptions = serviceProvider.GetRequiredService<IOptions<PublicExtensionModuleOptions>>().Value;
                ApplicationExtensions.ParsePublicOptions(internalOptions, publicOptions);
                ExtensionModule extensionModule = new ExtensionModule(internalOptions, publicOptions);
                return extensionModule;
            });
            ConfigureOptions<PublicExtensionModuleOptions>(builder, configureOptions);
            return result;
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