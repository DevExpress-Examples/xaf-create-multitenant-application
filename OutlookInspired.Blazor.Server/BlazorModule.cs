using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using OutlookInspired.Blazor.Server.Controllers;
using OutlookInspired.Blazor.Server.Features.Customers;
using OutlookInspired.Blazor.Server.Features.Employees;
using OutlookInspired.Blazor.Server.Features.Employees.CardView;
using OutlookInspired.Blazor.Server.Features.Evaluations;
using OutlookInspired.Blazor.Server.Features.Maps;
using OutlookInspired.Blazor.Server.Features.Quotes;
using OutlookInspired.Blazor.Server.Features.ViewFilter;
using OutlookInspired.Module.BusinessObjects;
using CellDisplayTemplateController = OutlookInspired.Blazor.Server.Features.Employees.Evaluations.CellDisplayTemplateController;

namespace OutlookInspired.Blazor.Server;
public class MyClass:ObjectViewController<DetailView,Employee>{
    public MyClass(){
        var simpleAction = new SimpleAction(this,"Select",PredefinedCategory.View);
        simpleAction.Executed+=SimpleActionOnExecuted;
    }

    private void SimpleActionOnExecuted(object sender, ActionBaseEventArgs e){
        var model = ((Model)View.GetItems<ControlViewItem>().First().Control);
        model.SelectObject(model.Objects.First());
    }

    protected override void OnActivated(){
        base.OnActivated();
        // View.CustomizeViewItemControl<ControlViewItem>(this,controlViewItem => {
        //     var model = ((Model)controlViewItem.Control);
        //     model.SelectObject(model.Objects.First());
        // });
    }

    private void EnabledOnResultValueChanged(object sender, BoolValueChangedEventArgs e){
        
    }
}
[ToolboxItemFilter("Xaf.Platform.Blazor")]
public sealed class OutlookInspiredBlazorModule : ModuleBase {
    private void Application_CreateCustomUserModelDifferenceStore(object sender, CreateCustomModelDifferenceStoreEventArgs e) {
        e.Store = new ModelDifferenceDbStore((XafApplication)sender, typeof(ModelDifference), false, "Blazor");
        e.Handled = true;
    }

    protected override IEnumerable<Type> GetDeclaredControllerTypes() 
        => new[]{
            typeof(CellDisplayTemplateController), typeof(SchedulerGroupTypeController), typeof(EnableDashboardMasterItemNewActionController),
            typeof(DxGridListEditorController),typeof(DetailRowController),typeof(RichTextPropertyEditorController),
            typeof(Features.Employees.Tasks.CellDisplayTemplateController),typeof(Features.Orders.DetailRowController),
            typeof(RouteMapsViewController),typeof(RouteMapsViewController),typeof(MapsViewController),typeof(SalesMapsViewController),
            typeof(Features.Products.SalesMapsViewController),typeof(Features.Orders.RouteMapsViewController),typeof(MyClass),
            typeof(BlazorMapsViewController),typeof(PaletteController),typeof(PopupWindowSizeController),typeof(ViewFilterController)
        };

    public override void Setup(XafApplication application) {
        base.Setup(application);
        application.CreateCustomUserModelDifferenceStore += Application_CreateCustomUserModelDifferenceStore;
    }

}
