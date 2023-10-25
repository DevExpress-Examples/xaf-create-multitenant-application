using System.Collections;
using System.Linq.Expressions;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Module.Features.MasterDetail;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Blazor.Server.Components.Models{

    public interface ISelectableModel{
        IList Objects { get; }
        void SelectObject(object value);
    }
    public abstract class UserControlComponentModel<T>:UserControlComponentModel,ISelectableModel{
        IList ISelectableModel.Objects => Objects;
        void ISelectableModel.SelectObject(object value) => SelectObject((T)value);

        public List<T> Objects{
            get => GetPropertyValue<List<T>>();
            set => SetPropertyValue(value);
        }
        public void SelectObject(T value) => OnObjectSelected(new SelectObjectEventArgs(value));
        public override void Refresh() => Objects = ObjectSpace.GetObjects<T>(Criteria).ToList();
        public override Type ObjectType => typeof(T);
    }
    public abstract class UserControlComponentModel:ComponentModelBase,IComponentContentHolder,IUserControl{
        private IList _selectedObjects = new List<object>();

        public CriteriaOperator Criteria{ get; private set; }

        public event AsyncEventHandler<SelectObjectEventArgs> ObjectSelected; 
        
        public IObjectSpace ObjectSpace{ get; private set; }

        public abstract RenderFragment ComponentContent{ get; }
        public virtual object CurrentObject => SelectedObjects.Cast<object>().FirstOrDefault();
        public virtual void Setup(IObjectSpace objectSpace, XafApplication application){
            ObjectSpace = objectSpace;
            Refresh();
        }

        public virtual IList SelectedObjects{
            get => _selectedObjects;
            set{
                _selectedObjects = value;
                OnSelectionChanged();
            }
        }

        public virtual SelectionType SelectionType=>SelectionType.Full;
        public virtual string Name=> GetType().Name;
        public virtual bool IsRoot => false;
        public event EventHandler CurrentObjectChanged;
        public event EventHandler SelectionChanged;
        public event EventHandler SelectionTypeChanged;

        public virtual void Refresh(){ }

        public virtual void Refresh(object currentObject) => throw new NotImplementedException();

        public event EventHandler ProcessObject;
        public void SetCriteria<T>(Expression<Func<T, bool>> lambda) 
            => SetCriteria((LambdaExpression)lambda);

        public virtual void SetCriteria(string criteria){
            Criteria = CriteriaOperator.Parse(criteria);
            Refresh();
        }

        public virtual void SetCriteria(LambdaExpression lambda) 
            => SetCriteria(lambda.ToCriteria(ObjectType).ToString());
        
        public abstract Type ObjectType{ get; }

        protected virtual void OnCurrentObjectChanged() 
            => CurrentObjectChanged?.Invoke(this, EventArgs.Empty);

        protected virtual void OnSelectionChanged(){
            CurrentObjectChanged?.Invoke(this, EventArgs.Empty);
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnSelectionTypeChanged() 
            => SelectionTypeChanged?.Invoke(this, EventArgs.Empty);

        public virtual void ProcessSelectedObject()
            => ProcessObject?.Invoke(this, EventArgs.Empty);


        protected virtual void OnObjectSelected(SelectObjectEventArgs e) => ObjectSelected?.Invoke(this, e);
    }

    public class SelectObjectEventArgs:EventArgs{
        public object Value{ get; }

        public SelectObjectEventArgs(object value) => Value = value;
    }
}