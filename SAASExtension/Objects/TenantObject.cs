using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.Validation;
using SAASExtension.Interfaces;
using SAASExtension.Security;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SAASExtension.BusinessObjects {

    //[DefaultClassOptions]
    [NavigationItem]
    [DefaultProperty(nameof(TenantObject.Name))]
    public class TenantObject : BaseObject, ITenant {

        [RuleRequiredField("RuleRequiredField for Tenant.Name", DefaultContexts.Save)]
        public virtual string Name { get; set; }
    }
    //[DefaultClassOptions]
    [NavigationItem]
    [DefaultProperty(nameof(TenantWithConnectionStringObject.Name))]
    public class TenantWithConnectionStringObject : TenantObject, IConnectionString {

        [RuleRequiredField("RuleRequiredField for Tenant.ConnectionString", DefaultContexts.Save)]
        public virtual string ConnectionString { get; set; }
    }
    //[DefaultClassOptions]
    [NavigationItem]
    [DefaultProperty(nameof(TenantWithConnectionStringWithUsersObject.Name))]
    public class TenantWithConnectionStringWithUsersObject : TenantWithConnectionStringObject {
        public virtual IList<SAASPermissionPolicyUserWithTenants> Users { get; set; } = new ObservableCollection<SAASPermissionPolicyUserWithTenants>();
    }
}
