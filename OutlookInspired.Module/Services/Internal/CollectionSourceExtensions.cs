using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;

namespace OutlookInspired.Module.Services.Internal{
    internal static class CollectionSourceExtensions{
        public static void SetCriteria<T>(this CollectionSourceBase collectionSourceBase, string key, Expression<Func<T, bool>> lambda) 
            => collectionSourceBase.Criteria[key]=CriteriaOperator.FromLambda(lambda);
        public static void SetCriteria<T>(this CollectionSourceBase collectionSourceBase, Expression<Func<T, bool>> lambda,[CallerMemberName]string callMemberName="") 
            => collectionSourceBase.SetCriteria(callMemberName,lambda);
    }
}