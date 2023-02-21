using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using SAASExtension.BusinessObjects;
using SAASExtension.Interfaces;
using SAASExtension.Security;

namespace SAASExample.Module.BusinessObjects;

[DefaultProperty(nameof(UserName))]
#if TenantFirst
public class ApplicationUser : PermissionPolicyUser, ISecurityUserWithLoginInfo {
#endif
#if TenantFirstOneDatabase
public class ApplicationUser : SAASPermissionPolicyUser, ISecurityUserWithLoginInfo {
#endif
#if LogInFirst
public class ApplicationUser : SAASPermissionPolicyUserWithTenants, ISecurityUserWithLoginInfo {
#endif
public ApplicationUser() : base() {
        UserLogins = new ObservableCollection<ApplicationUserLoginInfo>();
    }

    [Browsable(false)]
    [DevExpress.ExpressApp.DC.Aggregated]
    public virtual IList<ApplicationUserLoginInfo> UserLogins { get; set; }

    IEnumerable<ISecurityUserLoginInfo> IOAuthSecurityUser.UserLogins => UserLogins.OfType<ISecurityUserLoginInfo>();

    ISecurityUserLoginInfo ISecurityUserWithLoginInfo.CreateUserLoginInfo(string loginProviderName, string providerUserKey) {
        ApplicationUserLoginInfo result = ((IObjectSpaceLink)this).ObjectSpace.CreateObject<ApplicationUserLoginInfo>();
        result.LoginProviderName = loginProviderName;
        result.ProviderUserKey = providerUserKey;
        result.User = this;
        return result;
    }
}
