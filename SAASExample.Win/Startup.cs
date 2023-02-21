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
            .MultipleDatabases()
            .TenantFirst()
            .AddSelectTenantsLogonController();
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
        .MultipleDatabases().LogInFirst()
        .AddSelectTenantsRunTimeController()
        //.AddSelectUserTenantsStartupAction();
        .AddSelectUserTenantsLogonController();
#endif

        builder.Modules
            .AddConditionalAppearance()
            .AddValidation(options => {
                options.AllowValidationDetailsAccess = false;
            })
            .Add<SAASExample.Module.SAASExampleModule>()
        	.Add<SAASExampleWinModule>();
        builder.ObjectSpaceProviders
#if LogInFirst
               .AddSecuredEFCore().WithDbContext<ServiceDBContext>((application, options) => {
                   ArgumentNullException.ThrowIfNull(connectionString);
                   options.UseSqlServer(connectionString);
                   options.UseChangeTrackingProxies();
                   options.UseObjectSpaceLinkProxies();
               })
#endif
            .AddSecuredEFCore().WithDbContext<SAASExample.Module.BusinessObjects.SAASExampleEFCoreDbContext>((application, options) => {
                // Uncomment this code to use an in-memory database. This database is recreated each time the server starts. With the in-memory database, you don't need to make a migration when the data model is changed.
                // Do not use this code in production environment to avoid data loss.
                // We recommend that you refer to the following help topic before you use an in-memory database: https://docs.microsoft.com/en-us/ef/core/testing/in-memory
                //options.UseInMemoryDatabase("InMemory");
                //options.UseSqlServer(connectionString);
#if TenantFirstOneDatabase
                ArgumentNullException.ThrowIfNull(connectionString);
                options.UseSqlServer(connectionString);
#endif
                options.UseChangeTrackingProxies();
                options.UseObjectSpaceLinkProxies();
            })
            .AddNonPersistent();
        builder.Security
            .UseIntegratedMode(options => {
                options.RoleType = typeof(PermissionPolicyRole);
                options.UserType = typeof(SAASExample.Module.BusinessObjects.ApplicationUser);
                options.UserLoginInfoType = typeof(SAASExample.Module.BusinessObjects.ApplicationUserLoginInfo);
            })
#if TenantFirst || LogInFirst
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
