using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.Win.ApplicationBuilder;
using DevExtreme.AspNet.Data.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SAASExtension;
using SAASExtension.BusinessObjects;
using SAASExtension.Controllers;
using SAASExtension.Interfaces;
using SAASExtension.Modules;
using SAASExtension.Options;
using SAASExtension.Services;
using SAASExtensionWin.Classes;
using System.Reflection;

namespace SAASExtensionWin {
    public static class WinApplicationBuilderExtensions {
        public static ISAASApplicationBuilder MakeSAAS(this IWinApplicationBuilder builder, Action<PublicExtensionModuleOptions> configureOptions = null) {
            var result = new SAASApplicationBuilder(new WinXAFApplicationBuilderWrapper(builder));
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
        public static IWinApplicationBuilder AddService<TService, TImplementation>(this IWinApplicationBuilder builder)
            where TService : class
            where TImplementation : class, TService {
            IServiceCollection services = builder.Get();
            services.AddScoped<TService, TImplementation>();
            return builder;
        }
        public static IWinApplicationBuilder ConfigureOptions<TOptions>(this IWinApplicationBuilder builder, Action<TOptions> configureOptions)
            where TOptions : class {
            IServiceCollection services = builder.Get();
            services.Configure<TOptions>(configureOptions);
            return builder;
        }
        public static IWinApplicationBuilder AddSAASTenantModelDifferenceStore(this IWinApplicationBuilder builder, Action<ModelDifferencesOptions> setupOptions) {
            return AddSAASTenantModelDifferenceStore<TenantModelDifferenceStore>(builder, setupOptions);
        }
        public static IWinApplicationBuilder AddSAASTenantModelDifferenceStore<TTenantModelDifferenceStore>(this IWinApplicationBuilder builder, Action<ModelDifferencesOptions> setupOptions)
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
        public static IWinApplicationBuilder AddLogonController<TLogonController>(this IWinApplicationBuilder builder, Action<TLogonController> configure = null)
            where TLogonController : Controller, new() {
            builder.AddBuildStep(application => {
                ApplicationExtensions.CreateCustomLogonWindowController<TLogonController>(application, configure);
            });
            return builder;
        }
    }
}