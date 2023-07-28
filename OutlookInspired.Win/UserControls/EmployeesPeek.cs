using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.XtraGrid;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Controllers;

namespace OutlookInspired.Win.UserControls
{


    public partial class EmployeesPeek : UserControl, IComplexControl, IMasterDetailService
    {
        private IObjectSpace _objectSpace;
        public event EventHandler<ObjectArgs> SelectedObjectChanged;
        public event EventHandler<ObjectArgs> ProcessObject;

        public EmployeesPeek()
        {
            InitializeComponent();
            // layoutView2.SelectionChanged += (sender, e) => SelectedObjectChanged?.Invoke(this, new ObjectArgs(layoutView2.GetRow(e.ControllerRow)));
            layoutView2.FocusedRowObjectChanged += (sender, e) =>
            {
                SelectedObjectChanged?.Invoke(this, new ObjectArgs(e.Row));
            };
            layoutView2.DoubleClick += (_, _) => ProcessObject?.Invoke(this, new ObjectArgs(layoutView2.FocusedRowObject));
        }

        public void Setup(IObjectSpace objectSpace, XafApplication application)
        {
            _objectSpace = objectSpace;
            Refresh();
        }

        public override void Refresh() => gridControl1.DataSource = _objectSpace.GetObjects<Employee>();

    }

}