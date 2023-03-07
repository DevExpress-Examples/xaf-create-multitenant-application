using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAASExtension.Interfaces {
    public interface IConfigurationConnectionStringProvider {
        string GetConnectionString();
        string GetConnectionString(string key);
    }
}
