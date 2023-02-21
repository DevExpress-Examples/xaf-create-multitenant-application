using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using SAASExtension.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAASExtension.Options {
    public class InternalExtensionModuleOptions {
        public Action<ExtensionModule, List<PopupWindowShowAction>> AddSelectTenantStartupAction { get; set; }
        public List<Type> DeclaredExportedTypes { get; set; } = new List<Type>();
        public List<Type> RunTimeControllers { get; set; } = new List<Type>();
        public List<Action<ITypesInfo>> CustomizeTypesInfo { get; set; } = new List<Action<ITypesInfo>>();
        public List<Action<PermissionPolicyRole>> CustomizeDefaultRole { get; set; } = new List<Action<PermissionPolicyRole>>();
    }
    public class PublicExtensionModuleOptions {
        public bool ShowOwnerProperty { get; set; } = false;
        public string SelectTenantPropertyCaption { get; set; }
        public string TenantObjectDisplayName { get; set; }
        public string LogonFormCaption { get; set; }
        public bool RemoveExtraNavigationItems { get; set; } = true;
        public string SelectTenantFormCaption { get; set; }
    }
}
