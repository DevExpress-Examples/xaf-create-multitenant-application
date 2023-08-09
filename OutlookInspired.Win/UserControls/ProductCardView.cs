using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Win.UserControls
{
    public partial class ProductCardView : ColumnViewUserControl
    {
        public ProductCardView()
        {
            InitializeComponent();
            labelControl1.Text = @"RECORDS: 0";
        }

        protected override void OnDataSourceOfFilterChanged(){
            base.OnDataSourceOfFilterChanged();
            labelControl1.Text = $@"RECORDS: {ColumnView.DataRowCount}";
        }

        protected override Type GetObjectType()
        {
            return typeof(Product);
        }

    }
}