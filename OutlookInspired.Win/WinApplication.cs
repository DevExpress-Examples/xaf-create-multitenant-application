using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Win.Utils;

namespace OutlookInspired.Win;
public class OutlookInspiredWindowsFormsApplication : WinApplication{
    public OutlookInspiredWindowsFormsApplication() {
		SplashScreen = new DXSplashScreen(typeof(XafDemoSplashScreen), new DefaultOverlayFormOptions());
        ApplicationName = "OutlookInspired";
        CheckCompatibilityType = CheckCompatibilityType.DatabaseSchema;
        UseOldTemplates = false;
        DatabaseVersionMismatch += OutlookInspiredWindowsFormsApplication_DatabaseVersionMismatch;
        CustomizeLanguagesList += OutlookInspiredWindowsFormsApplication_CustomizeLanguagesList;
    }
    
    private void OutlookInspiredWindowsFormsApplication_CustomizeLanguagesList(object sender, CustomizeLanguagesListEventArgs e) {
        string userLanguageName = Thread.CurrentThread.CurrentUICulture.Name;
        if(userLanguageName != "en-US" && e.Languages.IndexOf(userLanguageName) == -1) {
            e.Languages.Add(userLanguageName);
        }
    }
    private void OutlookInspiredWindowsFormsApplication_DatabaseVersionMismatch(object sender, DatabaseVersionMismatchEventArgs e){
		string message = "Application cannot connect to the specified database.";

		if(e.CompatibilityError is CompatibilityDatabaseIsOldError{ Module: not null } isOldError) {
			message = "The client application cannot connect to the Middle Tier Application Server and its database. " +
					  "To avoid this error, ensure that both the client and the server have the same modules set. Problematic module: " + isOldError.Module.Name +
					  ". For more information, see https://docs.devexpress.com/eXpressAppFramework/113439/concepts/security-system/middle-tier-security-wcf-service#troubleshooting";
		}
		if(e.CompatibilityError == null) {
			message = "You probably tried to update the database in Middle Tier Security mode from the client side. " +
					  "In this mode, the server application updates the database automatically. " +
					  "To disable the automatic database update, set the XafApplication.DatabaseUpdateMode property to the DatabaseUpdateMode.Never value in the client application.";
		}
		throw new InvalidOperationException(message);
	}

    [Browsable(false)]
    public bool Importing{ get; set; }
}
[Obsolete]
public interface ILegacyImport{
	bool Importing{ get; set; }
}
