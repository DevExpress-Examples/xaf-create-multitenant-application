using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Win.UserControls
{
    public partial class CustomerLayoutView : BaseUserControl
    {
        public CustomerLayoutView() => InitializeComponent();

        protected override Type GetObjectType() => typeof(Customer);

        public override void Refresh()
        {
            base.Refresh();
            labelControl1.Text = $@"RECORDS: {GetColumnView().DataRowCount.ToString()}";
        }
    }
}