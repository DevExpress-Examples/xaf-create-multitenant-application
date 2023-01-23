using DevExpress.ExpressApp.EFCore.Updating;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.ExpressApp.Design;
using DevExpress.ExpressApp.EFCore.DesignTime;
using SAASExample1.Module.Services;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.Extensions.Options;

namespace SAASExample1.Module.BusinessObjects;

// This code allows our Model Editor to get relevant EF Core metadata at design time.
// For details, please refer to https://supportcenter.devexpress.com/ticket/details/t933891.
public class SAASExample1ContextInitializer : DbContextTypesInfoInitializerBase {
    protected override DbContext CreateDbContext() {
		var optionsBuilder = new DbContextOptionsBuilder<SAASExample1EFCoreDbContext>()
            .UseSqlServer(";")
            .UseChangeTrackingProxies()
            .UseObjectSpaceLinkProxies();
        return new SAASExample1EFCoreDbContext(optionsBuilder.Options, null);
	}
}
//This factory creates DbContext for design-time services. For example, it is required for database migration.
public class SAASExample1DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SAASExample1EFCoreDbContext> {
	public SAASExample1EFCoreDbContext CreateDbContext(string[] args) {
		throw new InvalidOperationException("Make sure that the database connection string and connection provider are correct. After that, uncomment the code below and remove this exception.");
		//var optionsBuilder = new DbContextOptionsBuilder<SAASExample1EFCoreDbContext>();
		//optionsBuilder.UseSqlServer("Integrated Security=SSPI;Pooling=false;Data Source=(localdb)\\mssqllocaldb;Initial Catalog=SAASExample1");
        //optionsBuilder.UseChangeTrackingProxies();
        //optionsBuilder.UseObjectSpaceLinkProxies();
		//return new SAASExample1EFCoreDbContext(optionsBuilder.Options);
	}
}
[TypesInfoInitializer(typeof(SAASExample1ContextInitializer))]
public class SAASExample1EFCoreDbContext : DbContext {
    readonly IConnectionStringProvider connectionStringProvider;
    public SAASExample1EFCoreDbContext(DbContextOptions<SAASExample1EFCoreDbContext> options, IConnectionStringProvider connectionStringProvider) : base(options) {
        this.connectionStringProvider = connectionStringProvider;
    }
	//public DbSet<ModuleInfo> ModulesInfo { get; set; }
	public DbSet<ModelDifference> ModelDifferences { get; set; }
	public DbSet<ModelDifferenceAspect> ModelDifferenceAspects { get; set; }
	public DbSet<PermissionPolicyRole> Roles { get; set; }
	public DbSet<SAASExample1.Module.BusinessObjects.ApplicationUser> Users { get; set; }
    public DbSet<SAASExample1.Module.BusinessObjects.ApplicationUserLoginInfo> UserLoginInfos { get; set; }
    public DbSet<SAASExample1.Module.BusinessObjects.Company> Companies { get; set; }
    public DbSet<SAASExample1.Module.BusinessObjects.Payment> Payments { get; set; }
    public DbSet<SAASExample1.Module.BusinessObjects.Position> Positions { get; set; }
    public DbSet<SAASExample1.Module.BusinessObjects.Employee> Employees { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangingAndChangedNotificationsWithOriginalValues);
        modelBuilder.Entity<SAASExample1.Module.BusinessObjects.ApplicationUserLoginInfo>(b => {
            b.HasIndex(nameof(DevExpress.ExpressApp.Security.ISecurityUserLoginInfo.LoginProviderName), nameof(DevExpress.ExpressApp.Security.ISecurityUserLoginInfo.ProviderUserKey)).IsUnique();
        });
        modelBuilder.Entity<ModelDifference>()
            .HasMany(t => t.Aspects)
            .WithOne(t => t.Owner)
            .OnDelete(DeleteBehavior.Cascade);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        base.OnConfiguring(optionsBuilder);
        //Configure the connection string based on logon parameter values.
        if (!optionsBuilder.IsConfigured) {
            string connectionString = connectionStringProvider.GetConnectionString();
            optionsBuilder.UseSqlServer(connectionString);
            optionsBuilder.UseChangeTrackingProxies();
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
