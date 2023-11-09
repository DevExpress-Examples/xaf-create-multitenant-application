using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Win;

namespace OutlookInspired.Win;
public class OutlookInspiredWindowsFormsApplication : WinApplication{
	public OutlookInspiredWindowsFormsApplication(){
		AboutInfo.Instance.Version = "Version " + AssemblyInfo.FileVersion;
		AboutInfo.Instance.Copyright = AssemblyInfo.AssemblyCopyright + " All Rights Reserved";
	}
}
