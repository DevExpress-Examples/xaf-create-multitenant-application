using System.Collections;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using OutlookInspired.Module.Controllers;

namespace OutlookInspired.Win.UserControls {
    public  partial class BaseUserControl : UserControl , IComplexControl, IUserControl{
        private IObjectSpace _objectSpace;
        public event EventHandler<ObjectArgs> SelectedObjectChanged;
        public event EventHandler<ObjectArgs> ProcessObject;

        protected BaseUserControl() {
            InitializeComponent();
            Load += (_, _) => {
                var columnView = GetColumnView();
                if (columnView == null) return;
                columnView.FocusedRowObjectChanged += (_, e) => SelectedObjectChanged?.Invoke(this, new ObjectArgs(e.Row));
                columnView.DoubleClick += (_, _) => ProcessObject?.Invoke(this, new ObjectArgs(columnView.FocusedRowObject));
            };
        }

        public void Setup(IObjectSpace objectSpace, XafApplication application) {
            _objectSpace = objectSpace;
            Refresh();
        }

        public override void Refresh() => GetColumnView().GridControl.DataSource = GridControlDataSource(_objectSpace);
        protected virtual IList GridControlDataSource(IObjectSpace objectSpace) => objectSpace.GetObjects(GetObjectType());
        protected virtual Type GetObjectType() => throw new NotImplementedException();
        protected virtual ColumnView GetColumnView() => (ColumnView)Controls.OfType<GridControl>().FirstOrDefault()?.MainView;
    }
}
