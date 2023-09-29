using System.Collections;
using System.Linq.Expressions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.Components.Models;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Module.Features.MasterDetail;

namespace OutlookInspired.Blazor.Server.Components{
    public abstract class UserControlComponentModel:ComponentModelBase,IComponentContentHolder,IUserControl{
        public abstract RenderFragment ComponentContent{ get; }
        public virtual object CurrentObject => SelectedObjects.Cast<object>().FirstOrDefault();
        public virtual IList SelectedObjects{ get; } = new List<object>();
        public virtual SelectionType SelectionType=>SelectionType.Full;
        public virtual string Name=> GetType().Name;
        public virtual bool IsRoot => false;
        public event EventHandler CurrentObjectChanged;
        public event EventHandler SelectionChanged;
        public event EventHandler SelectionTypeChanged;
        public virtual void Setup(IObjectSpace objectSpace, XafApplication application){
        }

        public virtual void Refresh(){
        }

        public virtual void Refresh(object currentObject){
        }

        public event EventHandler ProcessObject;
        public void SetCriteria<T>(Expression<Func<T, bool>> lambda){
        }

        public virtual void SetCriteria(string criteria){
            
        }

        public virtual void SetCriteria(LambdaExpression lambda){
            
        }
        public abstract Type ObjectType{ get; }

        protected virtual void OnCurrentObjectChanged() 
            => CurrentObjectChanged?.Invoke(this, EventArgs.Empty);

        protected virtual void OnSelectionChanged(){
            CurrentObjectChanged?.Invoke(this, EventArgs.Empty);
            SelectionChanged?.Invoke(this, EventArgs.Empty);
            
        }

        protected virtual void OnSelectionTypeChanged() 
            => SelectionTypeChanged?.Invoke(this, EventArgs.Empty);

        protected virtual void OnProcessObject()
            => ProcessObject?.Invoke(this, EventArgs.Empty);
    }
}