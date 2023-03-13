using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.ApplicationBuilder.Internal;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.EFCore;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Services.Core;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using DevExpress.Persistent.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SAASExtension.BusinessObjects;
using SAASExtension.Controllers;
using SAASExtension.Interfaces;
using SAASExtension.Modules;
using SAASExtension.Objects;
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
        public IMultipleDatabaseBuilder MultipleDatabases(Action<object> setupProviders) { 
            return MultipleDatabases<ConnectionStringProvider, ConfigurationConnectionStringProvider>(setupProviders);
        }
        public IMultipleDatabaseBuilder MultipleDatabases<TConnectionStringProvider, TConfigurationConnectionStringProvider>(Action<object> setupProviders)
            where TConnectionStringProvider : class, IConnectionStringProvider
            where TConfigurationConnectionStringProvider : class, IConfigurationConnectionStringProvider {
            wrapper.AddService<IConnectionStringProvider, TConnectionStringProvider>();
            wrapper.AddService<IConfigurationConnectionStringProvider, TConfigurationConnectionStringProvider>();
            wrapper.AddService<IExtraObjectSpaceProviders, ExtraObjectSpaceProviders>();
            wrapper.AddService<IDBContextSwitcher, DBContextSwitcher>();
            wrapper.RemoveService<ICheckCompatibilityHelper>();
            wrapper.AddService<ICheckCompatibilityHelper, MultipleDatabaseCheckCompatibilityHelper>();
            wrapper.ConfigureOptions<InternalExtensionModuleOptions>(o => {
                o.CustomizeTypesInfo.Add(typesInfo => {
                    ITypeInfo typeInfo = typesInfo.FindTypeInfo(typeof(TenantObject));
                    if (typeInfo != null) {
                        IMemberInfo memberInfo = typeInfo.FindMember("ConnectionString");
                        if (memberInfo != null) {
                            memberInfo.AddAttribute(new RuleRequiredFieldAttribute("RuleRequiredField for Tenant.ConnectionString", DefaultContexts.Save));
                        }
                    }
                });
            });
            wrapper.SetupMultipleDatabaseProviders(setupProviders);
            return new MultipleDatabaseBuilder(wrapper);
        }
        public IOneDatabaseBuilder OneDatabase() {
            return OneDatabase<SimpleConnectionStringProvider, ConfigurationConnectionStringProvider>();
        }
        public IOneDatabaseBuilder OneDatabase<TConnectionStringProvider, TConfigurationConnectionStringProvider>()
            where TConnectionStringProvider : class, IConnectionStringProvider
            where TConfigurationConnectionStringProvider : class, IConfigurationConnectionStringProvider {
            wrapper.AddService<IConnectionStringProvider, TConnectionStringProvider>();
            wrapper.AddService<IConfigurationConnectionStringProvider, TConfigurationConnectionStringProvider>();
            wrapper.ConfigureOptions<InternalExtensionModuleOptions>(o => {
                o.CustomizeTypesInfo.Add(typesInfo => {
                    ITypeInfo typeInfo = typesInfo.FindTypeInfo(typeof(TenantObject));
                    if (typeInfo != null) {
                        IMemberInfo memberInfo = typeInfo.FindMember("ConnectionString");
                        if (memberInfo != null) {
                            memberInfo.AddAttribute(new BrowsableAttribute(false));
                        }
                    }
                });
            });
            return new OneDatabaseBuilder(wrapper);
        }
    }
    public class MultipleDatabaseBuilder : IMultipleDatabaseBuilder {
        IXAFApplicationBuilderWrapper wrapper;
        public MultipleDatabaseBuilder(IXAFApplicationBuilderWrapper wrapper) {
            this.wrapper = wrapper;
        }
        public IMultipleDatabaseLogInFirstBuilder LogInFirst<TContext>()
            where TContext : DbContext {
            wrapper.AddTenantObjectObjectSpaceProvider<TContext>(false);
            return LogInFirst<UserTenantNamesHelper<TContext>>(true);
        }
        public IMultipleDatabaseLogInFirstBuilder LogInFirst<TTenantNamesHelper>(bool useBuiltInTypes = false)
            where TTenantNamesHelper : class, ITenantNamesHelper { 
            wrapper.AddService<ITenantNamesHelper, TTenantNamesHelper>();
            if (useBuiltInTypes) {
                wrapper.ConfigureOptions<InternalExtensionModuleOptions>(o => {
                    o.DeclaredExportedTypes.Add(typeof(TenantObjectWithUsers));
                    o.DeclaredExportedTypes.Add(typeof(TenantObject));

                    o.CustomizeDefaultRole.Add(defaultRole => {
                        defaultRole.AddTypePermission<TenantObjectWithUsers>(
                        SecurityOperations.ReadWriteAccess, SecurityPermissionState.Deny);
                    });
                });
            }

            return new MultipleDatabaseLogInFirstBuilder(wrapper);
        }
        public IMultipleDatabaseTenantFirstBuilder TenantFirst() {
            return TenantFirst<PermissionPolicyUser>();
        }
        public IMultipleDatabaseTenantFirstBuilder TenantFirst<TUserType>()
            where TUserType : PermissionPolicyUser {
            wrapper.AddTenantObjectObjectSpaceProvider<TenantWithConnectionStringDbContext<TUserType>>();
            return TenantFirst<TenantNamesHelper<TenantObject, TenantWithConnectionStringDbContext<TUserType>>>(true);
        }
        public IMultipleDatabaseTenantFirstBuilder TenantFirst<TTenantNamesHelper>(bool useBuiltInTypes = false)
            where TTenantNamesHelper : class, ITenantNamesHelper {
            if (useBuiltInTypes) {
                wrapper.ConfigureOptions<InternalExtensionModuleOptions>(o => {
                    o.DeclaredExportedTypes.Add(typeof(TenantObject));
                });
            }
            wrapper.AddService<ITenantNamesHelper, TTenantNamesHelper>();
            return new MultipleDatabaseTenantFirstBuilder(wrapper);
        }
        public IMultipleDatabasePredefinedTenantBuilder PredefinedTenant<TContext>()
            where TContext : DbContext {
            wrapper.AddTenantObjectObjectSpaceProvider<TContext>(false);
            wrapper.AddService<ITenantNamesHelper, TenantNamesHelper<TenantObject, TContext>>();
            wrapper.ConfigureOptions<InternalExtensionModuleOptions>(o => {
                o.DeclaredExportedTypes.Add(typeof(TenantObject));
                o.CustomizeTypesInfo.Add(typesInfo => {
                    ITypeInfo typeInfo = typesInfo.FindTypeInfo(typeof(CustomLogonParametersForStandardAuthentication));
                    if (typeInfo != null) {
                        IMemberInfo memberInfo = typeInfo.FindMember("Tenant");
                        if (memberInfo != null) {
                            memberInfo.AddAttribute(new BrowsableAttribute(false));
                        }
                    }
                });
            });
            wrapper.AddBuildStep(application => {
                ApplicationExtensions.SetTenantFromUserToLogonParameters<TContext>(application);
            });
            return new MultipleDatabasePredefinedTenantBuilder(wrapper);
        }
    }
    public class OneDatabaseBuilder : IOneDatabaseBuilder {
        IXAFApplicationBuilderWrapper wrapper;
        public OneDatabaseBuilder(IXAFApplicationBuilderWrapper wrapper) {
            this.wrapper = wrapper;
        }
        public IOneDatabaseTenantFirstBuilder TenantFirst() { 
            wrapper.AddService<IExtraObjectSpaceProviders, ExtraObjectSpaceProviders>();
            wrapper.AddTenantObjectObjectSpaceProvider<TenantObjectDbContext>(false);
            wrapper.ConfigureOptions<InternalExtensionModuleOptions>(o => {
                o.RunTimeControllers.Add(typeof(AddAdditionalObjectSpaceController));
            });
            return TenantFirst<TenantNamesHelper<TenantObject, TenantObjectDbContext>>(true);
        }
        public IOneDatabaseTenantFirstBuilder TenantFirst<TTenantNamesHelper>(bool useBuiltInTypes = false)
            where TTenantNamesHelper : class, ITenantNamesHelper { 
            wrapper.AddService<ITenantNamesHelper, TTenantNamesHelper>();
            wrapper.ConfigureOptions<InternalExtensionModuleOptions>(o => {
                if (useBuiltInTypes) {
                    o.DeclaredExportedTypes.Add(typeof(TenantObject));
                }
                o.CustomizeTypesInfo.Add(typesInfo => {
                    CurrentUserOwnerOperator.Register();
                });
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
        //public IOneDatabaseLogInFirstBuilder LogInFirst<TContext>() where TContext : DbContext {
        //    wrapper.AddTenantObjectObjectSpaceProvider<TContext>(false);
        //    wrapper.AddService<IExtraObjectSpaceProviders, ExtraObjectSpaceProviders>();
        //    return LogInFirst<UserTenantNamesHelper<TContext>>(true);
        //}
        //public IOneDatabaseLogInFirstBuilder LogInFirst<TTenantNamesHelper>(bool useBuiltInTypes = false) 
        //    where TTenantNamesHelper : class, ITenantNamesHelper { 
        //    wrapper.AddService<ITenantNamesHelper, TTenantNamesHelper>();
        //    if (useBuiltInTypes) {
        //        wrapper.ConfigureOptions<InternalExtensionModuleOptions>(o => {
        //            o.DeclaredExportedTypes.Add(typeof(TenantObjectWithUsers));
        //            o.DeclaredExportedTypes.Add(typeof(TenantObject));

        //            o.CustomizeDefaultRole.Add(defaultRole => {
        //                defaultRole.AddTypePermission<TenantObjectWithUsers>(
        //                SecurityOperations.ReadWriteAccess, SecurityPermissionState.Deny);
        //            });
        //            //o.CustomizeDefaultRole.Add(defaultRole => {
        //            //    defaultRole.AddObjectPermissionFromLambda<Tenant>(
        //            //    SecurityOperations.ReadWriteAccess,
        //            //    t => (t.Owner != null) && (t.Owner != (string)CurrentUserOwnerOperator.CurrentUserOwner()),
        //            //    SecurityPermissionState.Deny);
        //            //    defaultRole.AddMemberPermission<SAASPermissionPolicyUser>("Read;Write", "TenantName", null, SecurityPermissionState.Deny);
        //            //});
        //        });
        //    }
        //    return new OneDatabaseLogInFirstBuilder(wrapper);
        //}
        public IOneDatabaseTenantFirstBuilder PredefinedTenant<TContext>() where TContext : DbContext {
            wrapper.ConfigureOptions<InternalExtensionModuleOptions>(o => {
                o.RunTimeControllers.Add(typeof(AddAdditionalObjectSpaceController));
            });
            wrapper.AddTenantObjectObjectSpaceProvider<TContext>(false);
            wrapper.AddService<ITenantNamesHelper, TenantNamesHelper<TenantObject, TContext>>();
            wrapper.ConfigureOptions<InternalExtensionModuleOptions>(o => {
                o.DeclaredExportedTypes.Add(typeof(TenantObject));
                o.CustomizeTypesInfo.Add(typesInfo => {
                    ITypeInfo typeInfo = typesInfo.FindTypeInfo(typeof(CustomLogonParametersForStandardAuthentication));
                    if (typeInfo != null) {
                        IMemberInfo memberInfo = typeInfo.FindMember("Tenant");
                        if (memberInfo != null) {
                            memberInfo.AddAttribute(new BrowsableAttribute(false));
                        }
                    }
                });
                o.CustomizeTypesInfo.Add(typesInfo => {
                    CurrentUserOwnerOperator.Register();
                });
                o.CustomizeDefaultRole.Add(defaultRole => {
                    defaultRole.AddObjectPermissionFromLambda<Tenant>(
                    SecurityOperations.ReadWriteAccess,
                    t => (t.Owner != null) && (t.Owner != (string)CurrentUserOwnerOperator.CurrentUserOwner()),
                    SecurityPermissionState.Deny);
                    defaultRole.AddMemberPermission<SAASPermissionPolicyUser>("Read;Write", "TenantName", null, SecurityPermissionState.Deny);
                });
            });
            wrapper.AddBuildStep(application => {
                ApplicationExtensions.SetTenantFromUserToLogonParameters<TContext>(application);
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
                            memberInfo.AddAttribute(new DevExpress.Persistent.Base.IndexAttribute(5));
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
        public IOneDatabaseLogInFirstBuilder AddSelectTenantsRunTimeController() {
            wrapper.ConfigureOptions<InternalExtensionModuleOptions>(o => {
                o.RunTimeControllers.Add(typeof(SelectTenantRunTimeController));
            });
            return this;
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
                            memberInfo.AddAttribute(new DevExpress.Persistent.Base.IndexAttribute(5));
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
    public class MultipleDatabasePredefinedTenantBuilder : IMultipleDatabasePredefinedTenantBuilder {
        IXAFApplicationBuilderWrapper wrapper;
        public MultipleDatabasePredefinedTenantBuilder(IXAFApplicationBuilderWrapper wrapper) {
            this.wrapper = wrapper;
        }
    }
    public class OneDatabasePredefinedTenantBuilder : IOneDatabasePredefinedTenantBuilder {
        IXAFApplicationBuilderWrapper wrapper;
        public OneDatabasePredefinedTenantBuilder(IXAFApplicationBuilderWrapper wrapper) {
            this.wrapper = wrapper;
        }
    }
}
