using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using Microsoft.EntityFrameworkCore;
using SAASExtension.Interfaces;
using System;

namespace SAASExtension.Objects {
    public class TenantObjectDbContext : DbContext { 
        public TenantObjectDbContext(DbContextOptions<TenantObjectDbContext> options) : base(options) {
        }
        public DbSet<SAASExtension.BusinessObjects.TenantObject> Companies { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangingAndChangedNotificationsWithOriginalValues);
        }
    }
    public class TenantWithConnectionStringDbContext<TUserType> : DbContext
        where TUserType : PermissionPolicyUser {
        public TenantWithConnectionStringDbContext(DbContextOptions<TenantWithConnectionStringDbContext<TUserType>> options) : base(options) {
        }
        public DbSet<TUserType> Users { get; set; }
        public DbSet<PermissionPolicyRole> Roles { get; set; }
        public DbSet<ModelDifference> ModelDifferences { get; set; }
        public DbSet<ModelDifferenceAspect> ModelDifferenceAspects { get; set; }
        public DbSet<SAASExtension.BusinessObjects.TenantObject> Companies { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangingAndChangedNotificationsWithOriginalValues);
        }
    }
}
