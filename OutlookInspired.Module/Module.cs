using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.ReportsV2;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Model;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Module;

// For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ModuleBase.
public sealed class OutlookInspiredModule : ModuleBase{
	public const string ModelCategory = "OutlookInspired";
    public OutlookInspiredModule() {
		// 
		// OutlookInspiredModule
		// 
		AdditionalExportedTypes.Add(typeof(ApplicationUser));
		AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.EF.PermissionPolicy.PermissionPolicyRole));
		AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.EF.ModelDifference));
		AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.EF.ModelDifferenceAspect));
		RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.SystemModule.SystemModule));
		RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Security.SecurityModule));
		RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule));
		RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Chart.ChartModule));
		RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule));
		RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Dashboards.DashboardsModule));
		RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Notifications.NotificationsModule));
		RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Office.OfficeModule));
		RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.PivotChart.PivotChartModuleBase));
		RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.PivotGrid.PivotGridModule));
		RequiredModuleTypes.Add(typeof(ReportsModuleV2));
		RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Scheduler.SchedulerModuleBase));
		RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.TreeListEditors.TreeListEditorsModuleBase));
		RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Validation.ValidationModule));
		RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ViewVariantsModule.ViewVariantsModule));
		DevExpress.ExpressApp.Security.SecurityModule.UsedExportedTypes = UsedExportedTypes.Custom;
		AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.EF.FileData));
		AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.EF.FileAttachment));
		AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.EF.Analysis));
		AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.EF.Event));
		AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.EF.Resource));
		AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.EF.HCategory));
    }
    public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
        yield return new DatabaseUpdate.Updater(objectSpace, versionFromDB);
    }
    public override void Setup(XafApplication application) {
	    base.Setup(application);
	    application.ObjectSpaceCreated += Application_ObjectSpaceCreated;
    }
    private void Application_ObjectSpaceCreated(object sender, ObjectSpaceCreatedEventArgs e) {
	    if (e.ObjectSpace is CompositeObjectSpace{ Owner: not CompositeObjectSpace } compositeObjectSpace) {
		    compositeObjectSpace.PopulateAdditionalObjectSpaces((XafApplication)sender);
	    }
	    if (e.ObjectSpace is NonPersistentObjectSpace objectSpace){
		    objectSpace.ObjectsGetting+= (o, args) => {
			    if (args.ObjectType == typeof(QuoteMapItem)){
				    args.Objects = new BindingList<QuoteMapItem>(((NonPersistentObjectSpace)o)!.GetObjectsQuery<Quote>().Opportunities().ToArray());
			    }
		    };
	    }

    }
    public override void AddGeneratorUpdaters(ModelNodesGeneratorUpdaters updaters) {
	    base.AddGeneratorUpdaters(updaters);
	    updaters.Add(new ModelViewClonerUpdater());
	    updaters.Add(new MasterDetailViewUpdater());
    }
    
    

}

