using System.Reactive.Linq;
using DevExpress.ExpressApp;
using XAF.Testing.RX;

namespace XAF.Testing.XAF{
    public static class CollectionSourceExtensions{
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