using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.ApplicationBuilder;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using DevExpress.ExpressApp.Design;
using Microsoft.EntityFrameworkCore;
using OutlookInspired.Win.Extensions;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace OutlookInspired.Win;
public class ApplicationBuilder : IDesignTimeApplicationFactory {
    public static WinApplication BuildApplication(bool useMiddleTier){
        var builder = WinApplication.CreateBuilder();
        builder.UseApplication<OutlookInspiredWindowsFormsApplication>();
        builder.AddModules();
        builder.AddObjectSpaceProviders(options => {
            if (useMiddleTier) return false;
            options.UseSqlServer(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            return true;
        });
        builder.AddSecurity(useMiddleTier);
        builder.AddBuildStep(application => application.DatabaseUpdateMode = DatabaseUpdateMode.Never);
        return builder.Build();
    }
    
    XafApplication IDesignTimeApplicationFactory.Create() {
        MiddleTierClientSecurityBase.DesignModeUserType = typeof(Module.BusinessObjects.ApplicationUser);
        MiddleTierClientSecurityBase.DesignModeRoleType = typeof(PermissionPolicyRole);
        return BuildApplication(false);
    }
}
