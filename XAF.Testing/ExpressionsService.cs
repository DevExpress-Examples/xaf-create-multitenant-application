using System.Linq.Expressions;

namespace XAF.Testing;
public static class ExpressionExtensions{
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
// namespace OutlookInspired.Tests.ImportData{
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