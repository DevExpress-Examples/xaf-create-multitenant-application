using System.Text;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace OutlookInspired.MiddleTier.Extensions{
    public static class ServiceCollection{
        public static void AddMiddleTier(this IServiceCollection services,IConfiguration configuration) 
            => services.AddXafMiddleTier(configuration, builder => {
                builder.Modules
                    .AddReports(options => options.ReportDataType = typeof(DevExpress.Persistent.BaseImpl.EF.ReportDataV2))
                    .Add<Module.OutlookInspiredModule>();
                builder.ObjectSpaceProviders
                    .AddSecuredEFCore()
                    .WithDbContext<Module.BusinessObjects.OutlookInspiredEFCoreDbContext>((_, options) => {
                        // Uncomment this code to use an in-memory database. This database is recreated each time the server starts. With the in-memory database, you don't need to make a migration when the data model is changed.
                        // Do not use this code in production environment to avoid data loss.
                        // We recommend that you refer to the following help topic before you use an in-memory database: https://docs.microsoft.com/en-us/ef/core/testing/in-memory
                        //options.UseInMemoryDatabase("InMemory");
                        string connectionString = null;
                        if(configuration.GetConnectionString("ConnectionString") != null) {
                            connectionString = configuration.GetConnectionString("ConnectionString");
                        }
#if EASYTEST
                        if(Configuration.GetConnectionString("EasyTestConnectionString") != null) {
                            connectionString = Configuration.GetConnectionString("EasyTestConnectionString");
                        }
#endif
                        ArgumentNullException.ThrowIfNull(connectionString);
                        options.UseSqlServer(connectionString);
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
                        options.UserType = typeof(Module.BusinessObjects.ApplicationUser);
                        // ApplicationUserLoginInfo is only necessary for applications that use the ApplicationUser user type.
                        // If you use PermissionPolicyUser or a custom user type, comment out the following line:
                        options.UserLoginInfoType = typeof(Module.BusinessObjects.ApplicationUserLoginInfo);
                        options.Events.OnSecurityStrategyCreated = _ => {
                            //((SecurityStrategy)securityStrategy).AnonymousAllowedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.EF.ModelDifference));
                            //((SecurityStrategy)securityStrategy).AnonymousAllowedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.EF.ModelDifferenceAspect));
                        };
                    })
                    .AddPasswordAuthentication(options => {
                        options.IsSupportChangePassword = true;
                    });

                builder.AddBuildStep(application => {
                    application.ApplicationName = "SetupApplication.OutlookInspired";
                    application.CheckCompatibilityType = CheckCompatibilityType.DatabaseSchema;
                    //application.CreateCustomModelDifferenceStore += += (s, e) => {
                    //    e.Store = new ModelDifferenceDbStore((XafApplication)sender!, typeof(ModelDifference), true, "Win");
                    //    e.Handled = true;
                    //};
#if DEBUG
                    if(System.Diagnostics.Debugger.IsAttached && application.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema) {
                        application.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
                        application.DatabaseVersionMismatch += (_, e) => {
                            e.Updater.Update();
                            e.Handled = true;
                        };
                    }
#endif
                });
            });

        public static AuthenticationBuilder AddAuthentication(this IServiceCollection services,IConfiguration configuration) 
            => services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters() {
                        ValidateIssuerSigningKey = true,
                        //ValidIssuer = Configuration["Authentication:Jwt:Issuer"],
                        //ValidAudience = Configuration["Authentication:Jwt:Audience"],
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Authentication:Jwt:IssuerSigningKey"]!))
                    };
                });

        public static IServiceCollection AddAuthorization(this IServiceCollection services,IConfiguration configuration) 
            => services.AddAuthorization(options => options.DefaultPolicy = new AuthorizationPolicyBuilder(
                    JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .RequireXafAuthentication()
                .Build());

        public static IServiceCollection AddSwagger(this IServiceCollection services) 
            => services.AddSwaggerGen(c => {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo {
                    Title = "OutlookInspired API",
                    Version = "v1",
                    Description = @"MiddleTier"
                });
                c.AddSecurityDefinition("JWT", new OpenApiSecurityScheme() {
                    Type = SecuritySchemeType.Http,
                    Name = "Bearer",
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement{{
                        new OpenApiSecurityScheme() {
                            Reference = new OpenApiReference() {
                                Type = ReferenceType.SecurityScheme,
                                Id = "JWT"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
    }
}