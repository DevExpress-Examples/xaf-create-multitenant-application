using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Controllers;
using OutlookInspired.Win.Extensions;

namespace OutlookInspired.Win.UserControls
{
    [DetailUserControl]
    public partial class CustomerStoreView : ColumnViewUserControl
    {
        public CustomerStoreView()
        {
            InitializeComponent();
            DataSourceOrFilterChanged += (_, _) => labelControl1.Text = $@"RECORDS: {ColumnView.DataRowCount}";
        }

        public override void Refresh(object currentObject)
        {
            DataSource = ((Customer)currentObject).CustomerStores;
            base.Refresh(currentObject);
        }

        protected override Type GetObjectType() => typeof(CustomerStore);
    }
}