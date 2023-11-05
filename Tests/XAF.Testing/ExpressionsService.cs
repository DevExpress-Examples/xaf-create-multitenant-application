using System.Linq.Expressions;

namespace XAF.Testing;
public static class ExpressionExtensions{
    public static string MemberExpressionName<TObject, TMemberValue>(this Expression<Func<TObject, TMemberValue>> memberName) 
        => memberName.Body switch{
            MemberExpression memberExpression => memberExpression.Member.Name,
            UnaryExpression{ Operand: MemberExpression operand } => operand.Member.Name,
            _ => throw new ArgumentException("Invalid expression type", nameof(memberName))
        };

    
    public static LambdaExpression Filter(this LambdaExpression  expr,Func<Type,bool> match){
        var body = expr.Body.FilterExpression(match);
        return body == null ? null : Expression.Lambda(body, expr.Parameters);
    }

    private static Expression FilterExpression(this Expression expr,Func<Type,bool> match){
        switch (expr.NodeType){
            case ExpressionType.AndAlso:
                var binaryExpr = (BinaryExpression)expr;
                var left = binaryExpr.Left.FilterExpression(match);
                var right = binaryExpr.Right.FilterExpression(match);
                return left == null ? right : right == null ? left : Expression.AndAlso(left, right);
            case ExpressionType.Call:
                var methodCallExpr = (MethodCallExpression)expr;
                return methodCallExpr.Method.Name == "Any" ? methodCallExpr.Arguments[0] is MemberExpression instance
                    ? instance.Type.IsGenericType ? !match(instance.Type.GetGenericArguments()[0]) ? null : expr : expr : expr : expr;
            default:
                return expr;
        }
    }


    public static MethodCallExpression Call(this Type type,string methodName,Type[] typeArguments, params Expression[] arguments)
        => Expression.Call(type, methodName, typeArguments,arguments);
    
    
    public static T FuseAny<T>(this T expression,params LambdaExpression[] expressions) where T:Expression
        => (T)new ExpressionReplacer(expressions).Visit(expression);
    
    class ExpressionReplacer : ExpressionVisitor{
        private readonly LambdaExpression[] _expressions;

        public ExpressionReplacer(LambdaExpression[] expressions) => _expressions = expressions;
        
        protected override Expression VisitMethodCall(MethodCallExpression node){
            if (node.Method.Name == "Any"){
                var arg = node.Arguments[0];
                if (arg.Type.IsGenericType){
                    var genericArguments = arg.Type.GetGenericArguments();
                    if (genericArguments.Length == 1){
                        var type = genericArguments.First();
                        var lambdaExpression = _expressions.FirstOrDefault(expression => expression.Parameters.First().Type==type);
                        if (lambdaExpression != null){
                            return typeof(Enumerable).Call("Any", new[]{ type }, arg, lambdaExpression);    
                        }
                    }
                }
            }
            return base.VisitMethodCall(node);
        }
    }

}

