//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
//using DevExpress.Persistent.BaseImpl.EF;
//using DevExpress.ExpressApp.Design;
//using DevExpress.ExpressApp.EFCore.DesignTime;
//using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
//using Microsoft.Extensions.Options;
//using System.Security;
//using MultiTenancyExtension.BusinessObjects;
//using MultiTenancyExtension.Security;

//namespace MultiTenancyExample.Module.BusinessObjects;

//// This code allows our Model Editor to get relevant EF Core metadata at design time.
//// For details, please refer to https://supportcenter.devexpress.com/ticket/details/t933891.
//public class ServiceDBContextInitializer : DbContextTypesInfoInitializerBase {
//    protected override DbContext CreateDbContext() {
//        var optionsBuilder = new DbContextOptionsBuilder<ServiceDBContext>()
//            .UseSqlServer(";")
//            .UseChangeTrackingProxies()
//            .UseObjectSpaceLinkProxies();
//        return new ServiceDBContext(optionsBuilder.Options);
//    }
//}
////This factory creates DbContext for design-time services. For example, it is required for database migration.
//public class ServiceDesignTimeDbContextFactory : IDesignTimeDbContextFactory<ServiceDBContext> {
//    public ServiceDBContext CreateDbContext(string[] args) {
//        throw new InvalidOperationException("Make sure that the database connection string and connection provider are correct. After that, uncomment the code below and remove this exception.");
//        //var optionsBuilder = new DbContextOptionsBuilder<MultiTenancyExample2EFCoreDbContext>();
//        //optionsBuilder.UseSqlServer("Integrated Security=SSPI;Pooling=false;Data Source=(localdb)\\mssqllocaldb;Initial Catalog=MultiTenancyExample2");
//        //optionsBuilder.UseChangeTrackingProxies();
//        //optionsBuilder.UseObjectSpaceLinkProxies();
//        //return new MultiTenancyExample2EFCoreDbContext(optionsBuilder.Options);
//    }
//}
//[TypesInfoInitializer(typeof(ServiceDBContextInitializer))]
//public class ServiceDBContext : DbContext {
//    public ServiceDBContext(DbContextOptions<ServiceDBContext> options) : base(options) {
//    }
//    public DbSet<PermissionPolicyRole> Roles { get; set; }
//    public DbSet<ApplicationUser> Users { get; set; }
//    public DbSet<ApplicationUserLoginInfo> UserLoginInfos { get; set; }
//    public DbSet<MultiTenancyPermissionPolicyUserWithTenants> MultiTenancyUsers { get; set; }
//    public DbSet<TenantObject> Tenants { get; set; }
//    public DbSet<TenantObjectWithUsers> Tenants1 { get; set; }
//    public DbSet<ModelDifference> ModelDifferences { get; set; }
//    public DbSet<ModelDifferenceAspect> ModelDifferenceAspects { get; set; }
//    protected override void OnModelCreating(ModelBuilder modelBuilder) {
//        base.OnModelCreating(modelBuilder);
//        modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangingAndChangedNotificationsWithOriginalValues);
//        modelBuilder.Entity<ModelDifference>()
//           .HasMany(t => t.Aspects)
//           .WithOne(t => t.Owner)
//           .OnDelete(DeleteBehavior.Cascade);
//        modelBuilder.Entity<MultiTenancyExample.Module.BusinessObjects.ApplicationUserLoginInfo>(b => {
//            b.HasIndex(nameof(DevExpress.ExpressApp.Security.ISecurityUserLoginInfo.LoginProviderName), nameof(DevExpress.ExpressApp.Security.ISecurityUserLoginInfo.ProviderUserKey)).IsUnique();
//        });
//    }
//}