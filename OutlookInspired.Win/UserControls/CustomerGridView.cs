using DevExpress.ExpressApp;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Win.UserControls
{
    public partial class CustomerGridView : ColumnViewUserControl
    {
        public CustomerGridView()
        {
            InitializeComponent();
        }

        public override Type ObjectType => typeof(Customer);
        public override void Setup(IObjectSpace objectSpace, XafApplication application)
        {
            base.Setup(objectSpace, application);
            if (!application.CanRead(typeof(CustomerEmployee)))
            {
                ColumnView.GridControl.LevelTree.Nodes.RemoveAt(0);
            }
            if (!application.CanRead(typeof(OrderItem)))
            {
                ColumnView.GridControl.LevelTree.Nodes.RemoveAt(ColumnView.GridControl.LevelTree.Nodes.Count == 1 ? 0 : 1);
            }
        }
    }
}