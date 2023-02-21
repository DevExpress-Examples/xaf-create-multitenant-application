using DevExpress.Data.Filtering;
using DevExpress.Data.Linq;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp;
using System.Linq.Expressions;
using SAASExtension.Interfaces;

namespace SAASExtension.Security {
    public class CurrentUserOwnerOperator : ICustomFunctionOperatorConvertibleToExpression {
        public const string OperatorName = "CurrentUserOwner";
        private static readonly CurrentUserOwnerOperator instance = new CurrentUserOwnerOperator();

        public static void Register() {
            CustomFunctionOperatorHelper.Register(instance);
        }
        public static object CurrentUserOwner() {
            return instance.Evaluate();
        }
        public object Evaluate(params object[] operands) {
            return (SecuritySystem.CurrentUser as IOwner)?.Owner;
        }
        public string Name {
            get { return OperatorName; }
        }
        public Type ResultType(params Type[] operands) {
            return typeof(object);
        }
        Expression ICustomFunctionOperatorConvertibleToExpression.Convert(ICriteriaToExpressionConverter converter, params Expression[] operands) {
            return Expression.Constant((SecuritySystem.CurrentUser as IOwner)?.Owner);
        }
    }
}
