using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.Validation;
using SAASExtension.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAASExample.Module.BusinessObjects {
    [DefaultClassOptions]
    [DefaultProperty(nameof(Position.Title))]
#if TenantFirst || LogInFirst
    public class Position : BaseObject {
#endif
#if TenantFirstOneDatabase
    public class Position : Tenant {
#endif

        [RuleRequiredField("RuleRequiredField for Position.Title", DefaultContexts.Save)]
        public virtual string Title { get; set; }
    }
}
