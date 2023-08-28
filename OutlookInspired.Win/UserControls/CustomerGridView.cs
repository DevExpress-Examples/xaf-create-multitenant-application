using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Win.UserControls{
    public partial class CustomerGridView : ColumnViewUserControl{
        public CustomerGridView(){
            InitializeComponent();
        }

        public override Type ObjectType => typeof(Customer);
    }
}