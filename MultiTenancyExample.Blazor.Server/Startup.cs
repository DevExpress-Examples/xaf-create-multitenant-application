using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.Blazor.ApplicationBuilder;
using DevExpress.ExpressApp.Blazor.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.EntityFrameworkCore;
using MultiTenancyExample.Blazor.Server.Services;
using DevExpress.ExpressApp.MultiTenancy.Blazor;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using MultiTenancyExample.Module;
using DevExpress.ExpressApp.MultiTenancy.Security;
using DevExpress.ExpressApp.MultiTenancy.BusinessObjects;
using DevExpress.ExpressApp.MultiTenancy.Services;
using MultiTenancyExample.Module.BusinessObjects;
using DevExpress.ExpressApp.MultiTenancy.Controllers;
using DevExpress.ExpressApp.MultiTenancy.Interfaces;
using DevExpress.ExpressApp.MultiTenancy;
using System.Collections.ObjectModel;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Security;
using static System.Net.Mime.MediaTypeNames;

namespace MultiTenancyExample.Blazor.Server;

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
            builder.UseApplication<MultiTenancyExampleBlazorApplication>();

#if TenantFirst
            builder
            .AddMultiTenancyModelDifferenceStore(mds => {
                mds.Assembly = typeof(MultiTenancyExampleModule).Assembly;
            })
            .MakeMultiTenancy()
            .MultipleDatabases(builder => {
                ((IBlazorApplicationBuilder)builder).ObjectSpaceProviders.AddSecuredEFCore().WithDbContext<MultiTenancyExampleEFCoreDbContext>((serviceProvider, options) => {
                    options.UseDefaultSQLServerMultiTenancyOptions(serviceProvider);
                });
            })
            .TenantFirst<ApplicationUser>();
            //.AddSelectTenantsLogonController();
            builder.Security
                .AddMultiTenancyPasswordAuthentication(options => {
                    options.IsSupportChangePassword = true;
                });
#endif
#if TenantFirstOneDatabase
            builder
            .AddMultiTenancyModelDifferenceStore(mds => {
                mds.Assembly = typeof(MultiTenancyExampleModule).Assembly;
                mds.ServiceModelResourceName = "ServiceModelOneDatabase";
            })
            .MakeMultiTenancy()
            .OneDatabase()
            .TenantFirst()
            .AddSelectTenantsLogonController();
            builder.Security.AddMultiTenancyPasswordAuthentication<CustomAuthenticationStandardProvider>(options => {
                 options.IsSupportChangePassword = true;
             });
            builder.ObjectSpaceProviders
            .AddSecuredEFCore().WithDbContext<MultiTenancyExampleEFCoreDbContext>((serviceProvider, options) => {
                options.UseDefaultSQLServerOptions(serviceProvider);
            });
#endif
#if LogInFirst
            builder
            .AddMultiTenancyModelDifferenceStore(mds => {
                mds.Assembly = typeof(MultiTenancyExampleModule).Assembly;
                mds.ServiceModelResourceName = "ExtendedServiceModel";
                mds.ProductionModelResourceName = "LiteProductionModel";
            })
            .MakeMultiTenancy()
            .MultipleDatabases(builder => {
                ((IBlazorApplicationBuilder)builder).ObjectSpaceProviders.AddSecuredEFCore().WithDbContext<MultiTenancyExampleEFCoreDbContext>((serviceProvider, options) => {
                    options.UseDefaultSQLServerMultiTenancyOptions(serviceProvider);
                });
            }).LogInFirst<ServiceDBContext<ApplicationUser, ApplicationUserLoginInfo>>()
            .AddSelectUserTenantsLogonController();
            builder.Security
                .AddMultiTenancyPasswordAuthentication(options => {
                    options.IsSupportChangePassword = true;
                });
#endif
#if LogInFirstOneDatabase
            builder
            .AddMultiTenancyModelDifferenceStore(mds => {
                mds.Assembly = typeof(MultiTenancyExampleModule).Assembly;
                mds.ServiceModelResourceName = "ExtendedServiceModel";
                mds.ProductionModelResourceName = "LiteProductionModel";
            })
            .MakeMultiTenancy()
            .OneDatabase()
            .LogInFirst<ServiceDBContext<ApplicationUser, ApplicationUserLoginInfo>>()
            .AddSelectUserTenantsLogonController();
            builder.Security
                .AddMultiTenancyPasswordAuthentication(options => {
                    options.IsSupportChangePassword = true;
                });
            builder.ObjectSpaceProviders
                .AddSecuredEFCore().WithDbContext<MultiTenancyExampleEFCoreDbContext>((serviceProvider, options) => {
                    options.UseDefaultSQLServerOptions(serviceProvider);
                });
#endif
#if PredefinedTenant
            builder
            .AddMultiTenancyModelDifferenceStore(mds => {
                mds.Assembly = typeof(MultiTenancyExampleModule).Assembly;
                mds.ServiceModelResourceName = "PredefinedTenantServiceModel";
                mds.ProductionModelResourceName = "LiteProductionModel";
            })
            .MakeMultiTenancy()
            .MultipleDatabases(builder => {
                ((IBlazorApplicationBuilder)builder).ObjectSpaceProviders.AddSecuredEFCore().WithDbContext<MultiTenancyExampleEFCoreDbContext>((serviceProvider, options) => {
                    options.UseDefaultSQLServerMultiTenancyOptions(serviceProvider);
                });
            })
            .PredefinedTenant<ServiceDBContext<ApplicationUser, ApplicationUserLoginInfo>>();
            builder.Security
                .AddMultiTenancyPasswordAuthentication(options => {
                    options.IsSupportChangePassword = true;
                });
#endif
#if PredefinedTenantOneDatabase
            builder
            .AddMultiTenancyModelDifferenceStore(mds => {
                mds.Assembly = typeof(MultiTenancyExampleModule).Assembly;
                mds.ServiceModelResourceName = "PredefinedTenantServiceModel";
                mds.ProductionModelResourceName = "LiteProductionModel";
            })
            .MakeMultiTenancy()
            .OneDatabase()
            .PredefinedTenant<ServiceDBContext<ApplicationUser, ApplicationUserLoginInfo>>();
            builder.Security
                .AddMultiTenancyPasswordAuthentication(options => {
                    options.IsSupportChangePassword = true;
                });
            builder.ObjectSpaceProviders
               .AddSecuredEFCore().WithDbContext<MultiTenancyExampleEFCoreDbContext>((serviceProvider, options) => {
                options.UseDefaultSQLServerOptions(serviceProvider);
});
#endif
            builder.Modules
                .AddConditionalAppearance()
                .AddValidation(options => {
                    options.AllowValidationDetailsAccess = false;
                })
                .Add<MultiTenancyExampleModule>()
            	.Add<MultiTenancyExampleBlazorModule>();
            builder.ObjectSpaceProviders.AddNonPersistent();
            builder.Security
                .UseIntegratedMode(options => {
                    options.RoleType = typeof(PermissionPolicyRole);
                    options.UserType = typeof(ApplicationUser);
                    options.UserLoginInfoType = typeof(ApplicationUserLoginInfo);
                    options.Events.OnCustomizeSecurityCriteriaOperator = context => {
                        DevExpress.ExpressApp.Utils.Guard.ArgumentNotNull(context.ServiceProvider, nameof(context.ServiceProvider));
                        if (context.Operator is FunctionOperator functionOperator) {
                            if (functionOperator.Operands.Count == 1 && "CurrentTenant".Equals((functionOperator.Operands[0] as ConstantValue)?.Value?.ToString(), StringComparison.InvariantCultureIgnoreCase)) {
                                context.Result = new ConstantValue(((ITenantName)context.ServiceProvider.GetService<ILogonParameterProvider>()?.GetLogonParameters(typeof(ITenantName)))?.TenantName);
                            }
                        }
                    };
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
