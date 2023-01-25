using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Win.Utils;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using Microsoft.EntityFrameworkCore;
using DevExpress.ExpressApp.EFCore;
using DevExpress.EntityFrameworkCore.Security;
using SAASExample1.Module;
using SAASExample1.Module.BusinessObjects;
using System.Data.Common;
using DevExpress.Persistent.BaseImpl.EF;
using Microsoft.Extensions.DependencyInjection;
using SAASExample1.Module.Services;
using System.Collections.Concurrent;

namespace SAASExample1.Win;

// For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Win.WinApplication._members
public class SAASExample1WindowsFormsApplication : WinApplication {
    private static ConcurrentDictionary<string, bool> isCompatibilityChecked = new ConcurrentDictionary<string, bool>();
    public SAASExample1WindowsFormsApplication() {
		SplashScreen = new DXSplashScreen(typeof(XafSplashScreen), new DefaultOverlayFormOptions());
        ApplicationName = "SAASExample1";
        CheckCompatibilityType = DevExpress.ExpressApp.CheckCompatibilityType.DatabaseSchema;
        UseOldTemplates = false;
        DatabaseVersionMismatch += SAASExample1WindowsFormsApplication_DatabaseVersionMismatch;
        CustomizeLanguagesList += SAASExample1WindowsFormsApplication_CustomizeLanguagesList;
        CreateCustomUserModelDifferenceStore += SAASExample1WindowsFormsApplication_CreateCustomUserModelDifferenceStore;
    }
    private void SAASExample1WindowsFormsApplication_CreateCustomUserModelDifferenceStore(object sender, CreateCustomModelDifferenceStoreEventArgs e) {
        var logonParameters = ((ILogonParameterProvider)((WinApplication)sender).ServiceProvider?.GetService(typeof(ILogonParameterProvider)))?.GetLogonParameters(typeof(CustomLogonParametersForStandardAuthentication)) as CustomLogonParametersForStandardAuthentication;
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
    protected override bool IsCompatibilityChecked {
        get => isCompatibilityChecked.ContainsKey(ServiceProvider.GetRequiredService<IConnectionStringProvider>().GetConnectionString());

        set => isCompatibilityChecked.TryAdd(ServiceProvider.GetRequiredService<IConnectionStringProvider>().GetConnectionString(), value);
    }
    private void SAASExample1WindowsFormsApplication_CustomizeLanguagesList(object sender, CustomizeLanguagesListEventArgs e) {
        string userLanguageName = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
        if(userLanguageName != "en-US" && e.Languages.IndexOf(userLanguageName) == -1) {
            e.Languages.Add(userLanguageName);
        }
    }
    private void SAASExample1WindowsFormsApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e) {
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
				"because the database doesn't exist, its version is older " +
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
