using System.Collections;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.ExpressApp.Win.Editors.Grid.Internal;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Layout;
using DevExpress.XtraGrid.Views.Layout.ViewInfo;

namespace OutlookInspired.Win.Editors.GridListEditor{
    public class ColumnViewListEditorControlProvider :IControlOrderProvider{
        private readonly Dictionary<object, ObjectRecord> _objectRecords = new();
        private readonly ListEditor _listEditor;
        private readonly ColumnView _columnView;
        private readonly CollectionSourceBase _collectionSource;
        private readonly ColumnViewDataRowDoubleClickAdapter _gridViewDataRowDoubleClickAdapter;

        public ColumnViewListEditorControlProvider(ListEditor listEditor,ColumnView columnView,CollectionSourceBase collectionSource,Action onProcessSelectedItem){
            _listEditor = listEditor;
            _columnView = columnView;
            _columnView.GridControl.KeyDown+= (_, e) => {
                if (e.KeyCode!=Keys.Return)return;
                e.SuppressKeyPress = true;
                e.Handled = true;
                onProcessSelectedItem();
            };
            _collectionSource = collectionSource;
            _gridViewDataRowDoubleClickAdapter = new ColumnViewDataRowDoubleClickAdapter(columnView.GridControl, columnView);
            _gridViewDataRowDoubleClickAdapter.DataRowDoubleClick+=(_, _) => onProcessSelectedItem();
        }

        ObjectRecord CreateObjectRecord(object objectKey, int rowHandle){
            bool isDataViewMode = DataAccessModeHelper.IsViewMode(_collectionSource.DataAccessMode);
            var objectRecord =
                _collectionSource.DataAccessMode == CollectionSourceDataAccessMode.InstantFeedback
                    ? new XafInstantFeedbackRecord(_collectionSource.ObjectTypeInfo.Type, objectKey, rowHandle, isDataViewMode)
                    : new ObjectRecord(_collectionSource.ObjectTypeInfo.Type, objectKey, rowHandle, isDataViewMode);
            objectRecord.EvaluatorContextDescriptorGetting += (_, e)
                => e.EvaluatorContextDescriptor = new ObjectRecordContextDescriptor(_columnView, e.ObjectSpace,
                    _collectionSource.ObjectTypeInfo.Type, _collectionSource.DataAccessMode);
            if (objectKey != null && _objectRecords != null)
                _objectRecords[objectKey] = objectRecord;
            return objectRecord;
        }

        bool IsObjectRecordMode => _collectionSource != null && DataAccessModeHelper.IsObjectRecordMode(_collectionSource.DataAccessMode);
        bool AreEqual(object firstKey, object secondKey) 
            => firstKey.Equals(secondKey) || firstKey is List<object> first && secondKey is List<object> second && first.SequenceEqual(second);
        bool TryFindObjectRecord(object obj, out ObjectRecord resultValue){
            bool objectRecord = false;
            resultValue = null;
            foreach (object key in _objectRecords.Keys){
                if (AreEqual(key, obj)){
                    objectRecord = true;
                    resultValue = _objectRecords[key];
                    break;
                }
            }
            return objectRecord;
        }
        
        object GetObjectKey( int rowHandle){
            object objectKey;
            if (_collectionSource.ObjectTypeInfo.KeyMembers.Count > 1){
                objectKey = new List<object>();
                foreach (IMemberInfo keyMember in _collectionSource.ObjectTypeInfo.KeyMembers)
                    ((IList) objectKey).Add(_columnView.GetRowCellValue(rowHandle, keyMember.Name));
            }
            else
                objectKey = _columnView.GetRowCellValue(rowHandle, _collectionSource.ObjectTypeInfo.KeyMember.Name);
            return objectKey;
        }

        public object GetObjectByIndex(int index){
            if (_columnView is not{ DataController: not null }) return null;
            object objectByIndex = null;
            if (IsObjectRecordMode){
                var rowHandle = index;
                if (_columnView.IsDataRow(rowHandle) && _columnView.IsRowLoaded(rowHandle)){
                    object objectKey = GetObjectKey(rowHandle);
                    if (objectKey != null)
                        objectByIndex = GetObjectRecord(objectKey,  rowHandle);
                }
            }
            else
                objectByIndex = _columnView.GetRow(index);
            return objectByIndex;

        }
        
        ObjectRecord GetObjectRecord(object objectKey, int rowHandle){
            if (objectKey != null ){
                ObjectRecord resultValue;
                if (BaseObjectSpace.CompositeKeyPropertyType.IsAssignableFrom(objectKey.GetType()))                {
                    if (TryFindObjectRecord(objectKey, out resultValue))
                        return resultValue;
                }
                else if (_objectRecords.TryGetValue(objectKey, out resultValue))
                    return resultValue;
            }
            return CreateObjectRecord(objectKey, rowHandle);
        }


        public IList GetOrderedObjects(){
            var orderedObjects = new List<object>();
            if (_columnView == null) return orderedObjects;
            if (IsObjectRecordMode){
                for (var index = 0; index < _columnView.DataRowCount; ++index){
                    var rowHandle = index;
                    if (_columnView.IsDataRow(rowHandle) && _columnView.IsRowLoaded(rowHandle)){
                        var objectKey = GetObjectKey( rowHandle);
                        if (objectKey != null)
                            orderedObjects.Add(GetObjectRecord(objectKey, rowHandle));
                    }
                }
            }
            else if (_columnView.IsServerMode){
                var num1 = _columnView.GetVisibleIndex(_columnView.FocusedRowHandle) -
                           WinColumnsListEditor.PageRowCountForServerMode / 2;
                if (num1 < 0)
                    num1 = 0;
                var num2 = num1 + WinColumnsListEditor.PageRowCountForServerMode - 1;
                if (num2 > _columnView.RowCount - 1)
                    num2 = _columnView.RowCount - 1;
                for (var rowVisibleIndex = num1; rowVisibleIndex <= num2; ++rowVisibleIndex){
                    int visibleRowHandle = _columnView.GetVisibleRowHandle(rowVisibleIndex);
                    if (_columnView.IsDataRow(visibleRowHandle) && _columnView.IsRowLoaded(visibleRowHandle)){
                        object row = _columnView.GetRow(visibleRowHandle);
                        if (row != null)
                            orderedObjects.Add(row);
                    }
                }
            }
            else{
                for (var index = 0; index < _columnView.DataRowCount; ++index){
                    var rowHandle = index;
                    if (_columnView.IsRowLoaded(rowHandle)){
                        object row = _columnView.GetRow(rowHandle);
                        if (row != null)
                            orderedObjects.Add(row);
                    }
                }
            }

            return orderedObjects;

        }

        public int GetIndexByObject(object obj){
            var indexByObject = -1;
            if (_columnView?.DataSource != null){
                if (IsObjectRecordMode && obj is ObjectRecord objectRecord){
                    if (objectRecord.RowHandle.HasValue)
                        indexByObject = objectRecord.RowHandle.Value;
                }
                else{
                    indexByObject = _columnView.GetRowHandle(_listEditor.List.IndexOf(obj));
                    if (indexByObject == int.MinValue)
                        indexByObject = -1;
                }
            }
            return indexByObject;

        }
    }

    public class ColumnViewDataRowDoubleClickAdapter : IGridViewDataRowDoubleClickAdapter{
        public readonly int DoubleClickTime = SystemInformation.DoubleClickTime;
        private int _mouseUpTime;
        private int _mouseDownTime;
        private bool _isDoubleClicking;
        private ColumnView _columnView;
        private GridControl _grid;

        private bool NeedProcessDoubleClick(RepositoryItemPopupBase repositoryItemPopupBase,
            ShowButtonModeEnum showButtonMode)
            => (!_columnView.ActiveEditor.IsModified ||
                _columnView.OptionsBehavior.EditorShowMode != EditorShowMode.MouseDown ||
                showButtonMode != ShowButtonModeEnum.ShowAlways) &&
               (repositoryItemPopupBase == null || repositoryItemPopupBase.ReadOnly ||
                repositoryItemPopupBase.ShowDropDown != ShowDropDown.DoubleClick || _columnView.FocusedColumn == null ||
                !_columnView.FocusedColumn.OptionsColumn.AllowEdit);

        private void grid_DoubleClick(object sender, EventArgs e){
            _isDoubleClicking = false;
            if (DataRowDoubleClick == null || _columnView.FocusedRowHandle < 0) return;
            var mouseArgs = DXMouseEventArgs.GetMouseArgs(_grid, e);
            if (mouseArgs.Button != MouseButtons.Left || !OnObject(mouseArgs.Location)) return;
            mouseArgs.Handled = true;
            DataRowDoubleClick(this, e);
        }

        private bool OnObject(Point mouseLocation) 
            => _columnView is GridView gridView ? gridView.CalcHitInfo(mouseLocation).InRow
                : ((LayoutView)_columnView).CalcHitInfo(mouseLocation).InCard;

        private void columnView_MouseDown(object sender, MouseEventArgs e){
            _isDoubleClicking = false;
            var columnView = (ColumnView)sender;
            var hitInfo = columnView.CalcHitInfo(new Point(e.X, e.Y));
            bool isClickOnObject = hitInfo switch{
                LayoutViewHitInfo layoutHitInfo => layoutHitInfo.InCard,
                GridHitInfo layoutHitInfo => layoutHitInfo.InRow,
                _ => false
            };
            _mouseDownTime = isClickOnObject ? Environment.TickCount : 0;
        }

        private void columnView_MouseUp(object sender, MouseEventArgs e){
            _mouseUpTime = Environment.TickCount;
            if (!_isDoubleClicking)
                return;
            if (e.Button == MouseButtons.Left && OnObject(e.Location) && e is DXMouseEventArgs args)
                args.Handled = true;
            _isDoubleClicking = false;
        }

        private void Editor_MouseDown(object sender, MouseEventArgs e){
            if (e.Button != MouseButtons.Left) return;
            var tickCount = Environment.TickCount;
            if (_mouseDownTime > _mouseUpTime || _mouseUpTime > tickCount ||
                tickCount - _mouseDownTime >= DoubleClickTime)
                return;
            _isDoubleClicking = true;
            RepositoryItemPopupBase repositoryItemPopupBase = null;
            var showButtonMode = _columnView.OptionsView.ShowButtonMode;
            if (_columnView.FocusedColumn != null){
                repositoryItemPopupBase = _columnView.FocusedColumn.ColumnEdit as RepositoryItemPopupBase;
                if (_columnView.FocusedColumn.ShowButtonMode != ShowButtonModeEnum.Default)
                    showButtonMode = _columnView.FocusedColumn.ShowButtonMode;
            }

            if (NeedProcessDoubleClick(repositoryItemPopupBase, showButtonMode) && DataRowDoubleClick != null){
                if (_columnView.ActiveEditor != null){
                    _columnView.ActiveEditor.DoValidate();
                    _columnView.PostEditor();
                    _columnView.CloseEditor();
                }

                if (e is DXMouseEventArgs args)
                    args.Handled = true;
                DataRowDoubleClick(this, e);
            }

            _mouseDownTime = 0;
        }

        private void Editor_MouseUp(object sender, MouseEventArgs e) => _mouseUpTime = Environment.TickCount;

        private void GridView_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e){
            var gridView = (GridView)sender;
            if (e.RepositoryItem == null || gridView.FocusedColumn == null ||
                gridView.FocusedRowHandle == -2147483646  || gridView.FocusedRowHandle == -2147483647 )
                return;
            e.RepositoryItem.MouseDown += Editor_MouseDown;
            e.RepositoryItem.MouseUp += Editor_MouseUp;
        }

        public ColumnViewDataRowDoubleClickAdapter(GridControl grid, ColumnView columnView){
            _columnView = columnView;
            _grid = grid;
            grid.DoubleClick += grid_DoubleClick;
            columnView.MouseUp += columnView_MouseUp;
            columnView.MouseDown += columnView_MouseDown;
            if (columnView is GridView gridView)
                gridView.CustomRowCellEditForEditing += GridView_CustomRowCellEditForEditing;
        }

        public void Dispose(){
            DataRowDoubleClick = null;
            if (_grid != null){
                _grid.DoubleClick -= grid_DoubleClick;
                _grid = null;
            }

            if (_columnView == null) return;
            _columnView.MouseUp -= columnView_MouseUp;
            _columnView.MouseDown -= columnView_MouseDown;
            if (_columnView is GridView gridView)
                gridView.CustomRowCellEditForEditing -= GridView_CustomRowCellEditForEditing;
            _columnView = null;
        }

        public event EventHandler<EventArgs> DataRowDoubleClick;
    }

}