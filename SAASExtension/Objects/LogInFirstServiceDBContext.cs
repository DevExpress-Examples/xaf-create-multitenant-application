using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.ExpressApp.Design;
using DevExpress.ExpressApp.EFCore.DesignTime;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.Extensions.Options;
using System.Security;
using SAASExtension.BusinessObjects;
using SAASExtension.Security;
using DevExpress.ExpressApp.Security;

namespace SAASExtension.BusinessObjects;

public class ServiceDBContext<TUserType, TApplicationUserLoginInfo> : DbContext
        where TApplicationUserLoginInfo: class, ISecurityUserLoginInfo
        where TUserType : PermissionPolicyUser {
    public ServiceDBContext(DbContextOptions<ServiceDBContext<TUserType, TApplicationUserLoginInfo>> options) : base(options) {
    }
    public DbSet<PermissionPolicyRole> Roles { get; set; }
    public DbSet<PermissionPolicyUser> Users1 { get; set; }
    public DbSet<SAASPermissionPolicyUser> Users2 { get; set; }
    public DbSet<SAASPermissionPolicyUserWithTenants> Users3 { get; set; }
    public DbSet<TUserType> Users { get; set; }
    public DbSet<TApplicationUserLoginInfo> UserLoginInfos { get; set; }
    public DbSet<TenantObjectWithUsers> Tenants { get; set; }
    public DbSet<TenantObject> Tenants1 { get; set; }
    public DbSet<ModelDifference> ModelDifferences { get; set; }
    public DbSet<ModelDifferenceAspect> ModelDifferenceAspects { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangingAndChangedNotificationsWithOriginalValues);
        modelBuilder.Entity<ModelDifference>()
           .HasMany(t => t.Aspects)
           .WithOne(t => t.Owner)
           .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<TApplicationUserLoginInfo>(b => {
            b.HasIndex(nameof(ISecurityUserLoginInfo.LoginProviderName), nameof(ISecurityUserLoginInfo.ProviderUserKey)).IsUnique();
        });
    }
}
