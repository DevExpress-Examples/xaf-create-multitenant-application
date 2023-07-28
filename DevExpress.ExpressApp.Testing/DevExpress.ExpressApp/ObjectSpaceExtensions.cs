using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using DevExpress.ExpressApp.Testing.RXExtensions;
using Observable = System.Reactive.Linq.Observable;
using Unit = System.Reactive.Unit;

namespace DevExpress.ExpressApp.Testing.DevExpress.ExpressApp{
    public enum ObjectModification{
        All,
        New,
        NewOrUpdated,
        NewOrDeleted,
        Updated,
        UpdatedOrDeleted,
        Deleted
    }

    public static class ObjectSpaceExtensions{
        public static T FindObject<T>(this IObjectSpace objectSpace, Expression<Func<T,bool>> expression,bool inTransaction=false) 
            => objectSpace.GetObjectsQuery<T>(inTransaction).FirstOrDefault(expression);
        public static object FindObject(this IObjectSpace objectSpace, Type objectType) 
            => objectSpace.FindObject(objectType,null);
        public static int Count<T>(this IObjectSpace objectSpace, Expression<Func<T, bool>> expression=null)
            => objectSpace.GetObjectsQuery<T>().Where(expression??(arg =>true) ).Count();
        public static IObjectSpace NewObjectSpace(this XafApplication application) 
            => application.CreateObjectSpace(typeof(object));
        private static IEnumerable<IObjectSpace> YieldAll(this IObjectSpace objectSpace)
            => objectSpace is not CompositeObjectSpace compositeObjectSpace
                ? objectSpace.YieldItem()
                : objectSpace.YieldItem().Concat(compositeObjectSpace.AdditionalObjectSpaces);
        
        public static IObservable<(IObjectSpace objectSpace, IEnumerable<T> objects)> WhenCommitted<T>(
            this IObjectSpace objectSpace, ObjectModification objectModification ,[CallerMemberName]string caller="") 
            => objectSpace.WhenCommitingDetailed<T>(objectModification, true,caller:caller)
                .Select(t => (t.objectSpace,t.details.Select(t1 => t1.instance)));
        
        public static IObservable<T> ToObjects<T>(this IObservable<(IObjectSpace objectSpace, T[] objects)> source)
            =>source.SelectMany(t => t.objects.Select(arg => arg));
        public static IObservable<T> ToObjects<T>(this IObservable<(IObjectSpace objectSpace, IEnumerable<T> objects)> source)
            =>source.SelectMany(t => t.objects.Select(arg => arg));
        
        public static IObservable<(IObjectSpace objectSpace, IEnumerable<T> objects)> WhenCommiting<T>(this IObjectSpace objectSpace, 
            ObjectModification objectModification = ObjectModification.All,bool emitAfterCommit = false) 
            => objectSpace.WhenCommitingDetailed<T>(objectModification, emitAfterCommit)
                .Select(t => (t.objectSpace,t.details.Select(t1 => t1.instance)));

        public static IObservable<(IObjectSpace objectSpace, (T instance, ObjectModification modification)[] details)>
            WhenCommitingDetailed<T>(this IObjectSpace objectSpace, ObjectModification objectModification, bool emitAfterCommit,Func<T,bool> criteria=null,[CallerMemberName]string caller="") 
            => objectSpace.WhenCommitingDetailed(emitAfterCommit, objectModification,criteria);
        public static IObservable<CancelEventArgs> WhenCommiting(this IObjectSpace objectSpace) 
            => objectSpace.WhenEvent<CancelEventArgs>(nameof(IObjectSpace.Committing))
                .TakeUntil(objectSpace.WhenDisposed());

        public static IObservable<(IObjectSpace objectSpace, (T instance, ObjectModification modification)[] details)> WhenCommitingDetailed<T>(
            this IObjectSpace objectSpace, bool emitAfterCommit, ObjectModification objectModification,Func<T,bool> criteria=null) 
            => objectSpace.WhenCommiting().SelectMany(_ => {
                    var modifiedObjects = objectSpace.ModifiedObjects<T>(objectModification).Where(t => criteria==null|| criteria.Invoke(t.instance)).ToArray();
                    return modifiedObjects.Any() ? emitAfterCommit ? objectSpace.WhenCommitted().Take(1).Select(space => (space, modifiedObjects))
                        : (objectSpace, modifiedObjects).Observe() : Observable.Empty<(IObjectSpace, (T instance, ObjectModification modification)[])>();
                });
        
        public static IEnumerable<(T instance, ObjectModification modification)> ModifiedObjects<T>(this IObjectSpace objectSpace, ObjectModification objectModification) 
            => objectSpace.ModifiedObjects(objectModification).Where(t => t.instance is T).Select(t => ((T)t.instance,t.modification));

        internal static IEnumerable<TSource> WhereNotDefault<TSource>(this IEnumerable<TSource> source) {
            var type = typeof(TSource);
            if (type.IsClass || type.IsInterface){
                return source.Where(source1 => source1!=null);   
            }
            var instance = type.CreateInstance();
            return source.Where(source1 => !source1.Equals(instance));
        }
        
        public static IEnumerable<(object instance, ObjectModification modification)> ModifiedObjects(this IObjectSpace objectSpace,ObjectModification objectModification) 
            => objectSpace.ModifiedObjects( objectModification, objectSpace.YieldAll()
                .SelectMany(space => space.GetObjectsToDelete(true).Cast<object>().Concat(space.GetObjectsToSave(true).Cast<object>())).Distinct()).WhereNotDefault();

        public static IEnumerable<(T o, ObjectModification modification)> ModifiedObjects<T>(this IObjectSpace objectSpace, ObjectModification objectModification, IEnumerable<T> objects) where T:class 
            => objects.Select(o => {
                if (objectSpace.IsDeletedObject(o) && objectModification.HasAnyValue(ObjectModification.Deleted,
                        ObjectModification.All, ObjectModification.NewOrDeleted, ObjectModification.UpdatedOrDeleted)) {
                    return (o, ObjectModification.Deleted);
                }

                if (objectSpace.IsNewObject(o) && objectModification.HasAnyValue(ObjectModification.New,
                        ObjectModification.All, ObjectModification.NewOrDeleted, ObjectModification.NewOrUpdated)) {
                    return (o, ObjectModification.New);
                }

                if (objectSpace.IsUpdated(o) && objectModification.HasAnyValue(ObjectModification.Updated,
                        ObjectModification.All, ObjectModification.UpdatedOrDeleted, ObjectModification.NewOrUpdated)) {
                    return (o, ObjectModification.Updated);
                }

                return default;
            });
        static bool HasAnyValue(this ObjectModification value, params ObjectModification[] values) => values.Any(@enum => value == @enum);
        public static bool IsUpdated<T>(this IObjectSpace objectSpace, T t) where T:class 
            => !objectSpace.IsNewObject(t)&&!objectSpace.IsDeletedObject(t);
        public static IObservable<IObjectSpace> WhenCommitted(this IObjectSpace objectSpace) 
            => objectSpace.WhenEvent(nameof(IObjectSpace.Committed)).To(objectSpace)
                .TakeUntil(objectSpace.WhenDisposed());
        
        public static IObservable<Unit> WhenDisposed(this IObjectSpace objectSpace)
            => objectSpace.WhenEvent(nameof(IObjectSpace.Disposed)).ToUnit();



    }
}