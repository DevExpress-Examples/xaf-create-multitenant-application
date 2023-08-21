```cs
    private void GridView_InitNewRow(object sender, InitNewRowEventArgs e)
    {
      if (this.newItemRowHandlingMode == GridListEditorNewItemRowHandlingMode.NativeControl || this.GridView.IsServerMode)
        this.OnNewObjectCreated(((BaseView) sender).GetRow(e.RowHandle));
      else
        this.OnNewObjectCreated((object) null);
    }

```

Can someone explain why when I am in Server mode I get the **rational** behavior I expect from this event and context? That is to emit the new object.

If not a server mode I will get a null object since newItemRowHandlingMode default value is different. Please do not say use the static DefaultNewItemRowHandlingMode to change it your self, I can read the code thanks. Changing the static properties implies that I will write code in a Win dependent assembly and I have also need to support it test it etc, it clearly leads to spaghetti code. 

The event is NewObjectCreated and emits a null value which is crazy, in addition the default value of newItemRowHandlingMode implies that will emit from the NewObjectViewController, to me all this BS looks as bad as it can get. The default newItemRowHandlingMode shouldbe NativeControl and the ServerMode check makes no sense what if I have a non persistent object? 

Designing such views is a very common task. e.g. a popup ListView on a MasterDetail mode with a dialogcontroller. Then tell me please how to configure my new object after is created.

Here a real world example from the middle tier demo, I can write an agnostic controller to filter my views

```cs
        public ViewFilterController(){
            _filterAction = new SingleChoiceAction(this,"FilterView",PredefinedCategory.Filters){
                ImageName = "Action_Filter",PaintStyle = ActionItemPaintStyle.Image,
            };
            _filterAction.Executed += (_, e) => {
                if (!ManagerFilters(e)) FilterView();
                _filterAction.SelectedItem = _filterAction.Items.First(item => item.Data as string =="All");
            };
        }

        ...
        private void CreateViewFilterListView(ShowViewParameters showViewParameters){
            var listView = Application.CreateListView(typeof(ViewFilter), true);
            listView.Editor.NewObjectCreated += (_, args) => ((ViewFilter)((ObjectManipulatingEventArgs)args).Object).DataType = View.ObjectTypeInfo.Type;
            showViewParameters.TargetWindow=TargetWindow.NewModalWindow;
            showViewParameters.CreatedView=listView;
        }


```

but the NewObjectCreated will emit null so I have to write another controller for win and blazor

```cs
    public class NewItemRowHandlingModeController:ListViewControllerBase{
        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            if (View.Editor is GridListEditor listEditor&&View.ObjectTypeInfo.FindAttribute<NewItemRowHandlingModeAttribute>()!=null){
                listEditor.NewItemRowHandlingMode=GridListEditorNewItemRowHandlingMode.NativeControl;
            }
        }
    }

```
What a waste of time am I trying to prove that I can write code all over the place? 
--------------------------
The state of the the StateMahine module is unclear, the wizard  does not include it, there are docs without any note that is discontinued for Blazor/netcore so I guess it works. Should I use it for my tasks?


----------------------------
You wanna hear a funny fact?

![image](https://github.com/eXpandFramework/eXpand/assets/159464/deaa8881-634e-4e9f-a51e-f16669c21290)


It really looks bad, like u say if your customer encounters a cross thread exception change it to be non async? Really what u trying to accomplish? to make their heads explode? you better switch it off if there are issues, cause this is totally un-trustable 


------------------
DevExpress.ExpressApp.Security.SecurityModule.UsedExportedTypes = DevExpress.Persistent.Base.UsedExportedTypes.Custom;
    protected override IEnumerable<Type> GetDeclaredExportedTypes() {
        return new Type[] {
                typeof(Address),
                typeof(Country),
                typeof(DevExpress.Persistent.BaseImpl.EF.Event),
                typeof(DevExpress.Persistent.BaseImpl.EF.ReportDataV2),
                typeof(EventResource),
                typeof(DevExpress.Persistent.BaseImpl.EF.Analysis),
                typeof(Note),
                typeof(Employee),
                typeof(DemoTask),
                typeof(Department),
                typeof(Location),
                typeof(Paycheck),
                typeof(PhoneNumber),
                typeof(PortfolioFileData),
                typeof(Position),
                typeof(Resume)
            };
    }

is this required in my demo

-----------------
https://docs.devexpress.com/eXpressAppFramework/112813/event-planning-and-notifications/scheduler/resources-in-a-schedule

NOTE

In ASP.NET Core Blazor applications, you cannot assign more than one resource to an event. If you assign multiple resources, XAF displays a warning message and treats the event as if it does not have any resources assigned to it.

The built-in DevExpress.Persistent.BaseImpl.Event class, which implements the IEvent interface, has a Many-to-Many association with the built-in DevExpress.Persistent.BaseImpl.Resource class, which implements the IResource interface. All Resource objects from the Event’s Resources collection are listed in the Event’s IEvent.ResourceId property

-----------------
if Employee has aggragated Evalutions

// [Aggregated]
		public virtual ObservableCollection<Evaluation> Evaluations { get; set; }=new();

then the Employee property editor is not visible in the Evaluation_DetailView even if I add it manually in a view with FreezeLayout

--------
https://docs.devexpress.com/eXpressAppFramework/402990/business-model-design-orm/business-model-design-with-entity-framework-core/how-to-initialize-business-objects-with-default-property-values-in-entity-framework-core
Description is empty
		public override void OnCreated() {
			base.OnCreated();
			
			Description = @"
Raise: No
Bonus: No
";
		}
