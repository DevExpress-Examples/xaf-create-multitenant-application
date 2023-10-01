using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;

namespace OutlookInspired.Blazor.Server;

public class OutlookInspiredBlazorApplication : BlazorApplication {
    public OutlookInspiredBlazorApplication(){
        AboutInfo.Instance.Version = "Version " + AssemblyInfo.FileVersion;
        AboutInfo.Instance.Copyright = AssemblyInfo.AssemblyCopyright + " All Rights Reserved";
    }
    protected override SettingsStorage CreateLogonParameterStoreCore() {
        return new EmptySettingsStorage();
    }

    protected override void OnSetupStarted() {
        base.OnSetupStarted();
#if DEBUG || EASYTEST
        if(System.Diagnostics.Debugger.IsAttached && CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema) {
            DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
        }
#endif
    }

    class EmptySettingsStorage : SettingsStorage {
        public override string LoadOption(string optionPath, string optionName) => null;
        public override void SaveOption(string optionPath, string optionName, string optionValue) { }
    }

}
