using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using SAASExtension.Interfaces;
using System.ComponentModel;
using SAASExtension.BusinessObjects;
using DevExpress.Persistent.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Collections.ObjectModel;

namespace SAASExtension.Security {
    public class SAASPermissionPolicyUser : PermissionPolicyUser, IOwner {
        private TenantNameHolder tenantName;
        private IReadOnlyList<TenantNameHolder> tenantNameObjs = null;
        [Browsable(false)]
        public virtual string Owner { get; set; }
        [DataSourceProperty(nameof(GetTenantNames), DataSourcePropertyIsNullMode.SelectAll)]
        [NotMapped]
        public TenantNameHolder TenantName {
            get {
                if (tenantName == null) {
                    tenantName = new TenantNameHolder(Owner);
                }
                return tenantName;
            }
            set {
                tenantName = value;
                Owner = value.Name;
            }
        }
        [Browsable(false)]
        [JsonIgnore]
        [NotMapped]
        public IReadOnlyList<TenantNameHolder> GetTenantNames {
            get {
                if (tenantNameObjs == null) {
                    tenantNameObjs = new List<TenantNameHolder>();
                    foreach (TenantObject tenant in ObjectSpace.CreateCollection(typeof(TenantObject))) {
                        ((List<TenantNameHolder>)tenantNameObjs).Add(new TenantNameHolder(tenant.Name));
                    }
                }
                return tenantNameObjs;
            }
        }
    }
    public class SAASPermissionPolicyUserWithTenants : PermissionPolicyUser {
        public virtual IList<TenantWithConnectionStringWithUsersObject> Tenants { get; set; } = new ObservableCollection<TenantWithConnectionStringWithUsersObject>();
    }
}
