using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win.ApplicationBuilder;
using Microsoft.EntityFrameworkCore;

namespace OutlookInspired.Win.Extensions{
    public static class ApplicationBuilder{
        public static void AddSecurity(this IWinApplicationBuilder builder)
            => builder.Security.UseMiddleTierMode(options => {
                    options.BaseAddress = new Uri("https://localhost:5001/");
                    options.Events.OnHttpClientCreated = client => client.DefaultRequestHeaders.Add("Accept", "application/json");
                    options.Events.OnCustomAuthenticate = (_, _, args) => {
                        args.Handled = true;
                        var msg = args.HttpClient.PostAsJsonAsync("api/Authentication/Authenticate", (AuthenticationStandardLogonParameters)args.LogonParameters).GetAwaiter().GetResult();
                        var token = (string)msg.Content.ReadFromJsonAsync(typeof(string)).GetAwaiter().GetResult();
                        if(msg.StatusCode == HttpStatusCode.Unauthorized) {
                            throw new UserFriendlyException(token);
                        }
                        msg.EnsureSuccessStatusCode();
                        args.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                    };
                })
                .UsePasswordAuthentication();

        public static IObjectSpaceProviderBuilder<IWinApplicationBuilder> AddObjectSpaceProviders(this IWinApplicationBuilder builder,Action<DbContextOptionsBuilder> configure=null) 
            => builder.ObjectSpaceProviders
                .AddEFCore(options => options.PreFetchReferenceProperties())
                .WithDbContext<Module.BusinessObjects.OutlookInspiredEFCoreDbContext>((application, options) => {
                    options.UseChangeTrackingProxies();
                    options.UseObjectSpaceLinkProxies();
                    if (configure != null){
                        configure(options);
                    }
                    else{
                        options.UseMiddleTier(application.Security);    
                    }
                })
                .AddNonPersistent();

        public static IModuleBuilder<IWinApplicationBuilder> AddModules(this IWinApplicationBuilder builder) 
            => builder.Modules
                .AddCharts()
                .AddConditionalAppearance()
                .AddDashboards(options => {
                    options.DashboardDataType = typeof(DevExpress.Persistent.BaseImpl.EF.DashboardData);
                    options.DesignerFormStyle = DevExpress.XtraBars.Ribbon.RibbonFormStyle.Ribbon;
                })
                .AddFileAttachments()
                .AddNotifications()
                .AddOffice()
                .AddPivotChart(options => options.ShowAdditionalNavigation = true)
                .AddPivotGrid()
                .AddReports(options => {
                    options.EnableInplaceReports = true;
                    options.ReportDataType = typeof(DevExpress.Persistent.BaseImpl.EF.ReportDataV2);
                    options.ReportStoreMode = DevExpress.ExpressApp.ReportsV2.ReportStoreModes.XML;
                    options.ShowAdditionalNavigation = true;
                })
                .AddScheduler()
                .AddTreeListEditors()
                .AddValidation(options => options.AllowValidationDetailsAccess = false)
                .AddViewVariants(options => options.ShowAdditionalNavigation = true)
                .Add<OutlookInspiredWinModule>();
    }
}