using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.Validation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAASExample1.Module.BusinessObjects {

    [DefaultClassOptions]
    [DefaultProperty(nameof(Company.Name))]
    public class Company : BaseObject {

        [RuleRequiredField("RuleRequiredField for Company.Name", DefaultContexts.Save)]
        public virtual string Name { get; set; }
        [RuleRequiredField("RuleRequiredField for Company.ConnectionString", DefaultContexts.Save)]
        public virtual string ConnectionString { get; set; }

    }
}
