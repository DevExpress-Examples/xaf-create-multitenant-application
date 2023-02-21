using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.Blazor;
using SAASExtension.Interfaces;
using System.Collections.Concurrent;

namespace SAASExample.Blazor.Server;

public class SAASExampleBlazorApplication : BlazorApplication {
    public SAASExampleBlazorApplication() {
        ApplicationName = "SAASExample";
        CheckCompatibilityType = DevExpress.ExpressApp.CheckCompatibilityType.DatabaseSchema;
        DatabaseVersionMismatch += SAASExampleBlazorApplication_DatabaseVersionMismatch;
    }
#if !TenantFirstOneDatabase
    private static ConcurrentDictionary<string, bool> isCompatibilityChecked = new ConcurrentDictionary<string, bool>();

    protected override bool IsCompatibilityChecked {
        get => isCompatibilityChecked.ContainsKey(ServiceProvider.GetRequiredService<IConnectionStringProvider>().GetConnectionString());
        set => isCompatibilityChecked.TryAdd(ServiceProvider.GetRequiredService<IConnectionStringProvider>().GetConnectionString(), value);
    }
#endif
    protected override void OnSetupStarted() {
        base.OnSetupStarted();
#if DEBUG
        if(System.Diagnostics.Debugger.IsAttached && CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema) {
            DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
        }
#endif
    }
    private void SAASExampleBlazorApplication_DatabaseVersionMismatch(object sender, DatabaseVersionMismatchEventArgs e) {
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
