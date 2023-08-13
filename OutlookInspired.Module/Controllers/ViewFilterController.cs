using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Base;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Module.Controllers{

    public class ViewFilterController:ObjectViewController<ObjectView,IViewFilter>{
        public const string FilterViewActionId = "FilterView";
        public ViewFilterController(){
            FilterAction = new SingleChoiceAction(this,FilterViewActionId,PredefinedCategory.Filters){
                ImageName = "Action_Filter",PaintStyle = ActionItemPaintStyle.Image,
            };
            FilterAction.Executed += (_, e) => {
                if (!ManagerFilters(e)) FilterView();
            };
        }

        public SingleChoiceAction FilterAction{ get; }

        private void FilterView(){
            var criteria = FilterAction.SelectedItem.Data is ViewFilter viewFilter ? viewFilter.Criteria : null;
            var userControl = View.GetItems<ControlViewItem>().Select(item => item.Control).OfType<IUserControl>().FirstOrDefault();
            if (userControl != null){
                userControl.SetCriteria(criteria);
            }
            else{
                View.ToListView().CollectionSource.Criteria[nameof(ViewFilterController)] = CriteriaOperator.Parse(criteria);
            }
        }

        private bool ManagerFilters(ActionBaseEventArgs e){
            if (FilterAction.SelectedItem.Data as string != "Manage") return false;
            CreateViewFilterListView(e.ShowViewParameters);
            AddDialogController(e.ShowViewParameters);
            return true;

        }

        private void AddDialogController(ShowViewParameters showViewParameters){
            var controller = Application.CreateController<DialogController>();
            showViewParameters.Controllers.Add(controller);
            controller.AcceptAction.Executed += (_, _) => {
                AddFilterItems();
                FilterAction.DoExecute(FilterAction.SelectedItem);
            };
            controller.CancelAction.Executed+= (_, _) => {
                AddFilterItems();
                FilterAction.DoExecute(FilterAction.SelectedItem);
            }; 
        }

        private void CreateViewFilterListView(ShowViewParameters showViewParameters){
            var listView = Application.CreateListView(typeof(ViewFilter), true);
            listView.Editor.NewObjectCreated += (_, args) => ((ViewFilter)((ObjectManipulatingEventArgs)args).Object).DataType = View.ObjectTypeInfo.Type;
            listView.CollectionSource.SetCriteria<ViewFilter>(filter => filter.DataTypeName == View.ObjectTypeInfo.Type.FullName);
            showViewParameters.TargetWindow=TargetWindow.NewModalWindow;
            showViewParameters.CreatedView=listView;
        }

        protected override void OnActivated(){
            base.OnActivated();
            FilterAction.Active[nameof(ViewFilterController)] = View is ListView||Frame is NestedFrame;
            AddFilterItems();
        }
        
        private void AddFilterItems(){
            FilterAction.Items.Clear();
            FilterAction.Items.AddRange(new[]{ (caption:"Manage...",data:"Manage"),
                    (caption:$"All ({ObjectSpace.GetObjectsCount(View.ObjectTypeInfo.Type, null)})",data:"All") }
                .Select(t => new ChoiceActionItem(t.caption, t.data)).Concat(ObjectSpace.GetObjectsQuery<ViewFilter>()
                    .Where(filter => filter.DataTypeName == View.ObjectTypeInfo.Type.FullName).ToArray()
                    .Select(filter => new ChoiceActionItem($"{filter.Name} ({filter.Count})",filter))).ToArray());
            FilterAction.SelectedItem = FilterAction.Items.First(item => item.Data as string == "All");
        }
    }
}