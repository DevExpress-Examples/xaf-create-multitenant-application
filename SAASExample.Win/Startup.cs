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
using SAASExample.Module;
using SAASExtensionWin;
using SAASExtension.Security;
using SAASExtension.BusinessObjects;
using SAASExtension.Services;
using SAASExample.Module.BusinessObjects;
using DevExpress.ExpressApp.MiddleTier;
using SAASExtension;

namespace SAASExample.Win;

public class ApplicationBuilder : IDesignTimeApplicationFactory {
    public static WinApplication BuildApplication(string connectionString) {
        var builder = WinApplication.CreateBuilder();
        builder.UseApplication<SAASExampleWindowsFormsApplication>();

#if TenantFirst
        builder
            .AddSAASTenantModelDifferenceStore(mds => {
                mds.Assembly = typeof(SAASExampleModule).Assembly;
            })
            .MakeSAAS(o => {
                o.SelectTenantPropertyCaption = "Company";
                o.TenantObjectDisplayName = "Company";
                o.LogonFormCaption = "Log In";
                o.RemoveExtraNavigationItems = true;
            })
            .MultipleDatabases(builder => {
                ((IWinApplicationBuilder)builder).ObjectSpaceProviders.AddSecuredEFCore().WithDbContext<SAASExampleEFCoreDbContext>((application, options) => {
                    options.UseDefaultSQLServerSAASOptions(application.ServiceProvider);
                }, ServiceLifetime.Transient);
            })
            .TenantFirst<ApplicationUser>();
            //.AddSelectTenantsLogonController();
#endif
#if TenantFirstOneDatabase
        builder
        .AddSAASTenantModelDifferenceStore(mds => {
            mds.Assembly = typeof(SAASExampleModule).Assembly;
            mds.ServiceModelResourceName = "ServiceModelOneDatabase";
        })
        .MakeSAAS(o => {
            o.SelectTenantPropertyCaption = "Company";
            o.TenantObjectDisplayName = "Company";
            o.LogonFormCaption = "Log In";
            o.RemoveExtraNavigationItems = true;
        })
        .OneDatabase()
        .TenantFirst()
        .AddSelectTenantsLogonController();
        builder.ObjectSpaceProviders
          .AddSecuredEFCore().WithDbContext<SAASExample.Module.BusinessObjects.SAASExampleEFCoreDbContext>((application, options) => {
           options.UseDefaultSQLServerSAASOptions(application.ServiceProvider);
        });
#endif
#if LogInFirst
        builder
        .AddSAASTenantModelDifferenceStore(mds => {
            mds.Assembly = typeof(SAASExampleModule).Assembly;
            mds.ServiceModelResourceName = "ExtendedServiceModel";
            mds.ProductionModelResourceName = "LiteProductionModel";
        })
        .MakeSAAS(o => {
            o.SelectTenantPropertyCaption = "Company";
            o.SelectTenantFormCaption = "Select Company";
            o.TenantObjectDisplayName = "Company";
            o.LogonFormCaption = "Log In";
            o.RemoveExtraNavigationItems = true;
        })
        .MultipleDatabases(builder => {
            ((IWinApplicationBuilder)builder).ObjectSpaceProviders.AddSecuredEFCore().WithDbContext<SAASExampleEFCoreDbContext>((application, options) => {
                options.UseDefaultSQLServerSAASOptions(application.ServiceProvider);
            }, ServiceLifetime.Transient);
        }).LogInFirst<ServiceDBContext<ApplicationUser, ApplicationUserLoginInfo>>()
        .AddSelectTenantsRunTimeController()
        //.AddSelectUserTenantsStartupAction();
        .AddSelectUserTenantsLogonController();
#endif
        //#if LogInFirstOneDatabase
        //        builder
        //        .AddSAASTenantModelDifferenceStore(mds => {
        //            mds.Assembly = typeof(SAASExampleModule).Assembly;
        //            mds.ServiceModelResourceName = "ExtendedServiceModel";
        //            mds.ProductionModelResourceName = "LiteProductionModel";
        //        })
        //        .MakeSAAS(o => {
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
        //            .AddSecuredEFCore().WithDbContext<SAASExampleEFCoreDbContext>((application, options) => {
        //                options.UseDefaultSQLServerSAASOptions(application.ServiceProvider);
        //            });
        //#endif
#if PredefinedTenant
        builder
        .AddSAASTenantModelDifferenceStore(mds => {
            mds.Assembly = typeof(SAASExampleModule).Assembly;
            mds.ServiceModelResourceName = "ExtendedServiceModel";
            mds.ProductionModelResourceName = "LiteProductionModel";
        })
        .MakeSAAS(o => {
            o.TenantObjectDisplayName = "Company";
            o.LogonFormCaption = "Log In";
            o.RemoveExtraNavigationItems = true;
        })
        .MultipleDatabases(builder => {
            ((IWinApplicationBuilder)builder).ObjectSpaceProviders.AddSecuredEFCore().WithDbContext<SAASExampleEFCoreDbContext>((application, options) => {
                options.UseDefaultSQLServerSAASOptions(application.ServiceProvider);
            });
        })
        .PredefinedTenant<ServiceDBContext<ApplicationUser, ApplicationUserLoginInfo>>();
#endif
#if PredefinedTenantOneDatabase
        builder
        .AddSAASTenantModelDifferenceStore(mds => {
            mds.Assembly = typeof(SAASExampleModule).Assembly;
            mds.ServiceModelResourceName = "ExtendedServiceModel";
            mds.ProductionModelResourceName = "LiteProductionModel";
        })
        .MakeSAAS(o => {
            o.TenantObjectDisplayName = "Company";
            o.LogonFormCaption = "Log In";
            o.RemoveExtraNavigationItems = true;
        })
        .OneDatabase()
        .PredefinedTenant<ServiceDBContext<ApplicationUser, ApplicationUserLoginInfo>>();
        builder.ObjectSpaceProviders
           .AddSecuredEFCore().WithDbContext<SAASExampleEFCoreDbContext>((application, options) => {
               options.UseDefaultSQLServerOptions(application.ServiceProvider);
           });
#endif

        builder.Modules
            .AddConditionalAppearance()
            .AddValidation(options => {
                options.AllowValidationDetailsAccess = false;
            })
            .Add<SAASExampleModule>()
        	.Add<SAASExampleWinModule>();
        builder.ObjectSpaceProviders.AddNonPersistent();
        builder.Security
            .UseIntegratedMode(options => {
                options.RoleType = typeof(PermissionPolicyRole);
                options.UserType = typeof(SAASExample.Module.BusinessObjects.ApplicationUser);
                options.UserLoginInfoType = typeof(SAASExample.Module.BusinessObjects.ApplicationUserLoginInfo);
            })
#if TenantFirst || LogInFirst || LogInFirstOneDatabase || PredefinedTenant || PredefinedTenantOneDatabase
             .AddSAASPasswordAuthentication(options => {
                 options.IsSupportChangePassword = true;
             });
#endif
#if TenantFirstOneDatabase
            .AddSAASPasswordAuthentication<CustomAuthenticationStandardProvider>(options => {
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
