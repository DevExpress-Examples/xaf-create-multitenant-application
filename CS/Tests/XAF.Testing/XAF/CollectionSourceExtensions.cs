using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using DevExpress.ExpressApp;

namespace XAF.Testing.XAF{
    public static class CollectionSourceExtensions{
        public static void SetCriteria(this CollectionSourceBase collectionSourceBase, string key, Type type, LambdaExpression lambda) 
            => collectionSourceBase.Criteria[key] = lambda.ToCriteria(type);
        public static void SetCriteria<T>(this CollectionSourceBase collectionSourceBase, string key, Expression<Func<T, bool>> lambda) 
            => collectionSourceBase.SetCriteria(key, lambda.Parameters.First().Type,lambda);
        public static void SetCriteria<T>(this CollectionSourceBase collectionSourceBase, Expression<Func<T, bool>> lambda,[CallerMemberName]string callMemberName="") 
            => collectionSourceBase.SetCriteria(callMemberName,lambda);
        
        public static void SetCriteria(this CollectionSourceBase collectionSourceBase, LambdaExpression lambda,[CallerMemberName]string caller="") 
            => collectionSourceBase.SetCriteria(caller,collectionSourceBase.ObjectTypeInfo.Type, lambda);
        
        public static IObservable<CollectionSourceBase> WhenCriteriaApplied(this CollectionSourceBase collectionSourceBase)
            => collectionSourceBase.WhenEvent(nameof(CollectionSourceBase.CriteriaApplied))
                .TakeUntil(collectionSourceBase.WhenDisposed()).To(collectionSourceBase);
        public static IObservable<T> WhenCollectionChanged<T>(this T collectionSourceBase) where T:CollectionSourceBase 
            => collectionSourceBase.WhenEvent(nameof(CollectionSourceBase.CollectionChanged)).To(collectionSourceBase)
                .TakeUntil(collectionSourceBase.WhenDisposed());
        public static IObservable<T> WhenDisposed<T>(this T collectionSourceBase) where T:CollectionSourceBase 
            => collectionSourceBase.WhenEvent(nameof(CollectionSourceBase.Disposed)).To(collectionSourceBase);
    }
}