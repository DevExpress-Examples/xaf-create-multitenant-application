using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.Office.Win;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Win.ApplicationBuilder;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Win.Extensions{
    public static class ApplicationBuilder{
        public static void AddBuildSteps(this IWinApplicationBuilder builder, string connectionString){
            builder.AddBuildStep(application => {
            application.DatabaseUpdateMode = DatabaseUpdateMode.Never;
            ((WinApplication)application).SplashScreen = new DevExpress.ExpressApp.Win.Utils.DXSplashScreen(
                typeof(XafDemoSplashScreen), new DefaultOverlayFormOptions());
            application.ApplicationName = "MainDemo";
            DevExpress.ExpressApp.Scheduler.Win.SchedulerListEditor.DailyPrintStyleCalendarHeaderVisible = false;
            DevExpress.ExpressApp.ReportsV2.Win.WinReportServiceController.UseNewWizard = true;
            application.DatabaseVersionMismatch += (s, e) => {
                string message = "Application cannot connect to the specified database.";
                if(e.CompatibilityError is CompatibilityDatabaseIsOldError isOldError && isOldError.Module != null) {
                    message = "The client application cannot connect to the Middle Tier Application Server and its database. " +
                              "To avoid this error, ensure that both the client and the server have the same modules set. Problematic module: " + isOldError.Module.Name +
                              ". For more information, see https://docs.devexpress.com/eXpressAppFramework/113439/concepts/security-system/middle-tier-security-wcf-service#troubleshooting";
                }
                if(e.CompatibilityError == null) {
                    message = "You probably tried to update the database in Middle Tier Security mode from the client side. " +
                              "In this mode, the server application updates the database automatically. " +
                              "To disable the automatic database update, set the XafApplication.DatabaseUpdateMode property to the DatabaseUpdateMode.Never value in the client application.";
                }
                throw new InvalidOperationException(message);
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
                options.Events.OnHttpClientCreated = client =>
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                options.Events.OnCustomAuthenticate = (_, _, args) => {
                    args.Handled = true;
                    var msg = args.HttpClient.PostAsJsonAsync("api/Authentication/Authenticate",
                        (AuthenticationStandardLogonParameters)args.LogonParameters).GetAwaiter().GetResult();
                    var token = (string)msg.Content.ReadFromJsonAsync(typeof(string)).GetAwaiter().GetResult();
                    if (msg.StatusCode == HttpStatusCode.Unauthorized){
                        throw new UserFriendlyException(token);
                    }

                    msg.EnsureSuccessStatusCode();
                    args.HttpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("bearer", token);
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
                .AddOffice(options => OptionsRichTextMailMergeDataType(options))
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

        [Obsolete]
        private static void OptionsRichTextMailMergeDataType(OfficeOptions options){
            options.RichTextMailMergeDataType=typeof(RichTextMailMergeData);
        }
    }
}