using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.Security.Authentication.ClientServer;
using OutlookInspired.MiddleTier.Extensions;
using OutlookInspired.WebApi.JWT;

namespace OutlookInspired.MiddleTier;

public class Startup {
    public Startup(IConfiguration configuration) => Configuration = configuration;
    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services) {
        services.AddScoped<IAuthenticationTokenProvider, JwtTokenProviderService>();
        services.AddMiddleTier(Configuration);
        services.AddAuthentication(Configuration);
        services.AddAuthorization(Configuration);
        services.AddSwagger();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        if(env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OutlookInspired Api v1"));
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
        app.UseXafMiddleTier();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}
