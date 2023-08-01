using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using OutlookInspired.Module.Attributes;

namespace OutlookInspired.Module.BusinessObjects{
    [DomainComponent]
    [CloneView(CloneViewType.DetailView, "EmployeeLayoutView_DetailView")]
    [CloneView(CloneViewType.DetailView, "CustomerGridView_DetailView")]
    [CloneView(CloneViewType.DetailView, "CustomerLayoutView_DetailView")]
    [CloneView(CloneViewType.DetailView, "CustomerLayoutView_DetailView_Child")]
    public class UserControlObject:NonPersistentBaseObject{
		
    }
}