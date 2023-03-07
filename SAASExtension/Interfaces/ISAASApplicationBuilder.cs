using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.ApplicationBuilder.Internal;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SAASExtension.Controllers;

namespace SAASExtension.Interfaces {
    public interface ISAASApplicationBuilder {
        IMultipleDatabaseBuilder MultipleDatabases(Action<object> setupProviders);
        IMultipleDatabaseBuilder MultipleDatabases<TConnectionStringProvider, TConfigurationConnectionStringProvider>(Action<object> setupProviders)
             where TConnectionStringProvider : class, IConnectionStringProvider
             where TConfigurationConnectionStringProvider : class, IConfigurationConnectionStringProvider;
        IOneDatabaseBuilder OneDatabase();
    }
    public interface IOneDatabaseBuilder {
        IOneDatabaseTenantFirstBuilder TenantFirst();
        IOneDatabaseTenantFirstBuilder TenantFirst<TTenantNamesHelper>(bool useBuiltInTypes = false) where TTenantNamesHelper : class, ITenantNamesHelper;
        IOneDatabaseLogInFirstBuilder LogInFirst();
    }
    public interface IMultipleDatabaseBuilder {
        IMultipleDatabaseTenantFirstBuilder TenantFirst();
        IMultipleDatabaseTenantFirstBuilder TenantFirst<TUserType>() where TUserType : PermissionPolicyUser;
        IMultipleDatabaseTenantFirstBuilder TenantFirst<TTenantNamesHelper>(bool useBuiltInTypes = false) where TTenantNamesHelper : class, ITenantNamesHelper;
        IMultipleDatabaseLogInFirstBuilder LogInFirst<TContext>() where TContext : DbContext;
        IMultipleDatabaseLogInFirstBuilder LogInFirst<TTenantNamesHelper, TContext>(bool useBuiltInTypes = false)
            where TTenantNamesHelper : class, ITenantNamesHelper
            where TContext : DbContext;
        }
    public interface IMultipleDatabaseTenantFirstBuilder {
        void AddSelectTenantsLogonController(Action<SelectTenantController> configure = null);
    }
    public interface IMultipleDatabaseLogInFirstBuilder {
        void AddSelectUserTenantsLogonController();
        void AddSelectUserTenantsStartupAction();
        IMultipleDatabaseLogInFirstBuilder AddSelectTenantsRunTimeController();
    }
    public interface IOneDatabaseTenantFirstBuilder {
        void AddSelectTenantsLogonController(Action<SelectTenantController> configure = null);
    }
    public interface IOneDatabaseLogInFirstBuilder {
    }
}
