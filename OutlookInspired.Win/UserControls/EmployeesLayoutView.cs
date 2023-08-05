using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Win.Extensions;

namespace OutlookInspired.Win.UserControls
{
    public partial class EmployeesLayoutView : ColumnViewUserControl
    {
        public EmployeesLayoutView()
        {
            InitializeComponent();
            labelControl1.Text = @"RECORDS: 0";
            DataSourceOrFilterChanged += (_, _) => labelControl1.Text = $@"RECORDS: {ColumnView.DataRowCount}";
        }

        protected override Type GetObjectType()
        {
            return typeof(Employee);
        }



    }
}