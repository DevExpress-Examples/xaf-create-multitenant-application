using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
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
                ImageName = "Action_Filter",PaintStyle = ActionItemPaintStyle.Image
            };
            FilterAction.Executed += (_, e) => {
                if (!ManagerFilters(e)) FilterView();
            };
        }
        
        public SingleChoiceAction FilterAction{ get; }
        
        private void FilterView(){
            var criteria = FilterAction.SelectedItem.Data is ViewFilter viewFilter ? viewFilter.Criteria : null;
            var userControl = View.UserControl();
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
            FilterAction.Active[nameof(ViewFilterController)] = Frame is NestedFrame;
            AddFilterItems();
            if (View is ListView listView){
                // listView.CollectionSource.CriteriaApplied+=CollectionSourceOnCriteriaApplied;
            }
        }

        protected override void OnDeactivated(){
            base.OnDeactivated();
            if (View is ListView listView){
                // listView.CollectionSource.CriteriaApplied-=CollectionSourceOnCriteriaApplied;
            }
        }

        private void CollectionSourceOnCriteriaApplied(object sender, EventArgs e) => AddFilterItems();

        private void AddFilterItems(){
            FilterAction.Items.Clear();
            var viewCriteria =View is ListView listView? listView.CollectionSource.GetTotalCriteria():null;
            FilterAction.Items.AddRange(new[]{ (caption:"Manage...",data:"Manage"),
                    (caption:$"All ({ObjectSpace.GetObjectsCount(View.ObjectTypeInfo.Type, viewCriteria)})",data:"All") }
                .Select(t => new ChoiceActionItem(t.caption, t.data)).Concat(ObjectSpace.GetObjectsQuery<ViewFilter>()
                    .Where(filter => filter.DataTypeName == View.ObjectTypeInfo.Type.FullName).ToArray()
                    .Select(filter => new ChoiceActionItem($"{filter.Name} ({filter.Count(viewCriteria)})",filter))).ToArray());
            FilterAction.SelectedItem = FilterAction.Items.First(item => item.Data as string == "All");
        }
    }
}