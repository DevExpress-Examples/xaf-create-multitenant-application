using System.Collections;
using System.Linq.Expressions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.EFCore;
using DevExpress.Utils.Serializing.Helpers;
using DevExpress.XtraGrid.Views.Base;
using OutlookInspired.Module.Features.MasterDetail;
using OutlookInspired.Module.Services.Internal;
using OutlookInspired.Win.Extensions.Internal;

namespace OutlookInspired.Win.Editors{
    public partial class ColumnViewUserControl : UserControl, IUserControl{
        private EFCoreObjectSpace _objectSpace;
        protected ColumnView ColumnView;
        protected IList DataSource;
        private string _criteria;
        public ColumnViewUserControl() => Load += (_, _) => Refresh();
        public event EventHandler CurrentObjectChanged;
        public event EventHandler SelectionChanged;
        public event EventHandler SelectionTypeChanged;
        public event EventHandler<ObjectEventArgs> ProcessObject;
        
        public void SetCriteria<T>(Expression<Func<T, bool>> lambda) 
            => SetCriteria((LambdaExpression)lambda);
        
        public void SetCriteria(LambdaExpression lambda) 
            => SetCriteria(lambda.ToCriteria(ObjectType).ToString());

        public void SetCriteria(string criteria){
            _criteria = criteria;
            Refresh();
        }
        
        public virtual void Refresh(object currentObject) => Refresh();
        
        public virtual void Setup(IObjectSpace objectSpace, XafApplication application){
            _objectSpace = (EFCoreObjectSpace)objectSpace;
            ColumnView = this.ColumnView();
            ColumnView.FocusedRowObjectChanged += (_, _) => {
                SelectionChanged?.Invoke(this, EventArgs.Empty);
                CurrentObjectChanged?.Invoke(this, EventArgs.Empty);
            };
            ColumnView.DoubleClick += (_, _) => {
                if (!ColumnView.IsNotGroupedRow()) return;
                ProcessObject?.Invoke(this, new ObjectEventArgs(SelectedObjects.Cast<object>().First()));
            };
            ColumnView.ColumnFilterChanged += (_, _) => OnDataSourceOfFilterChanged();
            ColumnView.DataSourceChanged += (_, _) => OnDataSourceOfFilterChanged();
            ColumnView.DataError+=(_, e) => throw new AggregateException(e.DataException.Message,e.DataException);
            application.ProtectDetailViews(ColumnView.GridControl,ObjectType);
        }

        public override void Refresh(){
            ColumnView.GridControl.DataSource =
                (object)DataSource ?? _objectSpace.NewEntityServerModeSource(ObjectType, _criteria);
        }

        public virtual Type ObjectType => throw new NotImplementedException();

        public object CurrentObject => ColumnView.FocusedRowObject( _objectSpace,ObjectType);

        public IList SelectedObjects => ColumnView.GetSelectedRows()
            .Concat(ColumnView.FocusedRowHandle.YieldItem()).Distinct()
            .Select(i => ColumnView.GetRow(i)).ToArray();
        public SelectionType SelectionType => SelectionType.Full;
        public bool IsRoot => false;

        protected virtual void OnDataSourceOfFilterChanged(){
        }

        protected virtual void OnSelectionTypeChanged() => SelectionTypeChanged?.Invoke(this, EventArgs.Empty);
    }
}
