using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Win.UserControls
{
    public partial class OrderGridView : ColumnViewUserControl
    {
        public OrderGridView()
        {
            InitializeComponent();
        }

        public override Type ObjectType => typeof(Order);
    }
}
