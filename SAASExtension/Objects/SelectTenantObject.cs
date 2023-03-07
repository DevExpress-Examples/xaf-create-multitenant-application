using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using Microsoft.Extensions.DependencyInjection;
using SAASExtension.Interfaces;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace SAASExtension.BusinessObjects {
    [DomainComponent]
    public class SelectTenantNameObject : NonPersistentLiteObject {
        private IServiceProvider serviceProvider;
        private TenantNameHolder tenantName;

        [DataSourceProperty(nameof(GetTenantNames), DataSourcePropertyIsNullMode.SelectAll)]
        [ImmediatePostData]
        public TenantNameHolder TenantName {
            get { return tenantName; }
            set {
                SetPropertyValue(ref tenantName, value);
            }
        }

        [Browsable(false)]
        [JsonIgnore]
        public IReadOnlyList<TenantNameHolder> GetTenantNames {
            get {
                IReadOnlyList<TenantNameHolder> tenantNameObjs = new List<TenantNameHolder>();
                if (serviceProvider != null) {
                    ITenantNamesHelper tenantNamesHelper = serviceProvider.GetRequiredService<ITenantNamesHelper>();
                    foreach (var name in tenantNamesHelper.GetTenantNamesMap().Keys) {
                        ((List<TenantNameHolder>)tenantNameObjs).Add(new TenantNameHolder(name));
                    }
                }
                return tenantNameObjs;
            }
        }

        public void SetServiceProvider(IServiceProvider serviceProvider) {
            this.serviceProvider = serviceProvider;
        }
    }
}
