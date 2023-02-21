
using DevExpress.ExpressApp.DC;

namespace SAASExtension.Interfaces {
    public interface ITenant {
        string Name { get; set; }
    }
    public interface IConnectionString {
        string ConnectionString { get; set; }
    }
}
