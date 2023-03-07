using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.Security;
using Microsoft.Extensions.Options;
using SAASExtension.BusinessObjects;
using SAASExtension.Interfaces;

namespace SAASExtension.Security {
    public class CustomAuthenticationStandardProvider : AuthenticationStandardProviderV2 {
        public CustomAuthenticationStandardProvider(IOptions<AuthenticationStandardProviderOptions> options,
        IOptions<SecurityOptions> securityOptions) :
            base(options, securityOptions) { }
        protected override AuthenticationBase CreateAuthentication(Type userType, Type logonParametersType) {
            return new CustomAuthentication(userType, logonParametersType);
        }
    }
    public class CustomAuthentication : AuthenticationStandard {
        public CustomAuthentication() : base() {
        }
        public CustomAuthentication(Type userType, Type logonParametersType) : base(userType, logonParametersType) {
        }
        public override object Authenticate(IObjectSpace objectSpace) {
            object result = base.Authenticate(objectSpace);
            IOwner user = result as IOwner;
            ITenantName parameters = LogonParameters as ITenantName;
            if ((parameters != null) && (user?.Owner != null) && (user.Owner != parameters.TenantName)) {
                throw new AuthenticationException(((ISecurityUser)user).UserName, SecurityExceptionLocalizer.GetExceptionMessage(SecurityExceptionId.RetypeTheInformation));
            }
            return result;
        }
    }
}
