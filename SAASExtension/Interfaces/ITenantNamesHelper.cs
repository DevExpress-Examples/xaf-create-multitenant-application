using System.Collections.Generic;

namespace SAASExtension.Interfaces;
public interface ITenantNamesHelper {
    IDictionary<string, string> GetTenantNamesMap();
}
