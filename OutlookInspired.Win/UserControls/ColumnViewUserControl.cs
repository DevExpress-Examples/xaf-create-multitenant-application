﻿using System.Collections;
using System.Reflection;
using DevExpress.ExpressApp;
using DevExpress.XtraGrid.Views.Base;
using OutlookInspired.Module.Controllers;
using OutlookInspired.Win.Extensions;

namespace OutlookInspired.Win.UserControls
{
    public partial class ColumnViewUserControl : UserControl, IUserControl
    {
        private IObjectSpace _objectSpace;
        protected ColumnView ColumnView;
        protected IList DataSource;
        private readonly bool _isChild;
        public event EventHandler DataSourceOrFilterChanged;
        public event EventHandler CurrentObjectChanged;
        public event EventHandler SelectionChanged;
        public event EventHandler SelectionTypeChanged;
        public event EventHandler ProcessObject;

        public ColumnViewUserControl() => _isChild = GetType().GetCustomAttribute(typeof(DetailUserControlAttribute)) != null;
        public virtual void Refresh(object currentObject) => Refresh();

        public void Setup(IObjectSpace objectSpace, XafApplication application)
        {
            _objectSpace = objectSpace;
            ColumnView = this.ColumnView();
            ColumnView.SelectionChanged += (_, _) =>
            {
                SelectionChanged?.Invoke(this, EventArgs.Empty);
                CurrentObjectChanged?.Invoke(this, EventArgs.Empty);
            };
            ColumnView.DoubleClick += (_, _) => ProcessObject?.Invoke(this, EventArgs.Empty);
            ColumnView.ColumnFilterChanged += (_, _) => OnDataSourceOfFilterChanged();
            ColumnView.DataSourceChanged += (_, _) => OnDataSourceOfFilterChanged();
            Refresh();
        }

        public override void Refresh() => ColumnView.GridControl.DataSource = _isChild ? DataSource : _objectSpace.GetObjects(GetObjectType());
        protected virtual Type GetObjectType() => throw new NotImplementedException();
        public object CurrentObject => ColumnView.FocusedRowObject;
        public IList SelectedObjects => ColumnView.GetSelectedRows().Select(i => ColumnView.GetRow(i)).ToArray();
        public SelectionType SelectionType => SelectionType.Full;
        public bool IsRoot => false;

        protected virtual void OnDataSourceOfFilterChanged() => DataSourceOrFilterChanged?.Invoke(this, EventArgs.Empty);
        protected virtual void OnSelectionTypeChanged() => SelectionTypeChanged?.Invoke(this, EventArgs.Empty);
    }
}
