using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.Blazor.ApplicationBuilder;
using DevExpress.ExpressApp.MultiTenancy;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using Microsoft.EntityFrameworkCore;
using OutlookInspired.Module.Services;
using Microsoft.IdentityModel.Tokens;

namespace OutlookInspired.Blazor.Server.Services.Internal{
    internal static class ApplicationBuilder{
        public static IBlazorApplicationBuilder AddBuildStep(this IBlazorApplicationBuilder builder){
            builder.AddBuildStep(application => {
                application.ApplicationName = "OutlookInspired";
                application.CheckCompatibilityType = DevExpress.ExpressApp.CheckCompatibilityType.DatabaseSchema;
                application.DatabaseVersionMismatch += (_, e) => {
                    e.Updater.Update();
                    e.Handled = true;
                };
                application.LastLogonParametersRead += (_, e) => {
                    if (e.LogonObject is not AuthenticationStandardLogonParameters logonParameters || !logonParameters.UserName.IsNullOrEmpty()) return;
                    logonParameters.UserName = "Admin";
                };
            });
            return builder;
        }

        public static IBlazorApplicationBuilder AddSecurity(this IBlazorApplicationBuilder builder){
            builder.Security
                .UseIntegratedMode(options => {
                    options.RoleType = typeof(PermissionPolicyRole);
                    options.UserType = typeof(Module.BusinessObjects.ApplicationUser);
                    options.UserLoginInfoType = typeof(Module.BusinessObjects.ApplicationUserLoginInfo);
                    options.Events.OnSecurityStrategyCreated += securityStrategy => ((SecurityStrategy)securityStrategy).PermissionsReloadMode = PermissionsReloadMode.NoCache;
                })
                .AddPasswordAuthentication(options => {
                    options.IsSupportChangePassword = true;
                });
            return builder;
        }

        public static IBlazorApplicationBuilder AddMultiTenancy(this IBlazorApplicationBuilder builder, IConfiguration configuration){
            builder.AddMultiTenancy()
                .WithHostDbContext((_, options) => {
#if EASYTEST
                    string connectionString = configuration.GetConnectionString("EasyTestConnectionString");
#else
                    string connectionString = configuration.GetConnectionString("ConnectionString");
#endif
                    options.UseSqlServer(connectionString);
                    options.UseChangeTrackingProxies();
                    options.UseLazyLoadingProxies();
                })
                .WithMultiTenancyModelDifferenceStore(mds => mds.ModuleType = typeof(Module.OutlookInspiredModule))
                .WithTenantResolver<TenantByEmailResolver>();
            return builder;
        }

        public static IBlazorApplicationBuilder AddObjectSpaceProviders(this IBlazorApplicationBuilder builder){
            builder.ObjectSpaceProviders
                .AddSecuredEFCore(options => options.PreFetchReferenceProperties())
                .WithDbContext<Module.BusinessObjects.OutlookInspiredEFCoreDbContext>((serviceProvider, options) => {
                    serviceProvider.AttachDatabase();
                    options.UseSqlServer(serviceProvider.GetRequiredService<IConnectionStringProvider>().GetConnectionString());
                    options.UseChangeTrackingProxies();
                    options.UseObjectSpaceLinkProxies();
                    options.UseLazyLoadingProxies();
                })
                .AddNonPersistent();
            return builder;
        }
        
        public static IBlazorApplicationBuilder AddModules(this IBlazorApplicationBuilder builder){
            builder.Modules
                .AddConditionalAppearance()
                .AddFileAttachments()
                .AddOffice(options => options.RichTextMailMergeDataType=typeof(RichTextMailMergeData))
                .AddReports(options => {
                    options.EnableInplaceReports = false;
                    options.ReportDataType = typeof(ReportDataV2);
                    options.ReportStoreMode = DevExpress.ExpressApp.ReportsV2.ReportStoreModes.XML;
                })
                .AddScheduler()
                .AddValidation()
                .AddViewVariants()
                .Add<Module.OutlookInspiredModule>()
                .Add<OutlookInspiredBlazorModule>();
            return builder;
        }
    }
}