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
using SAASExtension;

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
            .MultipleDatabases(builder => {
                ((IBlazorApplicationBuilder)builder).ObjectSpaceProviders.AddSecuredEFCore().WithDbContext<SAASExampleEFCoreDbContext>((serviceProvider, options) => {
                    options.UseDefaultSQLServerSAASOptions(serviceProvider);
                });
            })
            .TenantFirst<ApplicationUser>();
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
            builder.ObjectSpaceProviders
            .AddSecuredEFCore().WithDbContext<SAASExampleEFCoreDbContext>((serviceProvider, options) => {
                options.UseDefaultSQLServerSAASOptions(serviceProvider);
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
                ((IBlazorApplicationBuilder)builder).ObjectSpaceProviders.AddSecuredEFCore().WithDbContext<SAASExampleEFCoreDbContext>((serviceProvider, options) => {
                    options.UseDefaultSQLServerSAASOptions(serviceProvider);
                });
            }).LogInFirst<ServiceDBContext<ApplicationUser, ApplicationUserLoginInfo>>()
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
                .Add<SAASExampleModule>()
            	.Add<SAASExampleBlazorModule>();
            builder.ObjectSpaceProviders.AddNonPersistent();
            builder.Security
                .UseIntegratedMode(options => {
                    options.RoleType = typeof(PermissionPolicyRole);
                    options.UserType = typeof(ApplicationUser);
                    options.UserLoginInfoType = typeof(ApplicationUserLoginInfo);
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
