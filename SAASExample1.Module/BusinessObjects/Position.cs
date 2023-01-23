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
    [DefaultProperty(nameof(Position.Title))]
    public class Position : BaseObject {

        [RuleRequiredField("RuleRequiredField for Position.Title", DefaultContexts.Save)]
        public virtual string Title { get; set; }
    }
}
