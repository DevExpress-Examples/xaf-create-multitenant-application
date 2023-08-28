using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using DevExpress.Data.Filtering;

namespace XAF.Testing.XAF{
    public static class CriteriaOperatorExtensions{
        static readonly MethodInfo FromLambdaMethod=typeof(CriteriaOperator).GetMethods(BindingFlags.Public|BindingFlags.Static)
            .First(info => info.Name=="FromLambda"&&info.GetGenericArguments().Length==1);

        static readonly ConcurrentDictionary<Type, MethodInfo> FromLambdaCache = new();
        public static CriteriaOperator ToCriteria(this LambdaExpression expression,Type objectType)
            =>(CriteriaOperator)FromLambdaCache.GetOrAdd(objectType, t => FromLambdaMethod!.MakeGenericMethod(t))
                .Invoke(null, new object[] { expression });
    }
}