using System;
using System.ComponentModel;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;

namespace OutlookInspired.Module.BusinessObjects {
    [DefaultProperty(nameof(State))]
    public class TaxRate : OutlookInspiredBaseObject {

        [RuleUniqueValue("", DefaultContexts.Save)]
        public virtual StateEnum State { get; set; }

        [ModelDefault("DisplayFormat", "{0:F2}")]
        [ModelDefault("EditMask", "F2")]
        public virtual decimal Rate { get; set; }
    }
}
