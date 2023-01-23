using SAASExample1.Module.BusinessObjects;

namespace SAASExample1.Module.Services;
public interface ICompany {
    CompanyNameHolder CompanyName { get; set; }
}
