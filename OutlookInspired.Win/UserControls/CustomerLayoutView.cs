using DevExpress.XtraEditors;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Win.UserControls
{
    public partial class CustomerLayoutView : ColumnViewUserControl
    {
        public CustomerLayoutView()
        {
            InitializeComponent();
            labelControl1.Text = @"RECORDS: 0";
            DataSourceOrFilterChanged += (_, _) => labelControl1.Text = $@"RECORDS: {ColumnView.DataRowCount}";
        }

        protected override Type GetObjectType() => typeof(Customer);

    }
}