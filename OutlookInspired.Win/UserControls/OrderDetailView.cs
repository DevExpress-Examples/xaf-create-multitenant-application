using DevExpress.ExpressApp;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Win.UserControls
{
    public partial class OrderDetailView : ColumnViewUserControl
    {
        public OrderDetailView()
        {
            InitializeComponent();
            labelControl1.Text = @"RECORDS: 0";
        }

        protected override void OnDataSourceOfFilterChanged()
        {
            base.OnDataSourceOfFilterChanged();
            labelControl1.Text = $@"RECORDS: {ColumnView.DataRowCount}";
        }

        public override void Setup(IObjectSpace objectSpace, XafApplication application){
            base.Setup(objectSpace, application);
            if (!application.CanRead(typeof(Product)))
            {
                ColumnView.GridControl.LevelTree.Nodes.RemoveAt(0);
            }
        }

        public override Type ObjectType => typeof(Order);
    }
}
