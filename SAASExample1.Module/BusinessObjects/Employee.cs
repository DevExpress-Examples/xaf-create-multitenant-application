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
    [DefaultProperty(nameof(Employee.Name))]
    public class Employee : BaseObject {

        [RuleRequiredField("RuleRequiredField for Employee.Name", DefaultContexts.Save)]
        public virtual string Name { get; set; }
    }
}
