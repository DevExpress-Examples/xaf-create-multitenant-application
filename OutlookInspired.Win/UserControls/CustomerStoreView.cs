using System.Collections;
using System.ComponentModel;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Win.UserControls
{
    
    public partial class CustomerStoreView : ColumnViewUserControl
    {
        public CustomerStoreView()
        {
            InitializeComponent();
            labelControl1.Text = $@"RECORDS: 0";
        }
        protected override void OnDataSourceOfFilterChanged(){
            base.OnDataSourceOfFilterChanged();
            labelControl1.Text = $@"RECORDS: {ColumnView.DataRowCount}";
        }

        public override void Refresh(object currentObject)
        {
            DataSource = ((Customer)currentObject).CustomerStores;
            base.Refresh(currentObject);
        }

        protected override Type GetObjectType() => typeof(CustomerStore);
    }
}