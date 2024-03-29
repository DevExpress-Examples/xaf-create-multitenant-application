﻿using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Base;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Module.Features.ViewFilter{
    public class ViewFilterController:ObjectViewController<ObjectView,IViewFilter>{
        public const string FilterViewActionId = "FilterView";
        public ViewFilterController(){
            FilterAction = new SingleChoiceAction(this,FilterViewActionId,PredefinedCategory.Filters){
                ImageName = "Action_Filter",PaintStyle = ActionItemPaintStyle.Image
            };
            FilterAction.Executed += (_, e) => {
                if (ManagerFilters(e)) return;
                FilterView();
            };
        }
        
        public SingleChoiceAction FilterAction{ get; }
        
        private void FilterView(){
            var criteria = FilterAction.SelectedItem.Data is BusinessObjects.ViewFilter viewFilter ? viewFilter.Criteria : null;
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
            controller.Activated+=ListViewDialogControllerOnActivated;
            showViewParameters.Controllers.Add(controller);
            controller.AcceptAction.Executed += (_, _) => {
                AddFilterItems();
                FilterAction.DoExecute();
            };
            controller.CancelAction.Executed+= (_, _) => {
                AddFilterItems();
                FilterAction.DoExecute();
            }; 
        }

        private void ListViewDialogControllerOnActivated(object sender, EventArgs e){
            var dialogController = ((DialogController)sender);
            dialogController.Activated-=ListViewDialogControllerOnActivated;
            dialogController.Frame.GetController<NewObjectViewController>().ObjectCreated+=OnObjectCreated;
        }

        private void OnObjectCreated(object sender, ObjectCreatedEventArgs e){
            ((NewObjectViewController)sender).ObjectCreated-=OnObjectCreated;
            ((BusinessObjects.ViewFilter)e.CreatedObject).DataType = View.ObjectTypeInfo.Type;
        }
        
        private void CreateViewFilterListView(ShowViewParameters showViewParameters){
            var listView = Application.CreateListView(typeof(BusinessObjects.ViewFilter), true);
            listView.Editor.NewObjectCreated += (_, args) => ((BusinessObjects.ViewFilter)((ObjectManipulatingEventArgs)args).Object).DataType = View.ObjectTypeInfo.Type;
            listView.CollectionSource.SetCriteria<BusinessObjects.ViewFilter>(filter => filter.DataTypeName == View.ObjectTypeInfo.Type.FullName);
            showViewParameters.TargetWindow=TargetWindow.NewModalWindow;
            showViewParameters.CreatedView=listView;
        }
        
        protected override void OnDeactivated(){
            base.OnDeactivated();
            Application.ObjectSpaceCreated-=ApplicationOnObjectSpaceCreated;
        }
        
        protected override void OnActivated(){
            base.OnActivated();
            if (!(FilterAction.Active[nameof(ViewFilterController)] = Frame is NestedFrame&&Frame.View.IsRoot))return;
            AddFilterItems();
            View.CustomizeViewItemControlCore<ControlViewItem>(this, _ => FilterAction.DoExecute(item => $"{item.Data}" == "This Month"),_ => View.ObjectTypeInfo.Type==typeof(Quote));
            Application.ObjectSpaceCreated+=ApplicationOnObjectSpaceCreated;
        }

        private void ApplicationOnObjectSpaceCreated(object sender, ObjectSpaceCreatedEventArgs e){
            e.ObjectSpace.Committing+=ObjectSpaceOnCommitting;
            e.ObjectSpace.Disposed+=ObjectSpaceOnDisposed;
        }

        private void ObjectSpaceOnDisposed(object sender, EventArgs e) 
            => ((IObjectSpace)sender).Committing-=ObjectSpaceOnCommitting;

        private void ObjectSpaceOnCommitting(object sender, CancelEventArgs e){
            var objectSpace = ((IObjectSpace)sender);
            if (!objectSpace.ModifiedObjects.Cast<object>().OfType<IViewFilter>().Any()) return;
            objectSpace.Committed+=OnCommitted;
        }

        private void OnCommitted(object sender, EventArgs e){
            ((IObjectSpace)sender).Committed-=ObjectSpaceOnCommitted;
            AddFilterItems();
        }

        private void ObjectSpaceOnCommitted(object sender, EventArgs e) => AddFilterItems();
        
        void AddFilterItems(){
            if (View==null)return;
            FilterAction.Items.Clear();
            var viewCriteria =View is ListView listView? listView.CollectionSource.GetTotalCriteria():null;
            FilterAction.Items.AddRange(new[]{ (caption:"Manage...",data:"Manage"),
                    (caption:$"All ({ObjectSpace.GetObjectsCount(View.ObjectTypeInfo.Type, viewCriteria)})",data:"All") }
                .Select(t => new ChoiceActionItem(t.caption, t.data)).Concat(ObjectSpace.GetObjectsQuery<BusinessObjects.ViewFilter>()
                    .Where(filter => filter.DataTypeName == View.ObjectTypeInfo.Type.FullName).ToArray()
                    .Select(filter => new ChoiceActionItem($"{filter.Name} ({filter.Count(viewCriteria)})",filter))).ToArray());
            FilterAction.SelectedItem = FilterAction.Items.First(item => $"{item.Data}"=="All");
        }
    }
}