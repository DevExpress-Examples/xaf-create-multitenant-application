using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.ApplicationBuilder.Internal;
using DevExpress.ExpressApp.Blazor.ApplicationBuilder;
using DevExpress.ExpressApp.MiddleTier;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAASExtension;
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

        public void AddSinleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService {
            applicationBuilder.Get().TryAddSingleton<TService, TImplementation>();
        }

        public void AddTenantObjectObjectSpaceProvider<TDbContext>(bool removeAfterLogon = true) where TDbContext : DbContext {
            if (removeAfterLogon) {
                List<Func<IServiceProvider, IObjectSpaceProvider>> createObjectSpaceProviderDelegates = new List<Func<IServiceProvider, IObjectSpaceProvider>>();
                EventHandler<CustomAddFactoryDelegateEventArgs> eventHandler = (s, e) => {
                    createObjectSpaceProviderDelegates.Add(e.CreateObjectSpaceProviderFactoryDelegate);
                    e.Handled = true;
                };
                applicationBuilder.ObjectSpaceProviders.CustomAddFactoryDelegate += eventHandler;
                applicationBuilder.ObjectSpaceProviders
                                .AddSecuredEFCore().WithDbContext<TDbContext>((serviceProvider, options) => {
                                    options.UseServiceSQLServerOptions(serviceProvider);
                                });
                applicationBuilder.ObjectSpaceProviders.CustomAddFactoryDelegate -= eventHandler;
                AddBuildStep(application => {
                    ApplicationExtensions.AddServiceDatabaseProviders(application, createObjectSpaceProviderDelegates);
                });
            } else {
                applicationBuilder.ObjectSpaceProviders
                               .AddSecuredEFCore().WithDbContext<TDbContext>((serviceProvider, options) => {
                                   options.UseServiceSQLServerOptions(serviceProvider);
                               });
            }
        }

        public IXAFApplicationBuilderWrapper ConfigureOptions<TOptions>(Action<TOptions> configureOptions)
             where TOptions : class {
            applicationBuilder.ConfigureOptions<TOptions>(configureOptions);
            return this;
        }

        public void RemoveService<TService>() {
            IServiceCollection services = applicationBuilder.Get();
            var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IService));
            services.Remove(serviceDescriptor);
        }

        public void SetupMultipleDatabaseProviders(Action<object> setupProviders) {
            List<Func<IServiceProvider, IObjectSpaceProvider>> createObjectSpaceProviderDelegates = new List<Func<IServiceProvider, IObjectSpaceProvider>>();
            EventHandler<CustomAddFactoryDelegateEventArgs> eventHandler = (s, e) => {
                createObjectSpaceProviderDelegates.Add(e.CreateObjectSpaceProviderFactoryDelegate);
                e.Handled = true;
            };
            applicationBuilder.ObjectSpaceProviders.CustomAddFactoryDelegate += eventHandler;
            setupProviders.Invoke(applicationBuilder);
            applicationBuilder.ObjectSpaceProviders.CustomAddFactoryDelegate -= eventHandler;
            AddBuildStep(application => {
                ApplicationExtensions.AddMultipleDatabaseProviders(application, createObjectSpaceProviderDelegates);
            });
        }
    }
}
