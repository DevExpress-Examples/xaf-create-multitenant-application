using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.Blazor.ApplicationBuilder;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using Microsoft.EntityFrameworkCore;

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
                    if (e.LogonObject is AuthenticationStandardLogonParameters logonParameters &&
                        string.IsNullOrEmpty(logonParameters.UserName)){
                        logonParameters.UserName = "Admin";
                    }
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

        public static IBlazorApplicationBuilder AddObjectSpaceProviders(this IBlazorApplicationBuilder builder, IConfiguration configuration){
            builder.ObjectSpaceProviders
                .AddSecuredEFCore(options => options.PreFetchReferenceProperties())
                .WithDbContext<Module.BusinessObjects.OutlookInspiredEFCoreDbContext>((_, options) => {
                    string connectionString = null;
                    if (configuration.GetConnectionString("ConnectionString") != null){
                        connectionString = configuration.GetConnectionString("ConnectionString");
                    }
#if EASYTEST
                        if(Configuration.GetConnectionString("EasyTestConnectionString") != null) {
                            connectionString = Configuration.GetConnectionString("EasyTestConnectionString");
                        }
#endif
                    ArgumentNullException.ThrowIfNull(connectionString);
                    options.UseSqlServer(connectionString);
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