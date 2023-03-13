using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.ExpressApp.Design;
using DevExpress.ExpressApp.EFCore.DesignTime;
using SAASExtension.Security;
using Microsoft.Extensions.Options;

namespace SAASExample.Module.BusinessObjects;

// This code allows our Model Editor to get relevant EF Core metadata at design time.
// For details, please refer to https://supportcenter.devexpress.com/ticket/details/t933891.
public class SAASExampleContextInitializer : DbContextTypesInfoInitializerBase {
    protected override DbContext CreateDbContext() {
        var optionsBuilder = new DbContextOptionsBuilder<SAASExampleEFCoreDbContext>()
            .UseSqlServer(";")
            .UseChangeTrackingProxies()
            .UseObjectSpaceLinkProxies();
        return new SAASExampleEFCoreDbContext(optionsBuilder.Options);
    }
}
//This factory creates DbContext for design-time services. For example, it is required for database migration.
public class SAASExampleDesignTimeDbContextFactory : IDesignTimeDbContextFactory<SAASExampleEFCoreDbContext> {
	public SAASExampleEFCoreDbContext CreateDbContext(string[] args) {
		throw new InvalidOperationException("Make sure that the database connection string and connection provider are correct. After that, uncomment the code below and remove this exception.");
		//var optionsBuilder = new DbContextOptionsBuilder<SAASExampleEFCoreDbContext>();
		//optionsBuilder.UseSqlServer("Integrated Security=SSPI;Pooling=false;Data Source=(localdb)\\mssqllocaldb;Initial Catalog=SAASExample");
        //optionsBuilder.UseChangeTrackingProxies();
        //optionsBuilder.UseObjectSpaceLinkProxies();
		//return new SAASExampleEFCoreDbContext(optionsBuilder.Options);
	}
}
[TypesInfoInitializer(typeof(SAASExampleContextInitializer))]
public class SAASExampleEFCoreDbContext : DbContext {
    public SAASExampleEFCoreDbContext(DbContextOptions<SAASExampleEFCoreDbContext> options) : base(options) {
    }
#if !LogInFirst
    public DbSet<ModelDifference> ModelDifferences { get; set; }
	public DbSet<ModelDifferenceAspect> ModelDifferenceAspects { get; set; }
	public DbSet<PermissionPolicyRole> Roles { get; set; }
	public DbSet<SAASExample.Module.BusinessObjects.ApplicationUser> Users { get; set; }
    public DbSet<SAASExample.Module.BusinessObjects.ApplicationUserLoginInfo> UserLoginInfos { get; set; }
#endif
    public DbSet<SAASExample.Module.BusinessObjects.Payment> Payments { get; set; }
    public DbSet<SAASExample.Module.BusinessObjects.Position> Positions { get; set; }
    public DbSet<SAASExample.Module.BusinessObjects.Employee> Employees { get; set; }
#if OneDatabase
    public DbSet<SAASExtension.BusinessObjects.Tenant> Tenants { get; set; }
#endif
#if TenantFirstOneDatabase
    public DbSet<SAASPermissionPolicyUser> SAASUsers { get; set; }
#endif

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangingAndChangedNotificationsWithOriginalValues);
#if !LogInFirst
        modelBuilder.Entity<SAASExample.Module.BusinessObjects.ApplicationUserLoginInfo>(b => {
            b.HasIndex(nameof(DevExpress.ExpressApp.Security.ISecurityUserLoginInfo.LoginProviderName), nameof(DevExpress.ExpressApp.Security.ISecurityUserLoginInfo.ProviderUserKey)).IsUnique();
        });
        modelBuilder.Entity<ModelDifference>()
            .HasMany(t => t.Aspects)
            .WithOne(t => t.Owner)
            .OnDelete(DeleteBehavior.Cascade);
#endif
    }
}
