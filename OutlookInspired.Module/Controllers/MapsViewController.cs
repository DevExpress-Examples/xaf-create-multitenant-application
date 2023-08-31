using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Base;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Module.Controllers{
    public interface IModelOptionsHomeOffice{
        IModelHomeOffice HomeOffice{ get; }    
    }

    public interface IModelHomeOffice:IMapsMarker,IModelNode{
        [DefaultValue("Glendale")]
        string City{ get; set; }
        [DefaultValue("505 N. Brand Blvd")]
        string Line{ get; set; }
        [DefaultValue(StateEnum.CA)]
        StateEnum State{ get; set; }
        [DefaultValue(34.1532866)]
        new double Latitude{ get; set; }
        [DefaultValue(-118.2555815)]
        new double Longitude{ get; set; }
    }
    public class MapsViewController:ObjectViewController<ObjectView,IMapsMarker>,IModelExtender{
        public const string Key = "AgPa0XVf4_HaN5BOPbTUw5KNvYEGOx-EftnjNRnCILfNgobxJC_deESiKqcfEgLd";
        public MapsViewController(){
            MapItAction = MapIt();
            TravelModeAction = TravelMode();
            ExportMapAction = Export();
            PrintPreviewMapAction = PrintPreview();
            PrintAction = Print();
            SalesPeriodAction=SalesPeriod();
        }

        private SingleChoiceAction SalesPeriod() 
            => NewSingleChoiceAction("SalesPeriod","Period", Enum.GetValues<Period>().Where(period => period!=Period.FixedDate)
                    .Select(period => new ChoiceActionItem(period.ToString(), period){ImageName = period.ImageName()}).ToArray());

        private SimpleAction Print() 
            => new(this,"MapPrint",PredefinedCategory.View){ImageName = "Print"};

        private SimpleAction PrintPreview() 
            => new(this,"MapPrintPreview",PredefinedCategory.View){ImageName = "PrintPreview"};

        private SimpleAction Export() 
            => new(this,"MapExport",PredefinedCategory.View){ImageName = "Export"};

        private SingleChoiceAction TravelMode() 
            => NewSingleChoiceAction("TravelMode", new ChoiceActionItem("Driving", "Driving"){ ImageName = "Driving" },
                new ChoiceActionItem("Walking", "Walking"){ ImageName = "Walking" });

        private SimpleAction MapIt(){
            var action = new SimpleAction(this, "MapIt", PredefinedCategory.View){
                SelectionDependencyType = SelectionDependencyType.RequireSingleObject, ImageName = "MapIt",
                PaintStyle = ActionItemPaintStyle.Image
            };
            action.Executed+=(_, e) => e.NewDetailView(GetViewId(),TargetWindow.NewModalWindow);
            return action;
        }

        private SingleChoiceAction NewSingleChoiceAction(string actionId,string caption, params ChoiceActionItem[] items){
            var action = new SingleChoiceAction(this, actionId, PredefinedCategory.View){
                ItemType = SingleChoiceActionItemType.ItemIsOperation, PaintStyle = ActionItemPaintStyle.Image,
                ImageMode = ImageMode.UseItemImage,
                DefaultItemMode = DefaultItemMode.LastExecutedItem, TargetViewType = ViewType.DetailView,
                Active ={ [nameof(MapsViewController)] = false },
            };
            action.Items.AddRange(items);
            action.SelectedIndex = 0;
            if (caption != null){
                action.Caption = caption;
            }
            return action;
        }

        private SingleChoiceAction NewSingleChoiceAction(string actionId,params ChoiceActionItem[] items) => NewSingleChoiceAction(actionId, null, items);

        private string GetViewId() 
            => View.ObjectTypeInfo.Type switch{
                { } t when t == typeof(Employee) => Employee.EmployeeDetailViewMaps,
                { } t when t == typeof(Customer) => Customer.CustomerDetailViewMaps,
                { } t when t == typeof(Product) => Product.ProductDetailViewMaps,
                { } t when t == typeof(Order) => Order.OrderDetailViewMaps,
                _ => throw new NotImplementedException(View.ObjectTypeInfo.Type.Name)
            };

        public SingleChoiceAction SalesPeriodAction{ get; }
        public SimpleAction PrintAction{ get;  }
        public SimpleAction PrintPreviewMapAction{ get;  }
        public SingleChoiceAction TravelModeAction{ get; }
        public SimpleAction MapItAction{ get; }
        public SimpleAction ExportMapAction{ get; }

        protected override void OnActivated(){
            base.OnActivated();
            MapItAction.Active[nameof(MapsViewController)] = Frame.ParentIsNull();
            TravelModeAction.Active[nameof(MapsViewController)] = typeof(ITravelModeMapsMarker).IsAssignableFrom(View.ObjectTypeInfo.Type);
            TravelModeAction.Active[nameof(MapItAction)] =!MapItAction.Active&& Frame.Context == TemplateContext.View&&!Frame.View.IsRoot;
            SalesPeriodAction.Active[nameof(MapsViewController)]= typeof(ISalesMapsMarker).IsAssignableFrom(View.ObjectTypeInfo.Type);
            SalesPeriodAction.Active[nameof(MapItAction)] =TravelModeAction.Active[nameof(MapItAction)];
            ExportMapAction.Active[nameof(MapsViewController)] =TravelModeAction.Active||SalesPeriodAction.Active;
            PrintAction.Active[nameof(MapsViewController)] =TravelModeAction.Active||SalesPeriodAction.Active;
            PrintPreviewMapAction.Active[nameof(MapsViewController)] =TravelModeAction.Active||SalesPeriodAction.Active;
            if (typeof(ISalesMapsMarker).IsAssignableFrom(View.ObjectTypeInfo.Type)){
                MapItAction.Caption = "Sales Map";
                MapItAction.ToolTip = MapItAction.Caption;
            }
            if (typeof(Order).IsAssignableFrom(View.ObjectTypeInfo.Type)){
                MapItAction.Caption = "Shipping Map";
                MapItAction.ToolTip = MapItAction.Caption;
            }
        }

        public void ExtendModelInterfaces(ModelInterfaceExtenders extenders) 
            => extenders.Add<IModelOptions,IModelOptionsHomeOffice>();
    }
}