using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SAASExtension.BusinessObjects;
using SAASExtension.Controllers;
using SAASExtension.Interfaces;
using SAASExtension.Modules;
using SAASExtension.Options;
using SAASExtension.Security;
using SAASExtension.Services;
using System;
using System.ComponentModel;
using System.Reflection;

namespace SAASExtension {
    public class SAASApplicationBuilder : ISAASApplicationBuilder {
        IXAFApplicationBuilderWrapper wrapper;
        public SAASApplicationBuilder(IXAFApplicationBuilderWrapper wrapper, Action<PublicExtensionModuleOptions> configureOptions = null) {
            this.wrapper = wrapper;
            wrapper.AddOptions<InternalExtensionModuleOptions>();
            wrapper.AddOptions<PublicExtensionModuleOptions>();
            wrapper.AddModule((serviceProvider) => {
                InternalExtensionModuleOptions internalOptions = serviceProvider.GetRequiredService<IOptions<InternalExtensionModuleOptions>>().Value;
                PublicExtensionModuleOptions publicOptions = serviceProvider.GetRequiredService<IOptions<PublicExtensionModuleOptions>>().Value;
                ApplicationExtensions.ParsePublicOptions(internalOptions, publicOptions);
                ExtensionModule extensionModule = new ExtensionModule(internalOptions, publicOptions);
                return extensionModule;
            });
            wrapper.ConfigureOptions<PublicExtensionModuleOptions>(configureOptions);
            wrapper.ConfigureOptions<InternalExtensionModuleOptions>(o => {
                o.DeclaredExportedTypes.Add(typeof(TenantNameHolder));
            });
        }
        public IMultipleDatabaseBuilder MultipleDatabases() {
            return MultipleDatabases<ConnectionStringProvider, ConfigurationConnectionStringProvider>();
        }
        public IMultipleDatabaseBuilder MultipleDatabases<TConnectionStringProvider, TConfigurationConnectionStringProvider>()
            where TConnectionStringProvider : class, IConnectionStringProvider
            where TConfigurationConnectionStringProvider : class, IConfigurationConnectionStringProvider {
            wrapper.AddService<IConnectionStringProvider, TConnectionStringProvider>();
            wrapper.AddService<IConfigurationConnectionStringProvider, TConfigurationConnectionStringProvider>();
            return new MultipleDatabaseBuilder(wrapper);
        }
        public IOneDatabaseBuilder OneDatabase() {
            return new OneDatabaseBuilder(wrapper);
        }
    }
    public class MultipleDatabaseBuilder : IMultipleDatabaseBuilder {
        IXAFApplicationBuilderWrapper wrapper;
        public MultipleDatabaseBuilder(IXAFApplicationBuilderWrapper wrapper) {
            this.wrapper = wrapper;
        }
        public IMultipleDatabaseLogInFirstBuilder LogInFirst() {
            return LogInFirst<UserTenantNamesHelper>();
        }
        public IMultipleDatabaseLogInFirstBuilder LogInFirst<TTenantNamesHelper>()
            where TTenantNamesHelper : class, ITenantNamesHelper {
            wrapper.AddService<ITenantNamesHelper, TTenantNamesHelper>();
            wrapper.ConfigureOptions<InternalExtensionModuleOptions>(o => {
                o.DeclaredExportedTypes.Add(typeof(TenantWithConnectionStringWithUsersObject));
                o.DeclaredExportedTypes.Add(typeof(TenantWithConnectionStringObject));
                o.DeclaredExportedTypes.Add(typeof(TenantObject));
            });

            return new MultipleDatabaseLogInFirstBuilder(wrapper);
        }
        public IMultipleDatabaseTenantFirstBuilder TenantFirst() {
            return TenantFirst<TenantNamesHelper<TenantWithConnectionStringObject>>();
        }
        public IMultipleDatabaseTenantFirstBuilder TenantFirst<TTenantNamesHelper>()
            where TTenantNamesHelper : class, ITenantNamesHelper {
            wrapper.AddService<ITenantNamesHelper, TTenantNamesHelper>();
            wrapper.ConfigureOptions<InternalExtensionModuleOptions>(o => {
                o.DeclaredExportedTypes.Add(typeof(TenantWithConnectionStringObject));
                o.DeclaredExportedTypes.Add(typeof(TenantObject));
            });
            return new MultipleDatabaseTenantFirstBuilder(wrapper);
        }
    }
    public class OneDatabaseBuilder : IOneDatabaseBuilder {
        IXAFApplicationBuilderWrapper wrapper;
        public OneDatabaseBuilder(IXAFApplicationBuilderWrapper wrapper) {
            this.wrapper = wrapper;
        }
        public IOneDatabaseLogInFirstBuilder LogInFirst() {
            return new OneDatabaseLogInFirstBuilder(wrapper);
        }
        public IOneDatabaseTenantFirstBuilder TenantFirst() {
            wrapper.ConfigureOptions<InternalExtensionModuleOptions>(o => {
                o.CustomizeTypesInfo.Add(typesInfo => {
                    CurrentUserOwnerOperator.Register();
                });
            });
            return TenantFirst<TenantNamesHelper<TenantObject>>();
        }
        public IOneDatabaseTenantFirstBuilder TenantFirst<TTenantNamesHelper>()
            where TTenantNamesHelper : class, ITenantNamesHelper {
            wrapper.AddService<ITenantNamesHelper, TTenantNamesHelper>();
            wrapper.ConfigureOptions<InternalExtensionModuleOptions>(o => {
                o.DeclaredExportedTypes.Add(typeof(TenantObject));
                o.CustomizeDefaultRole.Add(defaultRole => {
                    defaultRole.AddObjectPermissionFromLambda<Tenant>(
                    SecurityOperations.ReadWriteAccess,
                    t => (t.Owner != null) && (t.Owner != (string)CurrentUserOwnerOperator.CurrentUserOwner()),
                    SecurityPermissionState.Deny);
                    defaultRole.AddMemberPermission<SAASPermissionPolicyUser>("Read;Write", "TenantName", null, SecurityPermissionState.Deny);
                });
            });
            return new OneDatabaseTenantFirstBuilder(wrapper);
        }
    }
    public class MultipleDatabaseLogInFirstBuilder : IMultipleDatabaseLogInFirstBuilder {
        IXAFApplicationBuilderWrapper wrapper;
        public MultipleDatabaseLogInFirstBuilder(IXAFApplicationBuilderWrapper wrapper) {
            this.wrapper = wrapper;
        }
        public void AddSelectUserTenantsLogonController() {
            wrapper.AddLogonController<SelectTenantAfterLogInController>();
            wrapper.ConfigureOptions<InternalExtensionModuleOptions>(o => {
                o.CustomizeTypesInfo.Add(typesInfo => {
                    ITypeInfo typeInfo = typesInfo.FindTypeInfo(typeof(CustomLogonParametersForStandardAuthentication));
                    if (typeInfo != null) {
                        typeInfo.FindMember("UserName").AddAttribute(new ImmediatePostDataAttribute());
                        IMemberInfo memberInfo = typeInfo.FindMember("Tenant");
                        if (memberInfo != null) {
                            memberInfo.AddAttribute(new DetailViewLayoutAttribute("AuthenticationStandardLogonParameters"));
                            memberInfo.AddAttribute(new IndexAttribute(5));
                        }
                    }
                });
            });
            wrapper.AddBuildStep(application => {
                ApplicationExtensions.AddAdditionalObjectSpace(application);
            });
        }
        public void AddSelectUserTenantsStartupAction() {
            wrapper.ConfigureOptions<InternalExtensionModuleOptions>(o => {
                o.DeclaredExportedTypes.Add(typeof(SelectTenantNameObject));
                o.CustomizeTypesInfo.Add(typesInfo => {
                    ITypeInfo typeInfo = typesInfo.FindTypeInfo(typeof(CustomLogonParametersForStandardAuthentication));
                    if (typeInfo != null) {
                        IMemberInfo memberInfo = typeInfo.FindMember("Tenant");
                        if (memberInfo != null) {
                            memberInfo.AddAttribute(new BrowsableAttribute(false));
                        }
                    }
                });
                o.AddSelectTenantStartupAction = (m, actions) => {
                    IObjectSpace objectSpace = m.Application.CreateObjectSpace(typeof(SelectTenantNameObject));
                    PopupWindowShowAction startupAction = new PopupWindowShowAction();
                    startupAction.CustomizePopupWindowParams +=
                        delegate (Object sender, CustomizePopupWindowParamsEventArgs e) {
                            SelectTenantNameObject obj = objectSpace.CreateObject<SelectTenantNameObject>();
                            obj.SetServiceProvider(m.Application.ServiceProvider);
                            e.View = m.Application.CreateDetailView(objectSpace, obj, true);
                        };
                    startupAction.Execute += (s, args) => {
                        var logonParameters = ((ILogonParameterProvider)m.Application.ServiceProvider.GetService(typeof(ILogonParameterProvider))).GetLogonParameters<ITenantName>();
                        logonParameters.TenantName = ((SelectTenantNameObject)args.PopupWindowViewCurrentObject).TenantName?.Name;
                        m.Application.ChangeModel();
                    };
                    actions.Add(startupAction);
                };
            });
        }
        public IMultipleDatabaseLogInFirstBuilder AddSelectTenantsRunTimeController() {
            wrapper.ConfigureOptions<InternalExtensionModuleOptions>(o => {
                o.RunTimeControllers.Add(typeof(SelectTenantRunTimeController));
            });
            return this;
        }
    }
    public class MultipleDatabaseTenantFirstBuilder : IMultipleDatabaseTenantFirstBuilder {
        IXAFApplicationBuilderWrapper wrapper;
        public MultipleDatabaseTenantFirstBuilder(IXAFApplicationBuilderWrapper wrapper) {
            this.wrapper = wrapper;
        }
        public void AddSelectTenantsLogonController(Action<SelectTenantController> configure = null) {
            wrapper.AddLogonController<SelectTenantController>(configure);
            wrapper.ConfigureOptions<InternalExtensionModuleOptions>(o => {
                o.RunTimeControllers.Add(typeof(SelectTenantController));
            });
        }
    }
    public class OneDatabaseLogInFirstBuilder : IOneDatabaseLogInFirstBuilder {
        IXAFApplicationBuilderWrapper wrapper;
        public OneDatabaseLogInFirstBuilder(IXAFApplicationBuilderWrapper wrapper) {
            this.wrapper = wrapper;
        }
    }
    public class OneDatabaseTenantFirstBuilder : IOneDatabaseTenantFirstBuilder {
        IXAFApplicationBuilderWrapper wrapper;
        public OneDatabaseTenantFirstBuilder(IXAFApplicationBuilderWrapper wrapper) {
            this.wrapper = wrapper;
        }
        public void AddSelectTenantsLogonController(Action<SelectTenantController> configure = null) {
            wrapper.AddLogonController<SelectTenantController>(configure);
            wrapper.ConfigureOptions<InternalExtensionModuleOptions>(o => {
                o.RunTimeControllers.Add(typeof(SelectTenantController));
            });
        }
    }
}
