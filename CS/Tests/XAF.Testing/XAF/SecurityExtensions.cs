using System.Linq.Expressions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using static DevExpress.ExpressApp.Security.SecurityOperations;

namespace XAF.Testing.XAF{
    public static class SecurityExtensions{
        public static IObservable<LambdaExpression> FuseAny(this XafApplication application, params LambdaExpression[] expressions)
            => expressions.Select(expression => application.FuseAny(expression, expressions)).ToArray().ToNowObservable().WhenNotDefault();
        public static LambdaExpression FuseAny(this XafApplication application, LambdaExpression expression,params LambdaExpression[] expressions) 
            => expression.Filter(application.CanRead).FuseAny(expressions.Select(lambdaExpression => lambdaExpression.Filter(application.CanRead))
                .WhereNotDefault().ToArray());
        
        public static bool CanRead(this XafApplication application,Type objectType) 
            => application.Security.IsGranted( new PermissionRequest(objectType, Read));

        public static bool IsGranted(this ISecurityStrategyBase security, PermissionRequest permissionRequest) 
            => ((IRequestSecurity)security).IsGranted(permissionRequest);

        public static bool IsAdmin(this ISecurityUserWithRoles applicationUser)
            => applicationUser.Roles.OfType<IPermissionPolicyRole>().Any(role => role.IsAdministrative);
    }
}