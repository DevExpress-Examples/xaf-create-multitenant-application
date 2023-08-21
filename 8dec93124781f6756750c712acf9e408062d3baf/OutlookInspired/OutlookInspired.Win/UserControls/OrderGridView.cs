using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Win.UserControls
{
    public partial class OrderGridView : ColumnViewUserControl
    {
        public OrderGridView()
        {
            InitializeComponent();
        }

        protected override Type GetObjectType()
        {
            return typeof(Order);
        }
    }
}
