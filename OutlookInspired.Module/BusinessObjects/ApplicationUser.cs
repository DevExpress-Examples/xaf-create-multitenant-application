using System.Collections.ObjectModel;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;

namespace OutlookInspired.Module.BusinessObjects;
[DefaultProperty(nameof(UserName))]
public class ApplicationUser : PermissionPolicyUser, ISecurityUserWithLoginInfo {
    [Browsable(false)]
    [DevExpress.ExpressApp.DC.Aggregated]
    public virtual IList<ApplicationUserLoginInfo> UserLogins { get; set; }= new ObservableCollection<ApplicationUserLoginInfo>();

    IEnumerable<ISecurityUserLoginInfo> IOAuthSecurityUser.UserLogins => UserLogins;

    ISecurityUserLoginInfo ISecurityUserWithLoginInfo.CreateUserLoginInfo(string loginProviderName, string providerUserKey) {
        var result = ((IObjectSpaceLink)this).ObjectSpace.CreateObject<ApplicationUserLoginInfo>();
        result.LoginProviderName = loginProviderName;
        result.ProviderUserKey = providerUserKey;
        result.User = this;
        return result;
    }
}
