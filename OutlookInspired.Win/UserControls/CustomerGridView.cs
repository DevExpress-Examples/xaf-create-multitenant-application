using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Layout;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Win.Extensions;

namespace OutlookInspired.Win.UserControls
{
    public partial class CustomerGridView : BaseUserControl
    {
        public CustomerGridView()
        {
            InitializeComponent();
            // gridViewOrders.SetRelationName(nameof(Customer.Orders));
            // layoutViewEmployees.SetRelationName(nameof(Customer.Employees));
            // layoutViewEmployees.Configure(layoutViewColumnPhoto, layoutViewColumnAddress, layoutViewColumnFullName, layoutViewColumnEmail, layoutViewColumnPhone);

        }

        protected override Type GetObjectType()
        {
            return typeof(Customer);
        }
    }
}