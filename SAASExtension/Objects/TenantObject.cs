using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.Validation;
using SAASExtension.Interfaces;
using SAASExtension.Security;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SAASExtension.BusinessObjects {

    [NavigationItem]
    [DefaultProperty(nameof(TenantObject.Name))]
    public class TenantObject : BaseObject, ITenant, IConnectionString {

        [RuleRequiredField("RuleRequiredField for Tenant.Name", DefaultContexts.Save)]
        public virtual string Name { get; set; }
        public virtual string ConnectionString { get; set; }
    }
    [NavigationItem]
    [DefaultProperty(nameof(TenantObjectWithUsers.Name))]
    public class TenantObjectWithUsers : TenantObject {
        public virtual IList<SAASPermissionPolicyUserWithTenants> Users { get; set; } = new ObservableCollection<SAASPermissionPolicyUserWithTenants>();
    }
}
