using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Blazor.ApplicationBuilder;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.MiddleTier;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SAASExtension.BusinessObjects;
using SAASExtension.Controllers;
using SAASExtension.Interfaces;
using SAASExtension.Options;
using SAASExtension.Security;
using SAASExtension.Services;
using SAASExtensionBlazor;
using System;
using System.ComponentModel;
using System.Reflection;

namespace SAASExtension {
    public class SAASApplicationBuilder : ISAASApplicationBuilder {
        IBlazorApplicationBuilder applicationBuilder;
        public SAASApplicationBuilder(IBlazorApplicationBuilder builder) {
            this.applicationBuilder = builder;
            applicationBuilder.ConfigureOptions<InternalExtensionModuleOptions>(o => {
                o.DeclaredExportedTypes.Add(typeof(TenantNameHolder));
            });
        }
        public IMultipleDatabaseBuilder MultipleDatabases() {
            return MultipleDatabases<ConnectionStringProvider, ConfigurationConnectionStringProvider>();
        }
        public IMultipleDatabaseBuilder MultipleDatabases<TConnectionStringProvider, TConfigurationConnectionStringProvider>()
            where TConnectionStringProvider : class, IConnectionStringProvider
            where TConfigurationConnectionStringProvider : class, IConfigurationConnectionStringProvider {
            applicationBuilder.AddService<IConnectionStringProvider, TConnectionStringProvider>();
            applicationBuilder.AddService<IConfigurationConnectionStringProvider, TConfigurationConnectionStringProvider>();
            return new MultipleDatabaseBuilder(applicationBuilder);
        }
        public IOneDatabaseBuilder OneDatabase() {
            return new OneDatabaseBuilder(applicationBuilder);
        }
    }
    public class MultipleDatabaseBuilder : IMultipleDatabaseBuilder {
        IBlazorApplicationBuilder applicationBuilder;
        public MultipleDatabaseBuilder(IBlazorApplicationBuilder builder) {
            this.applicationBuilder = builder;
        }
        public IMultipleDatabaseLogInFirstBuilder LogInFirst() {
            return LogInFirst<UserTenantNamesHelper>();
        }
        public IMultipleDatabaseLogInFirstBuilder LogInFirst<TTenantNamesHelper>()
            where TTenantNamesHelper : class, ITenantNamesHelper {
            applicationBuilder.AddService<ITenantNamesHelper, TTenantNamesHelper>();
            applicationBuilder.ConfigureOptions<InternalExtensionModuleOptions>(o => {
                o.DeclaredExportedTypes.Add(typeof(TenantWithConnectionStringWithUsersObject));
                o.DeclaredExportedTypes.Add(typeof(TenantWithConnectionStringObject));
                o.DeclaredExportedTypes.Add(typeof(TenantObject));
            });

            return new MultipleDatabaseLogInFirstBuilder(applicationBuilder);
        }
        public IMultipleDatabaseTenantFirstBuilder TenantFirst() {
            return TenantFirst<TenantNamesHelper<TenantWithConnectionStringObject>>();
        }
        public IMultipleDatabaseTenantFirstBuilder TenantFirst<TTenantNamesHelper>()
            where TTenantNamesHelper : class, ITenantNamesHelper {
            applicationBuilder.AddService<ITenantNamesHelper, TTenantNamesHelper>();
            applicationBuilder.ConfigureOptions<InternalExtensionModuleOptions>(o => {
                o.DeclaredExportedTypes.Add(typeof(TenantWithConnectionStringObject));
                o.DeclaredExportedTypes.Add(typeof(TenantObject));
            });
            return new MultipleDatabaseTenantFirstBuilder(applicationBuilder);
        }
    }
    public class OneDatabaseBuilder : IOneDatabaseBuilder {
        IBlazorApplicationBuilder applicationBuilder;
        public OneDatabaseBuilder(IBlazorApplicationBuilder builder) {
            this.applicationBuilder = builder;
        }
        public IOneDatabaseLogInFirstBuilder LogInFirst() {
            return new OneDatabaseLogInFirstBuilder(applicationBuilder);
        }
        public IOneDatabaseTenantFirstBuilder TenantFirst() {
            applicationBuilder.ConfigureOptions<InternalExtensionModuleOptions>(o => {
                o.CustomizeTypesInfo.Add(typesInfo => {
                    CurrentUserOwnerOperator.Register();
                });
            });
            return TenantFirst<TenantNamesHelper<TenantObject>>();
        }
        public IOneDatabaseTenantFirstBuilder TenantFirst<TTenantNamesHelper>()
            where TTenantNamesHelper : class, ITenantNamesHelper {
            applicationBuilder.AddService<ITenantNamesHelper, TTenantNamesHelper>();
            applicationBuilder.ConfigureOptions<InternalExtensionModuleOptions>(o => {
                o.DeclaredExportedTypes.Add(typeof(TenantObject));
                o.CustomizeDefaultRole.Add(defaultRole => {
                    defaultRole.AddObjectPermissionFromLambda<Tenant>(
                    SecurityOperations.ReadWriteAccess,
                    t => (t.Owner != null) && (t.Owner != (string)CurrentUserOwnerOperator.CurrentUserOwner()),
                    SecurityPermissionState.Deny);
                    defaultRole.AddMemberPermission<SAASPermissionPolicyUser>("Read;Write", "TenantName", null, SecurityPermissionState.Deny);
                });
            });
            return new OneDatabaseTenantFirstBuilder(applicationBuilder);
        }
    }
    public class MultipleDatabaseLogInFirstBuilder : IMultipleDatabaseLogInFirstBuilder {
        IBlazorApplicationBuilder applicationBuilder;
        public MultipleDatabaseLogInFirstBuilder(IBlazorApplicationBuilder builder) {
            this.applicationBuilder = builder;
        }
        public void AddSelectUserTenantsLogonController() {
            applicationBuilder.AddLogonController<SelectTenantAfterLogInController>();
            applicationBuilder.ConfigureOptions<InternalExtensionModuleOptions>(o => {
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
            applicationBuilder.AddBuildStep(application => {
                ApplicationExtensions.AddAdditionalObjectSpace(application);
            });
        }
        public void AddSelectUserTenantsStartupAction() {
            applicationBuilder.ConfigureOptions<InternalExtensionModuleOptions>(o => {
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
            applicationBuilder.ConfigureOptions<InternalExtensionModuleOptions>(o => {
                o.RunTimeControllers.Add(typeof(SelectTenantRunTimeController));
            });
            return this;
        }
    }
    public class MultipleDatabaseTenantFirstBuilder : IMultipleDatabaseTenantFirstBuilder {
        IBlazorApplicationBuilder applicationBuilder;
        public MultipleDatabaseTenantFirstBuilder(IBlazorApplicationBuilder builder) {
            this.applicationBuilder = builder;
        }
        public void AddSelectTenantsLogonController(Action<SelectTenantController> configure = null) {
            applicationBuilder.AddLogonController<SelectTenantController>(configure);
            applicationBuilder.ConfigureOptions<InternalExtensionModuleOptions>(o => {
                o.RunTimeControllers.Add(typeof(SelectTenantController));
            });
        }
    }
    public class OneDatabaseLogInFirstBuilder : IOneDatabaseLogInFirstBuilder {
        IBlazorApplicationBuilder applicationBuilder;
        public OneDatabaseLogInFirstBuilder(IBlazorApplicationBuilder builder) {
            this.applicationBuilder = builder;
        }
    }
    public class OneDatabaseTenantFirstBuilder : IOneDatabaseTenantFirstBuilder {
        IBlazorApplicationBuilder applicationBuilder;
        public OneDatabaseTenantFirstBuilder(IBlazorApplicationBuilder builder) {
            this.applicationBuilder = builder;
        }
        public void AddSelectTenantsLogonController(Action<SelectTenantController> configure = null) {
            applicationBuilder.AddLogonController<SelectTenantController>(configure);
            applicationBuilder.ConfigureOptions<InternalExtensionModuleOptions>(o => {
                o.RunTimeControllers.Add(typeof(SelectTenantController));
            });
        }
    }
}
