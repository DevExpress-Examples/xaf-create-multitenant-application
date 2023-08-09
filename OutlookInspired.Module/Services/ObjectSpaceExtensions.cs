using System.Linq.Expressions;
using DevExpress.ExpressApp;

namespace OutlookInspired.Module.Services{
    public static class ObjectSpaceExtensions{
        
        public static IEnumerable<IObjectSpace> YieldAll(this IObjectSpace objectSpace)
            => objectSpace is not CompositeObjectSpace compositeObjectSpace
                ? objectSpace.YieldItem()
                : objectSpace.YieldItem().Concat(compositeObjectSpace.AdditionalObjectSpaces);

        
        public static void DeleteObject(this IObjectSpace objectSpace, object obj)
            => objectSpace.Delete(objectSpace.GetObject(obj));
        
        public static bool Any<T>(this IObjectSpace objectSpace) 
            => objectSpace.GetObjectsQuery<T>().Any();
        public static int Count<T>(this IObjectSpace objectSpace, Expression<Func<T, bool>> expression=null)
            => objectSpace.GetObjectsQuery<T>().Where(expression??(arg =>true) ).Count();
        public static T FindObject<T>(this IObjectSpace objectSpace, Expression<Func<T,bool>> expression,bool inTransaction=false) 
            => objectSpace.GetObjectsQuery<T>(inTransaction).FirstOrDefault(expression);
    }
}