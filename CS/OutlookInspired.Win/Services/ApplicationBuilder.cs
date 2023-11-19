using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.MultiTenancy;
using DevExpress.ExpressApp.ReportsV2.Win;
using DevExpress.ExpressApp.Scheduler.Win;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Win.ApplicationBuilder;
using DevExpress.ExpressApp.Win.Utils;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Win.Services{
    public static class ApplicationBuilder{
        public static IWinApplicationBuilder Configure(this IWinApplicationBuilder builder,string connectionString){
            builder.UseApplication<OutlookInspiredWindowsFormsApplication>();
            builder.AddModules();
            builder.AddSecuredObjectSpaceProviders();
            builder.AddIntegratedModeSecurity();
            builder.AddMultiTenancy(connectionString);
            builder.AddBuildSteps(connectionString);
            return builder;
        }
        public static void AddBuildSteps(this IWinApplicationBuilder builder, string connectionString) 
            => builder.AddBuildStep(application => {
                application.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
                application.CheckCompatibilityType = CheckCompatibilityType.DatabaseSchema;
                ((WinApplication)application).SplashScreen = new DXSplashScreen(
                    typeof(XafDemoSplashScreen), new DefaultOverlayFormOptions());
                application.ApplicationName = "OutlookInspired";
                SchedulerListEditor.DailyPrintStyleCalendarHeaderVisible = false;
                WinReportServiceController.UseNewWizard = true;
                application.DatabaseVersionMismatch += (_, e) => {
                    e.Updater.Update();
                    e.Handled = true;
                };
                application.LastLogonParametersReading += (_, e) => {
                    if (!string.IsNullOrWhiteSpace(e.SettingsStorage.LoadOption("", "UserName"))) return;
                    e.SettingsStorage.SaveOption("", "UserName", "Admin");
                };
                application.ConnectionString = connectionString;
            });

        private static void AddIntegratedModeSecurity(this IWinApplicationBuilder builder) 
            => builder.Security
                .UseIntegratedMode(options => {
                    options.RoleType = typeof(PermissionPolicyRole);
                    options.UserType = typeof(ApplicationUser);
                    options.UserLoginInfoType = typeof(ApplicationUserLoginInfo);
                    options.Events.OnSecurityStrategyCreated += securityStrategy =>
                        ((SecurityStrategy)securityStrategy).PermissionsReloadMode = PermissionsReloadMode.NoCache;
                })
                .UsePasswordAuthentication();

        public static IObjectSpaceProviderBuilder<IWinApplicationBuilder> AddSecuredObjectSpaceProviders(this IWinApplicationBuilder builder)
            => builder.ObjectSpaceProviders.AddSecuredEFCore(ConfigureObjectSpaceProvider())
                .ObjectSpaceProviderBuilder(application => application.ServiceProvider.AttachDatabase());

        private static Action<EFCoreObjectSpaceProviderOptionsBuilder> ConfigureObjectSpaceProvider()
            => options => options.PreFetchReferenceProperties();

        public static IObjectSpaceProviderBuilder<IWinApplicationBuilder> AddObjectSpaceProviders(this IWinApplicationBuilder builder)
            => builder.ObjectSpaceProviders.AddEFCore(ConfigureObjectSpaceProvider()).ObjectSpaceProviderBuilder();

        public static IObjectSpaceProviderBuilder<IWinApplicationBuilder> ObjectSpaceProviderBuilder(this DbContextBuilder<IWinApplicationBuilder> builder,Action<XafApplication> configure=null) 
            => builder.WithDbContext<OutlookInspiredEFCoreDbContext>((application, options) => {
                configure?.Invoke(application);
                options.UseSqlServer(application.ServiceProvider.GetRequiredService<IConnectionStringProvider>().GetConnectionString());
                options.UseChangeTrackingProxies();
                options.UseObjectSpaceLinkProxies();
                options.UseLazyLoadingProxies();
            }, ServiceLifetime.Transient)
                .AddNonPersistent();

        public static IWinApplicationBuilder AddMultiTenancy(this IWinApplicationBuilder builder, string serviceConnectionString) {
            builder.AddMultiTenancy()
                .WithHostDbContext((_, options) => {
                    options.UseSqlServer(serviceConnectionString);
                    options.UseChangeTrackingProxies();
                    options.UseLazyLoadingProxies();
                })
                .WithMultiTenancyModelDifferenceStore(mds => mds.ModuleType = typeof(Module.OutlookInspiredModule))
                .WithTenantResolver<TenantByEmailResolver>();
            return builder;
        }

        public static IModuleBuilder<IWinApplicationBuilder> AddModules(this IWinApplicationBuilder builder) 
            => builder.Modules
                .AddCharts()
                .AddConditionalAppearance()
                .AddDashboards(options => {
                    options.DashboardDataType = typeof(DashboardData);
                    options.DesignerFormStyle = DevExpress.XtraBars.Ribbon.RibbonFormStyle.Ribbon;
                })
                .AddFileAttachments()
                .AddNotifications()
                .AddOffice(options => options.RichTextMailMergeDataType=typeof(RichTextMailMergeData))
                .AddPivotChart(options => options.ShowAdditionalNavigation = true)
                .AddPivotGrid()
                .AddReports(options => {
                    options.EnableInplaceReports = true;
                    options.ReportDataType = typeof(ReportDataV2);
                    options.ReportStoreMode = DevExpress.ExpressApp.ReportsV2.ReportStoreModes.XML;
                    options.ShowAdditionalNavigation = false;
                })
                .AddScheduler()
                .AddTreeListEditors()
                .AddValidation(options => options.AllowValidationDetailsAccess = false)
                .AddViewVariants()
                .Add<OutlookInspiredWinModule>();

        
    }
}