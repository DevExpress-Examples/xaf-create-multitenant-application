using Microsoft.Extensions.Configuration;
using SAASExtension.Interfaces;
using System.Configuration;

namespace SAASExtension.Services {
    public class ConfigurationConnectionStringProvider : IConfigurationConnectionStringProvider {
        private string connectionString = null;
        private IConfiguration configuration;
        public ConfigurationConnectionStringProvider(IConfiguration configuration) {
            this.configuration = configuration;
        }
        public string GetConnectionString() {
            if(connectionString == null){
                connectionString = configuration.GetConnectionString("ConnectionString");
            }
            return connectionString;
        }
        public string GetConnectionString(string key) {
            return configuration.GetConnectionString(key);
        }
    }
    public class ConfigurationManagerConnectionStringProvider : IConfigurationConnectionStringProvider {
        private string connectionString = null;
        public string GetConnectionString() {
            if ((connectionString == null) && ConfigurationManager.ConnectionStrings["ConnectionString"] != null) {
                connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
            return connectionString;
        }

        public string GetConnectionString(string key) {
            if (ConfigurationManager.ConnectionStrings[key] != null) {
                return ConfigurationManager.ConnectionStrings[key].ConnectionString;
            }
            return null;
        }
    }
}
