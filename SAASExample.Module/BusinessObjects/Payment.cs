using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.Validation;
using SAASExtension.BusinessObjects;
using System.ComponentModel;


namespace SAASExample.Module.BusinessObjects {
    [DefaultClassOptions]
    [DefaultProperty(nameof(Payment.Salary))]
#if TenantFirst || LogInFirst
    public class Payment : BaseObject {
#endif
#if TenantFirstOneDatabase
    public class Payment : Tenant {
#endif

        [RuleRequiredField("RuleRequiredField for (Payment.Salary", DefaultContexts.Save)]
        public virtual string Salary { get; set; }
    }
}
