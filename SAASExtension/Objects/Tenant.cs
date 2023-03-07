using DevExpress.ExpressApp;
using DevExpress.Persistent.BaseImpl.EF;
using SAASExtension.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAASExtension.BusinessObjects {
    public class Tenant : BaseObject {
        public static bool SetOwnerOnCreate = true;
        public virtual String Owner { get; set; }
        public override void OnCreated() {
            base.OnCreated();
            if (SetOwnerOnCreate) {
                Owner = (SecuritySystem.CurrentUser as IOwner)?.Owner;
            }
        }
    }
}
