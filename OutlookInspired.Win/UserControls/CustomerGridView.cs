using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Win.UserControls{
    public partial class CustomerGridView : ColumnViewUserControl{
        public CustomerGridView(){
            InitializeComponent();
        }

        protected override Type GetObjectType(){
            return typeof(Customer);
        }
    }
}