using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Win.ApplicationBuilder;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Win.Services{
    public static class ApplicationBuilder{
        public static WinApplication BuildApplication(this IWinApplicationBuilder builder,string connectionString, bool useSecuredProvider, string address=null){
            builder.UseApplication<OutlookInspiredWindowsFormsApplication>();
            builder.AddModules();
            builder.AddObjectSpaceProviders(connectionString, useSecuredProvider);
            builder.AddSecurity(connectionString == null, address);
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

        public static void AddSecurity(this IWinApplicationBuilder builder, bool useMiddleTier,string address=null){
            if (!useMiddleTier){
                builder.AddIntegratedModeSecurity();
            }
            else{
                builder.UseMiddleTierModeSecurity(address).UsePasswordAuthentication();    
            }
        }

        private static IEFCoreMiddleTierAuthenticationBuilder UseMiddleTierModeSecurity(this IWinApplicationBuilder builder,string address=null) 
            => builder.Security.UseMiddleTierMode(options => {
                options.BaseAddress = new Uri(address??"https://localhost:5001/");
                options.Events.OnHttpClientCreated = client => client.DefaultRequestHeaders.Add("Accept", "application/json");
                options.Events.OnCustomAuthenticate = (_, _, args) => {
                    args.Handled = true;
                    var msg = args.HttpClient.PostAsJsonAsync("api/Authentication/Authenticate",
                        (AuthenticationStandardLogonParameters)args.LogonParameters).GetAwaiter().GetResult();
                    var token = (string)msg.Content.ReadFromJsonAsync(typeof(string)).GetAwaiter().GetResult();
                    if (msg.StatusCode == HttpStatusCode.Unauthorized){
                        throw new UserFriendlyException(token);
                    }
                    msg.EnsureSuccessStatusCode();
                    args.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                };
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

        public static IObjectSpaceProviderBuilder<IWinApplicationBuilder> AddObjectSpaceProviders(this IWinApplicationBuilder builder,string connectionString,bool useSecuredProvider=true) 
            => builder.AddObjectSpaceProviders( useSecuredProvider,connectionString)
                .WithDbContext<OutlookInspiredEFCoreDbContext>((application, options) => {
                    
                    // options.ConfigureWarnings(configurationBuilder => configurationBuilder.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning));
                    options.UseChangeTrackingProxies();
                    options.UseObjectSpaceLinkProxies();
                    if (connectionString == null){
                        options.UseMiddleTier(application.Security);
                    }
                    else{
                        new SqlConnectionStringBuilder(connectionString).AttachDatabase();
                        options.UseSqlServer(connectionString);
                        options.UseLazyLoadingProxies();
                    }
                })
                .AddNonPersistent();

        private static DbContextBuilder<IWinApplicationBuilder> AddObjectSpaceProviders(
            this IWinApplicationBuilder builder, bool useSecuredProvider, string connectionString) 
            => !useSecuredProvider||connectionString == null? builder.ObjectSpaceProviders
                .AddEFCore(options => options.PreFetchReferenceProperties()): builder.ObjectSpaceProviders
                .AddSecuredEFCore(options => options.PreFetchReferenceProperties());

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