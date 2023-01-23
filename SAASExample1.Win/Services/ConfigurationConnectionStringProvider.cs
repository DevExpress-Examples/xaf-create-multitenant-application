using SAASExample1.Module.Interfaces;
using System.Configuration;


namespace SAASExample1.Win.Services {
    public class ConfigurationConnectionStringProvider : IConfigurationConnectionStringProvider {
        private string connectionString = null;
        public string GetConnectionString() {
            if ((connectionString == null) && ConfigurationManager.ConnectionStrings["ConnectionString"] != null) {
                connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
            return connectionString;
        }
    }
}
