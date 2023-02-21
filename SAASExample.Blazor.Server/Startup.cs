using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.Blazor.ApplicationBuilder;
using DevExpress.ExpressApp.Blazor.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.EntityFrameworkCore;
using SAASExample.Blazor.Server.Services;
using SAASExtensionBlazor;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using SAASExample.Module;
using SAASExtension.Security;
using SAASExtension.BusinessObjects;
using SAASExtension.Services;
using SAASExample.Module.BusinessObjects;
using SAASExtension.Controllers;
using SAASExtension.Interfaces;

namespace SAASExample.Blazor.Server;

public class Startup {
    public Startup(IConfiguration configuration) {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services) {
        services.AddSingleton(typeof(Microsoft.AspNetCore.SignalR.HubConnectionHandler<>), typeof(ProxyHubConnectionHandler<>));

        services.AddRazorPages();
        services.AddServerSideBlazor();
        services.AddHttpContextAccessor();
        services.AddScoped<CircuitHandler, CircuitHandlerProxy>();
        services.AddXaf(Configuration, builder => {
            builder.UseApplication<SAASExampleBlazorApplication>();

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
            .TenantFirst();
            //.AddSelectTenantsLogonController();
            builder.Security
                .AddSAASPasswordAuthentication(options => {
                    options.IsSupportChangePassword = true;
                });
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
            builder.Security.AddSAASPasswordAuthentication<CustomAuthenticationStandardProvider>(options => {
                 options.IsSupportChangePassword = true;
             });
            //builder.Security
            //    .AddSAASPasswordAuthentication(options => {
            //        options.IsSupportChangePassword = true;
            //    });
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
            builder.Security
                .AddSAASPasswordAuthentication(options => {
                    options.IsSupportChangePassword = true;
                });
#endif

            builder.Modules
                .AddConditionalAppearance()
                .AddValidation(options => {
                    options.AllowValidationDetailsAccess = false;
                })
                .Add<SAASExample.Module.SAASExampleModule>()
            	.Add<SAASExampleBlazorModule>();
            builder.ObjectSpaceProviders
#if LogInFirst
               .AddSecuredEFCore().WithDbContext<ServiceDBContext>((application, options) => {
                    string connectionString = null;
                    if (Configuration.GetConnectionString("ConnectionString") != null) {
                        connectionString = Configuration.GetConnectionString("ConnectionString");
                    }
                    ArgumentNullException.ThrowIfNull(connectionString);
                    options.UseSqlServer(connectionString);
                    options.UseChangeTrackingProxies();
                    options.UseObjectSpaceLinkProxies();
                })
#endif
                .AddSecuredEFCore().WithDbContext<SAASExample.Module.BusinessObjects.SAASExampleEFCoreDbContext>((serviceProvider, options) => {
                    // Uncomment this code to use an in-memory database. This database is recreated each time the server starts. With the in-memory database, you don't need to make a migration when the data model is changed.
                    // Do not use this code in production environment to avoid data loss.
                    // We recommend that you refer to the following help topic before you use an in-memory database: https://docs.microsoft.com/en-us/ef/core/testing/in-memory
                    //options.UseInMemoryDatabase("InMemory");
                    string connectionString = null;
                    if(Configuration.GetConnectionString("ConnectionString") != null) {
                        connectionString = Configuration.GetConnectionString("ConnectionString");
                    }
#if EASYTEST
                    if(Configuration.GetConnectionString("EasyTestConnectionString") != null) {
                        connectionString = Configuration.GetConnectionString("EasyTestConnectionString");
                    }
#endif
#if TenantFirstOneDatabase
                    ArgumentNullException.ThrowIfNull(connectionString);
                    options.UseSqlServer(connectionString);
#endif
                    options.UseChangeTrackingProxies();
                    options.UseObjectSpaceLinkProxies();
                    options.UseLazyLoadingProxies();
                })
                .AddNonPersistent();
            builder.Security
                .UseIntegratedMode(options => {
                    options.RoleType = typeof(PermissionPolicyRole);
                    // ApplicationUser descends from PermissionPolicyUser and supports the OAuth authentication. For more information, refer to the following topic: https://docs.devexpress.com/eXpressAppFramework/402197
                    // If your application uses PermissionPolicyUser or a custom user type, set the UserType property as follows:
                    options.UserType = typeof(SAASExample.Module.BusinessObjects.ApplicationUser);
                    // ApplicationUserLoginInfo is only necessary for applications that use the ApplicationUser user type.
                    // If you use PermissionPolicyUser or a custom user type, comment out the following line:
                    options.UserLoginInfoType = typeof(SAASExample.Module.BusinessObjects.ApplicationUserLoginInfo);
                });
        });
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => {
            options.LoginPath = "/LoginPage";
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        if(env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
        }
        else {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. To change this for production scenarios, see: https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseRequestLocalization();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseXaf();
        app.UseEndpoints(endpoints => {
            endpoints.MapXafEndpoints();
            endpoints.MapBlazorHub();
            endpoints.MapFallbackToPage("/_Host");
            endpoints.MapControllers();
        });
    }
}
