using System.Collections;
using DevExpress.Data;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Win.Editors.Grid.Internal;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Layout;

namespace OutlookInspired.Win.Editors.GridListEditor{
    [ListEditor(typeof(object),false)]
    public class ColumnViewListEditor(IModelListView model) : ListEditor(model), IComplexListEditor,IControlOrderProvider{
        private ColumnViewListEditorControlProvider _controlProvider;
        private CollectionSourceBase _collectionSource;
        private XafApplication _application;
        
        public event EventHandler<ColumnViewControlCreatingArgs> ColumnViewControlCreating;
        protected override object CreateControlsCore(){
            var e = new ColumnViewControlCreatingArgs();
            OnColumnViewControlCreating(e);
            ProtectDetailViews(e.Control.ColumnView);
            _controlProvider = new(this,e.Control.ColumnView,_collectionSource,OnProcessSelectedItem);
            return e.Control;
        }

        public new IColumnViewUserControl Control => (IColumnViewUserControl)base.Control;

        protected override void AssignDataSourceToControl(object dataSource){
            if (Control == null) return;
            var columnView = Control.ColumnView;
            columnView.SelectionChanged += ColumnViewOnSelectionChanged;
            columnView.DataSourceChanged+=ColumnViewOnDataSourceChanged;
            columnView.GridControl.DataSource = dataSource;
            if (columnView is GridView gridView){
                gridView.MasterRowGetRelationName+=GridViewOnMasterRowGetRelationName;
            }
            
        }

        private void GridViewOnMasterRowGetRelationName(object sender, MasterRowGetRelationNameEventArgs e){
            
        }

        public override void BreakLinksToControls(){
            base.BreakLinksToControls();
            if (Control == null) return;
            Control.ColumnView.SelectionChanged -= ColumnViewOnSelectionChanged;
            Control.ColumnView.DataSourceChanged-=ColumnViewOnDataSourceChanged;
        }

        void ProtectDetailViews(ColumnView columnView){
            var gridLevelNodes = columnView.GridControl.LevelTree.Nodes.ToArray()
                .Where(node => {
                    var listElementType = _application.TypesInfo.FindTypeInfo(_collectionSource.ObjectTypeInfo.Type)
                        .FindMember(node.RelationName).ListElementType;
                    return !((IRequestSecurity)_application.Security).IsGranted(new PermissionRequest(listElementType,
                        SecurityOperations.Read));
                });
            foreach (var gridLevelNode in gridLevelNodes){
                Control.ColumnView.GridControl.LevelTree.Nodes.Remove(gridLevelNode);
            }
        }
        
        private void ColumnViewOnSelectionChanged(object sender, SelectionChangedEventArgs e){
            OnFocusedObjectChanging();
            FocusedObject = GetSelectedObjects().Cast<object>().FirstOrDefault();
            OnFocusedObjectChanged();
            OnSelectionChanged();
        }

        private void ColumnViewOnDataSourceChanged(object sender, EventArgs e) => OnDataSourceChanged();
        
        
        public override void Refresh() => _collectionSource.ResetCollection();

        public override IList GetSelectedObjects(){
            if (Control == null) return new List<object>();
            var rows = Control.ColumnView.GetSelectedRows();
            var selectedObjects = rows.Any() ? rows.Select(i => Control.ColumnView.GetRow(i)).ToArray()
                : new[]{Control.ColumnView.FocusedRowHandle}
                    .Select(i => Control.ColumnView.GetRow(i));
            return selectedObjects.ToArray();
        }

        public override SelectionType SelectionType=>SelectionType.Full;
        
        public void Setup(CollectionSourceBase collectionSource, XafApplication application){
            _collectionSource = collectionSource;
            _application = application;
        }

        protected virtual void OnColumnViewControlCreating(ColumnViewControlCreatingArgs e) => ColumnViewControlCreating?.Invoke(this, e);


        public object GetObjectByIndex(int index) => _controlProvider.GetObjectByIndex(index);

        public int GetIndexByObject(object obj) => _controlProvider.GetIndexByObject(obj);


        public IList GetOrderedObjects() => _controlProvider.GetOrderedObjects();


    }

    public class ColumnViewControlCreatingArgs{
        public IColumnViewUserControl Control{ get; set; }
    }
    
    public interface IColumnViewUserControl{
        ColumnView ColumnView{ get; }
    }

}