using System.Collections.Generic;

namespace SAASExample1.Module.Services;
public interface ICompanyNamesHelper {
    IDictionary<string, string> GetCompanyNamesMap();
}
