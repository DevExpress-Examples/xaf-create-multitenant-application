using System.Linq.Expressions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.Utils.Serializing.Helpers;

namespace OutlookInspired.Module.Features.MasterDetail{
    public interface IUserControl:ISelectionContext,IComplexControl{
        void Refresh(object currentObject);
        event EventHandler<ObjectEventArgs> ProcessObject;
        void SetCriteria<T>(Expression<Func<T, bool>> lambda);
        void SetCriteria(string criteria);
        void SetCriteria(LambdaExpression lambda);
        Type ObjectType{ get; }
    }
}