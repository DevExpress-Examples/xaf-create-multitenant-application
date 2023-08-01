using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Win.UserControls
{
    public partial class EmployeesLayoutView : BaseUserControl
    {
        public EmployeesLayoutView()
        {
            InitializeComponent();
        }

        protected override Type GetObjectType()
        {
            return typeof(Employee);
        }


        public override void Refresh()
        {
            base.Refresh();
            labelControl1.Text = $@"RECORDS: {GetColumnView().DataRowCount.ToString()}";
        }
    }
}