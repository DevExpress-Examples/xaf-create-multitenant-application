using System.Configuration;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.Win.ApplicationBuilder;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win;
using DevExpress.Persistent.Base;
using Microsoft.EntityFrameworkCore;
using DevExpress.ExpressApp.EFCore;
using DevExpress.EntityFrameworkCore.Security;
using DevExpress.XtraEditors;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using DevExpress.ExpressApp.Design;
using Microsoft.Extensions.DependencyInjection;
using MultiTenancyExample.Module;
using MultiTenancyExtensionWin;
using MultiTenancyExtension.Security;
using MultiTenancyExtension.BusinessObjects;
using MultiTenancyExtension.Services;
using MultiTenancyExample.Module.BusinessObjects;
using DevExpress.ExpressApp.MiddleTier;
using MultiTenancyExtension;

namespace MultiTenancyExample.Win;

public class ApplicationBuilder : IDesignTimeApplicationFactory {
    public static WinApplication BuildApplication(string connectionString) {
        var builder = WinApplication.CreateBuilder();
        builder.UseApplication<MultiTenancyExampleWindowsFormsApplication>();

#if TenantFirst
        builder
            .AddMultiTenancyModelDifferenceStore(mds => {
                mds.Assembly = typeof(MultiTenancyExampleModule).Assembly;
            })
            .MakeMultiTenancy(o => {
                o.SelectTenantPropertyCaption = "Company";
                o.TenantObjectDisplayName = "Company";
                o.LogonFormCaption = "Log In";
                o.RemoveExtraNavigationItems = true;
            })
            .MultipleDatabases(builder => {
                ((IWinApplicationBuilder)builder).ObjectSpaceProviders.AddSecuredEFCore().WithDbContext<MultiTenancyExampleEFCoreDbContext>((application, options) => {
                    options.UseDefaultSQLServerMultiTenancyOptions(application.ServiceProvider);
                }, ServiceLifetime.Transient);
            })
            .TenantFirst<ApplicationUser>();
            //.AddSelectTenantsLogonController();
#endif
#if TenantFirstOneDatabase
        builder
        .AddMultiTenancyModelDifferenceStore(mds => {
            mds.Assembly = typeof(MultiTenancyExampleModule).Assembly;
            mds.ServiceModelResourceName = "ServiceModelOneDatabase";
        })
        .MakeMultiTenancy(o => {
            o.SelectTenantPropertyCaption = "Company";
            o.TenantObjectDisplayName = "Company";
            o.LogonFormCaption = "Log In";
            o.RemoveExtraNavigationItems = true;
        })
        .OneDatabase()
        .TenantFirst()
        .AddSelectTenantsLogonController();
        builder.ObjectSpaceProviders
          .AddSecuredEFCore().WithDbContext<MultiTenancyExample.Module.BusinessObjects.MultiTenancyExampleEFCoreDbContext>((application, options) => {
           options.UseDefaultSQLServerMultiTenancyOptions(application.ServiceProvider);
        });
#endif
#if LogInFirst
        builder
        .AddMultiTenancyModelDifferenceStore(mds => {
            mds.Assembly = typeof(MultiTenancyExampleModule).Assembly;
            mds.ServiceModelResourceName = "ExtendedServiceModel";
            mds.ProductionModelResourceName = "LiteProductionModel";
        })
        .MakeMultiTenancy(o => {
            o.SelectTenantPropertyCaption = "Company";
            o.SelectTenantFormCaption = "Select Company";
            o.TenantObjectDisplayName = "Company";
            o.LogonFormCaption = "Log In";
            o.RemoveExtraNavigationItems = true;
        })
        .MultipleDatabases(builder => {
            ((IWinApplicationBuilder)builder).ObjectSpaceProviders.AddSecuredEFCore().WithDbContext<MultiTenancyExampleEFCoreDbContext>((application, options) => {
                options.UseDefaultSQLServerMultiTenancyOptions(application.ServiceProvider);
            }, ServiceLifetime.Transient);
        }).LogInFirst<ServiceDBContext<ApplicationUser, ApplicationUserLoginInfo>>()
        .AddSelectUserTenantsLogonController();
#endif
        //#if LogInFirstOneDatabase
        //        builder
        //        .AddMultiTenancyModelDifferenceStore(mds => {
        //            mds.Assembly = typeof(MultiTenancyExampleModule).Assembly;
        //            mds.ServiceModelResourceName = "ExtendedServiceModel";
        //            mds.ProductionModelResourceName = "LiteProductionModel";
        //        })
        //        .MakeMultiTenancy(o => {
        //            o.SelectTenantPropertyCaption = "Company";
        //            o.SelectTenantFormCaption = "Select Company";
        //            o.TenantObjectDisplayName = "Company";
        //            o.LogonFormCaption = "Log In";
        //            o.RemoveExtraNavigationItems = true;
        //        })
        //        .OneDatabase()
        //        .LogInFirst<ServiceDBContext<ApplicationUser, ApplicationUserLoginInfo>>()
        //        .AddSelectTenantsRunTimeController()
        //        //.AddSelectUserTenantsStartupAction();
        //        .AddSelectUserTenantsLogonController();
        //        builder.ObjectSpaceProviders
        //            .AddSecuredEFCore().WithDbContext<MultiTenancyExampleEFCoreDbContext>((application, options) => {
        //                options.UseDefaultSQLServerMultiTenancyOptions(application.ServiceProvider);
        //            });
        //#endif
#if PredefinedTenant
        builder
        .AddMultiTenancyModelDifferenceStore(mds => {
            mds.Assembly = typeof(MultiTenancyExampleModule).Assembly;
            mds.ServiceModelResourceName = "PredefinedTenantServiceModel";
            mds.ProductionModelResourceName = "LiteProductionModel";
        })
        .MakeMultiTenancy(o => {
            o.TenantObjectDisplayName = "Company";
            o.LogonFormCaption = "Log In";
            o.RemoveExtraNavigationItems = true;
        })
        .MultipleDatabases(builder => {
            ((IWinApplicationBuilder)builder).ObjectSpaceProviders.AddSecuredEFCore().WithDbContext<MultiTenancyExampleEFCoreDbContext>((application, options) => {
                options.UseDefaultSQLServerMultiTenancyOptions(application.ServiceProvider);
            });
        })
        .PredefinedTenant<ServiceDBContext<ApplicationUser, ApplicationUserLoginInfo>>();
#endif
#if PredefinedTenantOneDatabase
        builder
        .AddMultiTenancyModelDifferenceStore(mds => {
            mds.Assembly = typeof(MultiTenancyExampleModule).Assembly;
            mds.ServiceModelResourceName = "PredefinedTenantServiceModel";
            mds.ProductionModelResourceName = "LiteProductionModel";
        })
        .MakeMultiTenancy(o => {
            o.TenantObjectDisplayName = "Company";
            o.LogonFormCaption = "Log In";
            o.RemoveExtraNavigationItems = true;
        })
        .OneDatabase()
        .PredefinedTenant<ServiceDBContext<ApplicationUser, ApplicationUserLoginInfo>>();
        builder.ObjectSpaceProviders
           .AddSecuredEFCore().WithDbContext<MultiTenancyExampleEFCoreDbContext>((application, options) => {
               options.UseDefaultSQLServerOptions(application.ServiceProvider);
           });
#endif

        builder.Modules
            .AddConditionalAppearance()
            .AddValidation(options => {
                options.AllowValidationDetailsAccess = false;
            })
            .Add<MultiTenancyExampleModule>()
        	.Add<MultiTenancyExampleWinModule>();
        builder.ObjectSpaceProviders.AddNonPersistent();
        builder.Security
            .UseIntegratedMode(options => {
                options.RoleType = typeof(PermissionPolicyRole);
                options.UserType = typeof(MultiTenancyExample.Module.BusinessObjects.ApplicationUser);
                options.UserLoginInfoType = typeof(MultiTenancyExample.Module.BusinessObjects.ApplicationUserLoginInfo);
            })
#if TenantFirst || LogInFirst || LogInFirstOneDatabase || PredefinedTenant || PredefinedTenantOneDatabase
             .AddMultiTenancyPasswordAuthentication(options => {
                 options.IsSupportChangePassword = true;
             });
#endif
#if TenantFirstOneDatabase
            .AddMultiTenancyPasswordAuthentication<CustomAuthenticationStandardProvider>(options => {
                 options.IsSupportChangePassword = true;
             });
#endif

        builder.AddBuildStep(application => {
            //application.ConnectionString = connectionString;
#if DEBUG
            if(System.Diagnostics.Debugger.IsAttached && application.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema) {
                application.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
#endif
        });
        var winApplication = builder.Build();
        return winApplication;
    }

    XafApplication IDesignTimeApplicationFactory.Create()
        => BuildApplication(XafApplication.DesignTimeConnectionString);
}
