using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.AspNetCore.Core;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.ApplicationBuilder;
using DevExpress.ExpressApp.Blazor.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Server.Circuits;
using OutlookInspired.Blazor.Server.Services;

namespace OutlookInspired.Blazor.Server;

public class Startup {
    public Startup(IConfiguration configuration) {
        Configuration = configuration;
    }
    public IConfiguration Configuration { get; }
    
    public void ConfigureServices(IServiceCollection services) {
        services.AddSingleton(typeof(Microsoft.AspNetCore.SignalR.HubConnectionHandler<>), typeof(ProxyHubConnectionHandler<>));
        services.AddRazorPages();
        services.AddServerSideBlazor();
        services.AddHttpContextAccessor();
        services.AddScoped<CircuitHandler, CircuitHandlerProxy>();
        services.AddXaf(Configuration, builder => builder.UseApplication<OutlookInspiredBlazorApplication>().AddModules()
            .AddObjectSpaceProviders(Configuration).AddSecurity().AddBuildStep());
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => options.LoginPath = "/LoginPage");
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        if(env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
        }
        else {
            app.UseExceptionHandler("/Error");
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

public class MyClass:XafApplicationFactory<OutlookInspiredBlazorApplication>{
    public MyClass(Func<OutlookInspiredBlazorApplication> createApplication) : base(createApplication){
    }
}

public class XafApplicationFactory<T> : 
    IXafApplicationFactory,
    IXafAspNetCoreApplicationFactory<BlazorApplication>
    where T : BlazorApplication
{
    private readonly Func<T> createApplication;

    public XafApplicationFactory(Func<T> createApplication) => this.createApplication = createApplication ?? throw new ArgumentNullException(nameof (createApplication));

    public T CreateApplication() => this.createApplication();

    BlazorApplication IXafAspNetCoreApplicationFactory<BlazorApplication>.CreateApplication() => (BlazorApplication) this.CreateApplication();
}

