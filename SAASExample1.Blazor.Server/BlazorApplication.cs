using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.SystemModule;
using SAASExample1.Module.BusinessObjects;
using Microsoft.EntityFrameworkCore;
using DevExpress.ExpressApp.EFCore;
using DevExpress.EntityFrameworkCore.Security;
using SAASExample1.Module.Services;
using System.Collections.Concurrent;
using SAASExample1.Module;
using DevExpress.ExpressApp.Core;
using DevExpress.Persistent.BaseImpl.EF;

namespace SAASExample1.Blazor.Server;

public class SAASExample1BlazorApplication : BlazorApplication {
    public SAASExample1BlazorApplication() {
        ApplicationName = "SAASExample1";
        CheckCompatibilityType = DevExpress.ExpressApp.CheckCompatibilityType.DatabaseSchema;
        DatabaseVersionMismatch += SAASExample1BlazorApplication_DatabaseVersionMismatch;
        CreateCustomUserModelDifferenceStore += SAASExample1BlazorApplication_CreateCustomUserModelDifferenceStore;
    }

    private void SAASExample1BlazorApplication_CreateCustomUserModelDifferenceStore(object sender, CreateCustomModelDifferenceStoreEventArgs e) {
        var logonParameters = ((BlazorApplication)sender).ServiceProvider?.GetService<ILogonParameterProvider>()?.GetLogonParameters(typeof(CustomLogonParametersForStandardAuthentication)) as CustomLogonParametersForStandardAuthentication;
        string resourceName = null;
        if (logonParameters.CompanyName == null) {
            resourceName = "ServiceModel";
        } else {
            resourceName = "CompaniesModel";
            e.AddExtraDiffStore("CompanyDifferences", new ModelDifferenceDbStore((XafApplication)sender, typeof(ModelDifference), true, logonParameters.CompanyName.Name));
        }
        ResourcesModelStore serviceStore = new ResourcesModelStore(typeof(SAASExample1Module).Assembly, resourceName);
        e.AddExtraDiffStore("ServiceStore", serviceStore);
    }

    private static ConcurrentDictionary<string, bool> isCompatibilityChecked = new ConcurrentDictionary<string, bool>();

    protected override bool IsCompatibilityChecked {
        get => isCompatibilityChecked.ContainsKey(ServiceProvider.GetRequiredService<IConnectionStringProvider>().GetConnectionString());

        set => isCompatibilityChecked.TryAdd(ServiceProvider.GetRequiredService<IConnectionStringProvider>().GetConnectionString(), value);
    }
    protected override void OnSetupStarted() {
        base.OnSetupStarted();
#if DEBUG
        if(System.Diagnostics.Debugger.IsAttached && CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema) {
            DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
        }
#endif
    }
    private void SAASExample1BlazorApplication_DatabaseVersionMismatch(object sender, DatabaseVersionMismatchEventArgs e) {
#if EASYTEST
        e.Updater.Update();
        e.Handled = true;
#else
        if(System.Diagnostics.Debugger.IsAttached) {
            e.Updater.Update();
            e.Handled = true;
        }
        else {
            string message = "The application cannot connect to the specified database, " +
                "because the database doesn't exist, its version is older " +
                "than that of the application or its schema does not match " +
                "the ORM data model structure. To avoid this error, use one " +
                "of the solutions from the https://www.devexpress.com/kb=T367835 KB Article.";

            if(e.CompatibilityError != null && e.CompatibilityError.Exception != null) {
                message += "\r\n\r\nInner exception: " + e.CompatibilityError.Exception.Message;
            }
            throw new InvalidOperationException(message);
        }
#endif
    }
}
