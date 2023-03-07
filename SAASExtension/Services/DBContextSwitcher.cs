using DevExpress.ExpressApp.EFCore;

namespace SAASExtension.Services {
    public class DBContextSwitcher : IDBContextSwitcher {
        public bool UseStandaloneDBContext { get; set; }
    }
}
