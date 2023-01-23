using SAASExample1.Module.Interfaces;

namespace SAASExample1.Blazor.Server.Services {
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
    }
}
