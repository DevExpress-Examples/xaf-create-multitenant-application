using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.ApplicationBuilder.Internal;
using DevExpress.ExpressApp.MiddleTier;
using DevExpress.ExpressApp.Win.ApplicationBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAASExtension;
using SAASExtension.Interfaces;
using SAASExtension.Services;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace SAASExtensionWin.Classes {
    internal class WinXAFApplicationBuilderWrapper : IXAFApplicationBuilderWrapper {
        IWinApplicationBuilder applicationBuilder;
        public WinXAFApplicationBuilderWrapper(IWinApplicationBuilder builder) {
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
            //TODO Replace when we will move to IConfiguration
            if (typeof(TImplementation) == typeof(ConfigurationConnectionStringProvider) && (typeof(TService) == typeof(IConfigurationConnectionStringProvider))) {
                applicationBuilder.AddService<IConfigurationConnectionStringProvider, ConfigurationManagerConnectionStringProvider>();
            } else {
                applicationBuilder.AddService<TService, TImplementation>();
            }
            return this;
        }

        public void AddSinleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService {
            applicationBuilder.Get().TryAddSingleton<TService, TImplementation>();
        }

        public void AddTenantObjectObjectSpaceProvider<TDbContext>(bool removeAfterLogon = true) where TDbContext : DbContext {
            if (removeAfterLogon) {
                List<Func<XafApplication, CreateCustomObjectSpaceProviderEventArgs, IObjectSpaceProvider>> createObjectSpaceProviderDelegates = new List<Func<XafApplication, CreateCustomObjectSpaceProviderEventArgs, IObjectSpaceProvider>>();
                EventHandler<CustomAddDelegateEventArgs> eventHandler = (s, e) => {
                    createObjectSpaceProviderDelegates.Add(e.CreateObjectSpaceProviderDelegate);
                    e.Handled = true;
                };
                applicationBuilder.ObjectSpaceProviders.CustomAddDelegate += eventHandler;
                applicationBuilder.ObjectSpaceProviders
                                .AddSecuredEFCore().WithDbContext<TDbContext>((application, options) => {
                                    options.UseServiceSQLServerOptions(application.ServiceProvider);
                                });
                applicationBuilder.ObjectSpaceProviders.CustomAddDelegate -= eventHandler;
                AddBuildStep(application => {
                    ApplicationExtensions.AddServiceDatabaseProviders(application, createObjectSpaceProviderDelegates);
                });
            } else {
                applicationBuilder.ObjectSpaceProviders
                .AddSecuredEFCore().WithDbContext<TDbContext>((application, options) => {
                    options.UseServiceSQLServerOptions(application.ServiceProvider);
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
            List<Func<XafApplication, CreateCustomObjectSpaceProviderEventArgs, IObjectSpaceProvider>> createObjectSpaceProviderDelegates = new List<Func<XafApplication, CreateCustomObjectSpaceProviderEventArgs, IObjectSpaceProvider>>();
            EventHandler<CustomAddDelegateEventArgs> eventHandler = (s, e) => {
                createObjectSpaceProviderDelegates.Add(e.CreateObjectSpaceProviderDelegate);
                e.Handled = true;
            };
            applicationBuilder.ObjectSpaceProviders.CustomAddDelegate += eventHandler;
            setupProviders.Invoke(applicationBuilder);
            applicationBuilder.ObjectSpaceProviders.CustomAddDelegate -= eventHandler;
            AddBuildStep(application => {
                ApplicationExtensions.AddMultipleDatabaseProviders(application, createObjectSpaceProviderDelegates);
            });
        }
    }
}