using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Win.Editors;

namespace OutlookInspired.Win.Features.Orders
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
