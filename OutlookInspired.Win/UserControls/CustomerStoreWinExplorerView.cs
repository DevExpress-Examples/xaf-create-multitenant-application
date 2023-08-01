using System.Collections;
using DevExpress.ExpressApp;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Win.UserControls
{
    public partial class CustomerStoreWinExplorerView : BaseUserControl
    {
        public CustomerStoreWinExplorerView()
        {
            InitializeComponent();
        }

        protected override IList GridControlDataSource(IObjectSpace objectSpace)
        {
            return base.GridControlDataSource(objectSpace);
        }

        protected override Type GetObjectType()
        {
            return typeof(CustomerStore);
        }
    }
}