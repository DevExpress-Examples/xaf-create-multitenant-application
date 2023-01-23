using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAASExample1.Module.Interfaces {
    public interface IConfigurationConnectionStringProvider {
        string GetConnectionString();
    }
}
