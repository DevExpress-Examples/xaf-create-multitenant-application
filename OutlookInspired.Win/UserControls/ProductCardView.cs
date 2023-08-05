using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Win.UserControls
{
    public partial class ProductCardView : ColumnViewUserControl
    {
        public ProductCardView()
        {
            InitializeComponent();
            labelControl1.Text = @"RECORDS: 0";
            DataSourceOrFilterChanged += (_, _) => labelControl1.Text = $@"RECORDS: {ColumnView.DataRowCount}";
        }

        protected override Type GetObjectType()
        {
            return typeof(Product);
        }

    }
}