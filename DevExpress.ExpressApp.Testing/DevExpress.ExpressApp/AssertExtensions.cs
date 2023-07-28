using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp.Testing.RXExtensions;

namespace DevExpress.ExpressApp.Testing.DevExpress.ExpressApp{
    public static class AssertExtensions{
        public static IObservable<Unit> AssertChangeViewVariant(this IObservable<Frame> source,string variantId) 
            => source.ChangeViewVariant(variantId).Assert().ToUnit();
        public static IObservable<Unit> AssertProcessSelectedObject(this IObservable<Window> source)
            => source.SelectMany(window => window.AssertProcessSelectedObject());

        public static IObservable<Unit> AssertCreateNewObject(this Window window)
            => window.CreateNewObject().Assert().ToUnit();

        public static IObservable<Unit> AssertSaveNewObject(this XafApplication application)
            => application.SaveNewObject().Assert().ToUnit();
        
        public static IObservable<Unit> AssertDeleteCurrentObject(this XafApplication application)
            => application.DeleteCurrentObject().Assert().ToUnit();

        public static IObservable<Unit> AssertCreateNewObject(this IObservable<Window> source)
            => source.SelectMany(window => window.AssertCreateNewObject());
        public static IObservable<Unit> AssertProcessSelectedObject(this Window window) 
            => window.ProcessSelectedObject().Assert();
        
        public static IObservable<Unit> AssertExistingObjectDetailView(this XafApplication application,Type objectType=null) 
            => application.ExistingObjectRootDetailView(objectType).Assert().ToUnit();

        public static IObservable<Unit> AssertNavigate(this XafApplication application, string viewId)
            => application.Navigate(viewId).Assert().ToUnit();
        
        public static IObservable<Unit> AssertWindowHasObjects(this IObservable<Window> source)
            => source.HasObjects().Assert();
        
         
    }
}