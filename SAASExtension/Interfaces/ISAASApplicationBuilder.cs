using Microsoft.Extensions.DependencyInjection;
using SAASExtension.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAASExtension.Interfaces {
    public interface ISAASApplicationBuilder {
        IMultipleDatabaseBuilder MultipleDatabases();
        IMultipleDatabaseBuilder MultipleDatabases<TConnectionStringProvider, TConfigurationConnectionStringProvider>()
             where TConnectionStringProvider : class, IConnectionStringProvider
             where TConfigurationConnectionStringProvider : class, IConfigurationConnectionStringProvider;
        IOneDatabaseBuilder OneDatabase();
    }
    public interface IOneDatabaseBuilder {
        IOneDatabaseTenantFirstBuilder TenantFirst();
        IOneDatabaseLogInFirstBuilder LogInFirst();
    }
    public interface IMultipleDatabaseBuilder {
        IMultipleDatabaseTenantFirstBuilder TenantFirst();
        IMultipleDatabaseLogInFirstBuilder LogInFirst();
        IMultipleDatabaseLogInFirstBuilder LogInFirst<TTenantNamesHelper>() where TTenantNamesHelper : class, ITenantNamesHelper;
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
