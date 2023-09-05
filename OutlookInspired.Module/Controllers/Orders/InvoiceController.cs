using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Module.Controllers.Orders{

    public class InvoiceReportDocumentController : ObjectViewController<DetailView, Order>{
        public InvoiceReportDocumentController() => TargetViewId = Order.InvoiceDetailView;
        protected override void OnActivated(){
            base.OnActivated();
            var order = (Order)View.CurrentObject;
            order.InvoiceDocument = order.MailMergeInvoice();
        }
    }

    public class InvoiceController : ObjectViewController<DetailView, Order>{
        public InvoiceController() => TargetViewId = Order.ChildDetailView;

        protected override void OnDeactivated(){
            base.OnDeactivated();
            View.CurrentObjectChanged-=ViewOnCurrentObjectChanged;
        }

        protected override void OnActivated(){
            base.OnActivated();
            View.CurrentObjectChanged+=ViewOnCurrentObjectChanged;
        }
        
        private void ViewOnCurrentObjectChanged(object sender, EventArgs e){
            var order = (Order)View.CurrentObject;
            order.InvoiceDocument = order.MailMergeInvoice().ToPdf();
            View.GetItems<PropertyEditor>().First(editor => editor.MemberInfo.Name==nameof(Order.InvoiceDocument)).ReadValue();
        }

    }
}