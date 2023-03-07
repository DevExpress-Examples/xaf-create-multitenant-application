using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Tests.TestObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SAASExtension.Interfaces;
using System;
using System.ComponentModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SAASExtension {
    public static class DbContextOptionsBuilderExtension {
        public static void UseDefaultSQLServerSAASOptions(this DbContextOptionsBuilder options, IServiceProvider serviceProvider) {
            string connectionString = serviceProvider.GetRequiredService<IConnectionStringProvider>()?.GetConnectionString();
            ArgumentNullException.ThrowIfNull(connectionString);
            options.UseSqlServer(connectionString);
            options.UseChangeTrackingProxies();
            options.UseObjectSpaceLinkProxies();
            options.UseLazyLoadingProxies();
        }
        public static void UseDefaultSQLServerOptions(this DbContextOptionsBuilder options, IServiceProvider serviceProvider) {
            string connectionString = serviceProvider.GetRequiredService<IConfigurationConnectionStringProvider>()?.GetConnectionString();
            ArgumentNullException.ThrowIfNull(connectionString);
            options.UseSqlServer(connectionString);
            options.UseChangeTrackingProxies();
            options.UseObjectSpaceLinkProxies();
            options.UseLazyLoadingProxies();
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public static void UseServiceSQLServerOptions(this DbContextOptionsBuilder options, IServiceProvider serviceProvider) {
            string connectionString = serviceProvider.GetRequiredService<IConfigurationConnectionStringProvider>()?.GetConnectionString("ServiceConnectionString");
            if (connectionString == null) {
                connectionString = serviceProvider.GetRequiredService<IConfigurationConnectionStringProvider>()?.GetConnectionString();
            }
            ArgumentNullException.ThrowIfNull(connectionString);
            options.UseSqlServer(connectionString);
            options.UseChangeTrackingProxies();
            options.UseObjectSpaceLinkProxies();
            options.UseLazyLoadingProxies();
        }
    }
}
