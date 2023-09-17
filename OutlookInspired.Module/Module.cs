using System.Runtime.CompilerServices;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.Persistent.BaseImpl.EF;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Controllers;
using OutlookInspired.Module.Features.CloneView;
using OutlookInspired.Module.Features.Customers;
using OutlookInspired.Module.Features.Employees;
using OutlookInspired.Module.Features.Maps;
using OutlookInspired.Module.Features.MasterDetail;
using OutlookInspired.Module.Features.Orders;
using OutlookInspired.Module.Features.ViewFilter;
using OutlookInspired.Module.Services;
using ReportController = OutlookInspired.Module.Features.Customers.ReportController;


[assembly:InternalsVisibleTo("OutlookInspired.Win")]
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
		AdditionalExportedTypes.Add(typeof(ModelDifference));
		AdditionalExportedTypes.Add(typeof(ModelDifferenceAspect));
		
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
		AdditionalExportedTypes.Add(typeof(FileData));
		AdditionalExportedTypes.Add(typeof(FileAttachment));
		AdditionalExportedTypes.Add(typeof(Analysis));
		AdditionalExportedTypes.Add(typeof(Event));
		AdditionalExportedTypes.Add(typeof(Resource));
		AdditionalExportedTypes.Add(typeof(HCategory));
		AdditionalExportedTypes.Add(typeof(RichTextMailMergeData));
    }
    
    public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
	    yield return new PredefinedReportsUpdater(Application, objectSpace, versionFromDB)
		    .AddOrderReports().AddCustomerReports().AddProductReports();
        yield return new DatabaseUpdate.Updater(objectSpace, versionFromDB);
    }

    protected override IEnumerable<Type> GetDeclaredControllerTypes() 
	    => new []{
		    typeof(MailMergeController),typeof(ReportController),
		    typeof(CommunicationController),typeof(RoutePointController),
		    typeof(MapsViewController),
		    typeof(FollowUpController),typeof(InvoiceReportDocumentController),typeof(InvoiceController),typeof(PayController),typeof(RefundController),typeof(Features.Orders.ReportController),typeof(ShipmentDetailController),
		    typeof(Features.Products.ReportController),
		    typeof(MasterDetailController),typeof(SplitterPositionController),typeof(ViewFilterController)
	    };

    public override void Setup(XafApplication application) {
	    base.Setup(application);
	    application.ObjectSpaceCreated += Application_ObjectSpaceCreated;
    }
    
    private void Application_ObjectSpaceCreated(object sender, ObjectSpaceCreatedEventArgs e) {
	    if (e.ObjectSpace is CompositeObjectSpace{ Owner: not CompositeObjectSpace } compositeObjectSpace) {
		    compositeObjectSpace.PopulateAdditionalObjectSpaces((XafApplication)sender);
	    }
    }
    
    public override void AddGeneratorUpdaters(ModelNodesGeneratorUpdaters updaters) {
	    base.AddGeneratorUpdaters(updaters);
	    updaters.Add(new CloneViewUpdater(), new MasterDetailUpdater(), new MapsUpdater(), new DataAccessModeUpdater());
    }
}

