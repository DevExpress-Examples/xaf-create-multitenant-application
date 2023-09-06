using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Office;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Base;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Module.Controllers.Maps{
    public class MapsViewController:ObjectViewController<ObjectView,IMapsMarker>,IModelExtender{
        public const string Key = "AgPa0XVf4_HaN5BOPbTUw5KNvYEGOx-EftnjNRnCILfNgobxJC_deESiKqcfEgLd";
        public MapsViewController(){
            MapItAction = MapIt();
            TravelModeAction = TravelMode();
            ExportMapAction = Export();
            PrintPreviewMapAction = PrintPreview();
            PrintAction = Print();
            SalesPeriodAction=SalesPeriod();
            StageAction=Stage();
        }

        private SingleChoiceAction SalesPeriod() 
            => NewSingleChoiceAction("SalesPeriod","Period", Enum.GetValues<Period>().Where(period => period!=Period.FixedDate)
                    .Select(period => new ChoiceActionItem(period.ToString(), period){ImageName = period.ImageName()}).ToArray());
        private SingleChoiceAction Stage() 
            => NewSingleChoiceAction("Stage",Enum.GetValues<Stage>().Where(stage => stage!=BusinessObjects.Stage.Summary)
                    .Select(stage => new ChoiceActionItem(stage.ToString(), stage){ImageName = stage.ImageName()}).ToArray());

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
                { } t when t == typeof(Employee) => Employee.MapsDetailView,
                { } t when t == typeof(Customer) => Customer.MapsDetailView,
                { } t when t == typeof(Product) => Product.MapsDetailView,
                { } t when t == typeof(Order) => Order.MapsDetailView,
                { } t when t == typeof(Quote) => Quote.MapsDetailView,
                _ => throw new NotImplementedException(View.ObjectTypeInfo.Type.Name)
            };

        public SingleChoiceAction StageAction{ get; set; }
        public SingleChoiceAction SalesPeriodAction{ get; }
        public SimpleAction PrintAction{ get;  }
        public SimpleAction PrintPreviewMapAction{ get;  }
        public SingleChoiceAction TravelModeAction{ get; }
        public SimpleAction MapItAction{ get; }
        public SimpleAction ExportMapAction{ get; }

        protected override void OnActivated(){
            base.OnActivated();
            ChangeMapItAction(typeof(ISalesMapsMarker),"Sales Map");
            ChangeMapItAction(typeof(Order),"Shipping Map");
            ChangeMapItAction(typeof(Quote),"Opportunities Map");
            MapItAction.Active[nameof(MapsViewController)] = Frame is NestedFrame;
            TravelModeAction.Active[nameof(MapsViewController)] = typeof(ITravelModeMapsMarker).IsAssignableFrom(View.ObjectTypeInfo.Type);
            TravelModeAction.Active[nameof(MapItAction)] =!MapItAction.Active&& Frame.Context == TemplateContext.View&&!Frame.View.IsRoot;
            if (View.Id==Employee.MapsDetailView){
                Frame.GetController<RichTextShowInDocumentControllerBase>().ShowInDocumentAction
                    .Active[nameof(TravelModeAction)] = false;    
            }
            
            SalesPeriodAction.Active[nameof(MapsViewController)]= typeof(ISalesMapsMarker).IsAssignableFrom(View.ObjectTypeInfo.Type);
            SalesPeriodAction.Active[nameof(MapItAction)] =TravelModeAction.Active[nameof(MapItAction)];
            StageAction.Active[nameof(MapsViewController)]= typeof(Quote).IsAssignableFrom(View.ObjectTypeInfo.Type);
            StageAction.Active[nameof(MapItAction)] =TravelModeAction.Active[nameof(MapItAction)];
            ExportMapAction.Active[nameof(MapsViewController)] = TravelModeAction.Active[nameof(MapItAction)] ||
                                                                 SalesPeriodAction.Active[nameof(MapItAction)] ||
                                                                 StageAction.Active[nameof(MapItAction)];
            PrintAction.Active[nameof(MapsViewController)] =ExportMapAction.Active;
            PrintPreviewMapAction.Active[nameof(MapsViewController)] =ExportMapAction.Active;
            
        }

        private void ChangeMapItAction(Type objectType,string caption){
            if (objectType.IsAssignableFrom(View.ObjectTypeInfo.Type)){
                MapItAction.Caption = caption;
                MapItAction.ToolTip = MapItAction.Caption;
                MapItAction.Active.RemoveItem(ActionBase.RequireSingleObjectContext);
            }
        }

        public void ExtendModelInterfaces(ModelInterfaceExtenders extenders) 
            => extenders.Add<IModelOptions,IModelOptionsHomeOffice>();
    }

}