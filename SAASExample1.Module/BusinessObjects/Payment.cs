using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAASExample1.Module.BusinessObjects {
    [DefaultClassOptions]
    [DefaultProperty(nameof(Payment.Salary))]
    public class Payment : BaseObject {

        [RuleRequiredField("RuleRequiredField for (Payment.Salary", DefaultContexts.Save)]
        public virtual string Salary { get; set; }
    }
}
