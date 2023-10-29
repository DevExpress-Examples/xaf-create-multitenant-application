using System.Collections;
using System.Linq.Expressions;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor;
using DevExpress.Utils.Serializing.Helpers;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Module.Features.MasterDetail;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Blazor.Server.Components.Models{

    public interface IUserControlProcessObject{
        void ProcessSelectedObject();
    }

    public interface IModelSelectObject{
        IList Objects { get; }
        void SelectObject(object value);
    }

    public interface IUserControlDataSource{
        event EventHandler DataSourceChanged;
        IList Objects { get; }
    }

    public abstract class UserControlComponentModel<T>:UserControlComponentModel,IModelSelectObject, IUserControlDataSource,IUserControlProcessObject{
        IList IModelSelectObject.Objects => Objects;
        IList IUserControlDataSource.Objects => Objects;
        void IModelSelectObject.SelectObject(object value) => SelectObject((T)value);

        public List<T> Objects{
            get => GetPropertyValue<List<T>>();
            set{
                SetPropertyValue(value);
                OnDataSourceChanged();
            }
        }

        public event EventHandler DataSourceChanged;

        public void SelectObject(T value) => OnObjectSelected(new ObjectEventArgs(value));
        public override void Refresh() => Objects = ObjectSpace.GetObjects<T>(Criteria).ToList();
        public override Type ObjectType => typeof(T);

        protected virtual void OnDataSourceChanged() => DataSourceChanged?.Invoke(this, EventArgs.Empty);
    }
    public abstract class UserControlComponentModel:ComponentModelBase,IComponentContentHolder,IUserControl{
        private IList _selectedObjects = new List<object>();
        public CriteriaOperator Criteria{ get; private set; }
        public event AsyncEventHandler<ObjectEventArgs> ObjectSelected; 
        public abstract RenderFragment ComponentContent{ get; }
        public virtual object CurrentObject => SelectedObjects.Cast<object>().FirstOrDefault();
        public override void Setup(IObjectSpace objectSpace, XafApplication application){
            base.Setup(objectSpace, application);
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

        public virtual void Refresh(object currentObject) => Refresh();

        public event EventHandler<ObjectEventArgs> ProcessObject;
        public event EventHandler CriteriaChanged;
        public void SetCriteria<T>(Expression<Func<T, bool>> lambda) 
            => SetCriteria((LambdaExpression)lambda);

        public virtual void SetCriteria(string criteria){
            Criteria = CriteriaOperator.Parse(criteria);
            OnCriteriaChanged();
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
            => ProcessObject?.Invoke(this, new ObjectEventArgs(_selectedObjects.Cast<object>().First()));


        protected virtual void OnObjectSelected(ObjectEventArgs e) => ObjectSelected?.Invoke(this, e);

        protected virtual void OnCriteriaChanged() => CriteriaChanged?.Invoke(this, EventArgs.Empty);
    }

    public class SelectObjectEventArgs:EventArgs{
        public object Value{ get; }

        public SelectObjectEventArgs(object value) => Value = value;
    }
}