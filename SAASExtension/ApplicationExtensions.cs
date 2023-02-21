using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl.EF;
using SAASExtension.Controllers;
using SAASExtension.Interfaces;
using System.Reflection;
using SAASExtension.BusinessObjects;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using static DevExpress.Data.Filtering.Helpers.SubExprHelper.ThreadHoppingFiltering;
using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.MiddleTier;
using System;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using SAASExtension.Options;
using static DevExpress.Data.Mask.MaskManager;

namespace SAASExtension {
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    public static class ApplicationExtensions {
        //public static void AddCustomizeLogonParameters(List<Action<ITypesInfo>> customizeTypesInfo) {
        //    customizeTypesInfo.Add(typesInfo => {
        //        ITypeInfo typeInfo = typesInfo.FindTypeInfo(typeof(CustomLogonParametersForStandardAuthentication));
        //        if (typeInfo != null) {
        //            typeInfo.FindMember("UserName").AddAttribute(new ImmediatePostDataAttribute());
        //            IMemberInfo memberInfo = typeInfo.FindMember("Tenant");
        //            if (memberInfo != null) {
        //                memberInfo.AddAttribute(new DetailViewLayoutAttribute("AuthenticationStandardLogonParameters"));
        //                memberInfo.AddAttribute(new IndexAttribute(5));
        //            }
        //        }
        //    });
        //}
        public static void ParsePublicOptions(InternalExtensionModuleOptions internalOptions, PublicExtensionModuleOptions publicOptions) {
            if (!publicOptions.ShowOwnerProperty) {
                internalOptions.CustomizeTypesInfo.Add(typesInfo => {
                    ITypeInfo typeInfo = typesInfo.FindTypeInfo(typeof(Tenant));
                    if (typeInfo != null) {
                        typeInfo.FindMember("Owner").AddAttribute(new BrowsableAttribute(false));
                    }
                });
            }
            if (publicOptions.LogonFormCaption != null) {
                internalOptions.CustomizeTypesInfo.Add(typesInfo => {
                    ITypeInfo typeInfo = typesInfo.FindTypeInfo(typeof(CustomLogonParametersForStandardAuthentication));
                    if (typeInfo != null) {
                        typeInfo.AddAttribute(new DisplayNameAttribute(publicOptions.LogonFormCaption));
                    }
                });
            }
            if (publicOptions.SelectTenantFormCaption != null) {
                internalOptions.CustomizeTypesInfo.Add(typesInfo => {
                    ITypeInfo typeInfo = typesInfo.FindTypeInfo(typeof(SelectTenantNameObject));
                    if (typeInfo != null) {
                        typeInfo.AddAttribute(new DisplayNameAttribute(publicOptions.SelectTenantFormCaption));
                    }
                });
            }
            if (publicOptions.SelectTenantPropertyCaption != null) {
                internalOptions.CustomizeTypesInfo.Add(typesInfo => {
                    ITypeInfo typeInfo = typesInfo.FindTypeInfo(typeof(CustomLogonParametersForStandardAuthentication));
                    if (typeInfo != null) {
                        IMemberInfo memberInfo = typeInfo.FindMember("Tenant");
                        if (memberInfo != null) {
                            if (!string.IsNullOrEmpty(publicOptions.SelectTenantPropertyCaption)) {
                                memberInfo.AddAttribute(new DisplayNameAttribute(publicOptions.SelectTenantPropertyCaption));
                            }
                        }
                    }
                    typeInfo = typesInfo.FindTypeInfo(typeof(SelectTenantNameObject));
                    if (typeInfo != null) {
                        IMemberInfo memberInfo = typeInfo.FindMember("TenantName");
                        if (memberInfo != null) {
                            memberInfo.AddAttribute(new DisplayNameAttribute(publicOptions.SelectTenantPropertyCaption));
                        }
                    }
                });
            }
            if (publicOptions.TenantObjectDisplayName != null) {
                internalOptions.CustomizeTypesInfo.Add(typesInfo => {
                    ITypeInfo typeInfo = typesInfo.FindTypeInfo(typeof(TenantObject));
                    if (typeInfo != null) {
                        typeInfo.AddAttribute(new DisplayNameAttribute(publicOptions.TenantObjectDisplayName));
                    }
                    typeInfo = typesInfo.FindTypeInfo(typeof(TenantWithConnectionStringObject));
                    if (typeInfo != null) {
                        typeInfo.AddAttribute(new DisplayNameAttribute(publicOptions.TenantObjectDisplayName));
                    }
                    typeInfo = typesInfo.FindTypeInfo(typeof(TenantWithConnectionStringWithUsersObject));
                    if (typeInfo != null) {
                        typeInfo.AddAttribute(new DisplayNameAttribute(publicOptions.TenantObjectDisplayName));
                    }
                });
            }
            if (publicOptions.RemoveExtraNavigationItems) {
                internalOptions.CustomizeDefaultRole.Add(defaultRole => {
                    defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/ModelDifference_ListView", SecurityPermissionState.Deny);
                    defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/PermissionPolicyRole_ListView", SecurityPermissionState.Deny);
                    defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/ApplicationUser_ListView", SecurityPermissionState.Deny);
                });
            }
        }
        public static void AddAdditionalObjectSpace(XafApplication application) {
            application.CreateCustomLogonWindowObjectSpace += (s,e)=> {
                e.ObjectSpace = ((XafApplication)s).CreateObjectSpace(typeof(CustomLogonParametersForStandardAuthentication));
                NonPersistentObjectSpace nonPersistentObjectSpace = e.ObjectSpace as NonPersistentObjectSpace;
                if (nonPersistentObjectSpace != null) {
                    if (!nonPersistentObjectSpace.IsKnownType(typeof(PermissionPolicyUser), true)) {
                        IObjectSpaceFactoryBase factory = (IObjectSpaceFactoryBase)((XafApplication)s).ServiceProvider.GetService(typeof(IObjectSpaceFactoryBase));
                        IObjectSpace additionalObjectSpace = factory.CreateNonSecuredObjectSpace(typeof(PermissionPolicyUser));
                        nonPersistentObjectSpace.AdditionalObjectSpaces.Add(additionalObjectSpace);
                        nonPersistentObjectSpace.Disposed += (s2, e2) => {
                            additionalObjectSpace.Dispose();
                        };
                    }
                }
            };
        }
        public static void AddExtraDiffStore(XafApplication application, Assembly assembly = null, string serviceModelResourceName = null, string productionModelResourceName = null) {
            application.CreateCustomUserModelDifferenceStore += (sender, e) => {
                var logonParameters = ((XafApplication)sender).ServiceProvider?.GetRequiredService<ILogonParameterProvider>()?.GetLogonParameters(typeof(ITenantName)) as ITenantName;
                if (logonParameters != null) {
                    string resourceName = null;
                    if (logonParameters.TenantName == null) {
                        resourceName = serviceModelResourceName;
                    } else {
                        resourceName = productionModelResourceName;
                        ModelStoreBase differenceStore = ((XafApplication)sender).ServiceProvider?.GetRequiredService<ITenantModelDifferenceStore>()?.GetTenantModelDifferenceStore(application, logonParameters.TenantName);
                        if (differenceStore != null) {
                            e.AddExtraDiffStore("TenantDifferences", differenceStore);
                        }
                    }
                    if (assembly == null) {
                        assembly = application.GetType().Assembly;
                    }
                    if (IsResourceExist(assembly, resourceName)) {
                        ResourcesModelStore serviceStore = new ResourcesModelStore(assembly, resourceName);
                        e.AddExtraDiffStore("ServiceStore", serviceStore);
                    }
                }
            };
        }
        private static bool IsResourceExist(Assembly assembly, string resourceName) {
            foreach(string name in assembly.GetManifestResourceNames()) {
                if (Path.GetFileNameWithoutExtension(name).EndsWith(resourceName)) {
                    return true;
                }
            }
            return false;
        }
        public static void CreateCustomLogonWindowController<TLogonController>(XafApplication application, Action<TLogonController> configure = null)
            where TLogonController : Controller, new() {
            application.CreateCustomLogonWindowControllers += (sender, e) => {
                Controller controller = ((XafApplication)sender).CreateController<TLogonController>();
                if (configure != null) {
                    configure.Invoke((TLogonController)controller);
                }
                e.Controllers.Add(controller);
            };
        }
    }
}