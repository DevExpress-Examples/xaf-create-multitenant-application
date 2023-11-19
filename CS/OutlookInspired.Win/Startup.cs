using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using DevExpress.ExpressApp.Design;
using OutlookInspired.Win.Services;

namespace OutlookInspired.Win;
public class ApplicationBuilder : IDesignTimeApplicationFactory {
    public static WinApplication BuildApplication(string connectionString) 
        => WinApplication.CreateBuilder().Configure(connectionString).Build();

    XafApplication IDesignTimeApplicationFactory.Create() {
        MiddleTierClientSecurityBase.DesignModeUserType = typeof(Module.BusinessObjects.ApplicationUser);
        MiddleTierClientSecurityBase.DesignModeRoleType = typeof(PermissionPolicyRole);
        return BuildApplication(XafApplication.DesignTimeConnectionString);
    }
}
