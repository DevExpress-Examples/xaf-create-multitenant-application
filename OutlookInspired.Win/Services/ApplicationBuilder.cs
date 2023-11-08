using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.MultiTenancy;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Win.ApplicationBuilder;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Win.Services{
    public static class ApplicationBuilder{
        public static WinApplication BuildApplication(this IWinApplicationBuilder builder,string connectionString){
            builder.UseApplication<OutlookInspiredWindowsFormsApplication>();
            builder.AddModules();
            builder.AddObjectSpaceProviders();
            builder.AddIntegratedModeSecurity();
            builder.AddMultiTenancy(connectionString);
            builder.AddBuildSteps(connectionString);
            return builder.Build();
        }
        public static void AddBuildSteps(this IWinApplicationBuilder builder, string connectionString){
            builder.AddBuildStep(application => {
            application.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            application.CheckCompatibilityType = CheckCompatibilityType.DatabaseSchema;
            ((WinApplication)application).SplashScreen = new DevExpress.ExpressApp.Win.Utils.DXSplashScreen(
                typeof(XafDemoSplashScreen), new DefaultOverlayFormOptions());
            application.ApplicationName = "OutlookInspired";
            DevExpress.ExpressApp.Scheduler.Win.SchedulerListEditor.DailyPrintStyleCalendarHeaderVisible = false;
            DevExpress.ExpressApp.ReportsV2.Win.WinReportServiceController.UseNewWizard = true;
            application.DatabaseVersionMismatch += (_, e) => {
                e.Updater.Update();
                e.Handled = true;
            };
            application.LastLogonParametersReading+= (_, e) => {
                if(string.IsNullOrWhiteSpace(e.SettingsStorage.LoadOption("", "UserName"))) {
                    e.SettingsStorage.SaveOption("", "UserName", "Admin");
                }
            };
            application.ConnectionString = connectionString;

        });
        }
        
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

        public static IObjectSpaceProviderBuilder<IWinApplicationBuilder> AddObjectSpaceProviders(this IWinApplicationBuilder builder)
            => builder.ObjectSpaceProviders.AddSecuredEFCore(options => options.PreFetchReferenceProperties())
                .WithDbContext<OutlookInspiredEFCoreDbContext>((application, options) => {
                    application.ServiceProvider.AttachDatabase("..\\..\\..\\..\\Data\\");
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