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
    
    public static Expression<TDelegate> Lambda<TDelegate>(this Expression body,
        params ParameterExpression[] parameters){
        return Expression.Lambda<TDelegate>(body, parameters);
    }
    
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

// using System.Linq.Expressions;
//
// namespace OutlookInspired.Win.Tests.ImportData{
//     public static class ExpressionExtensions{
//         public static Expression<TDelegate> Lambda<TDelegate>(this Expression body, params ParameterExpression[]? parameters)
//             => Expression.Lambda<TDelegate>(body, parameters);
//         
//         public static BinaryExpression AndAlso(this Expression expression, Expression right)
//             => Expression.AndAlso(expression, right);
//         
//         public static InvocationExpression Invoke(this Expression expression, params ParameterExpression[] parameterExpressions)
//             => Expression.Invoke(expression, parameterExpressions.Cast<Expression>());
//         
//         public static ParameterExpression ParameterExpression(this Type type, string name=null)
//             => Expression.Parameter(type, name??type.Name);
//         public static Expression<Func<TParent, bool>> Combine<TParent, TChild>(this Expression<Func<TChild, bool>> childCondition,
//             Expression<Func<TParent, IEnumerable<TChild>>> collectionSelector){
//             var parentParam = Expression.Parameter(typeof(TParent), "parent");
//             var childParam = Expression.Parameter(typeof(TChild), "child");
//             var methodInfo = typeof(Enumerable).GetMethods().First(m => m.Name == "Any" && m.GetParameters().Length == 2).MakeGenericMethod(typeof(TChild));
//             var invocationExpression = collectionSelector.Invoke( parentParam);
//             var expression = new ParameterReplacer(childParam).Visit(childCondition.Body);
//             return Expression.Lambda<Func<TParent, bool>>(
//                 Expression.Call(methodInfo,
//                 invocationExpression,
//                 Expression.Lambda(expression, childParam)
//             ), parentParam);
//         }
//         class ParameterReplacer : ExpressionVisitor{
//             private readonly ParameterExpression _parameter;
//             public ParameterReplacer(ParameterExpression parameter) => _parameter = parameter;
//             protected override Expression VisitParameter(ParameterExpression node) => _parameter;
//         }
//     }
//
//     
// }