using System.Linq.Expressions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;

namespace OutlookInspired.Module.Controllers{

    public interface IUserControl:ISelectionContext,IComplexControl{
        void Refresh(object currentObject);
        event EventHandler ProcessObject;
        void SetCriteria<T>(Expression<Func<T, bool>> lambda);
        void SetCriteria(string criteria);
        void SetCriteria(LambdaExpression lambda);
        Type ObjectType{ get; }
    }
}