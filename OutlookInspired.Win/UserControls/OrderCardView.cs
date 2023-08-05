using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Win.UserControls
{
    public partial class OrderCardView : ColumnViewUserControl
    {
        public OrderCardView()
        {
            InitializeComponent();
            labelControl1.Text = @"RECORDS: 0";
            DataSourceOrFilterChanged += (_, _) => labelControl1.Text = $@"RECORDS: {ColumnView.DataRowCount}";
        }

        protected override Type GetObjectType()
        {
            return typeof(Order);
        }

    }
}
