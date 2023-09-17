using System.Collections;
using System.Linq.Expressions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.EFCore;
using DevExpress.XtraGrid.Views.Base;
using OutlookInspired.Module.Features.MasterDetail;
using OutlookInspired.Module.Services;
using OutlookInspired.Win.Extensions;

namespace OutlookInspired.Win.UserControls
{
    public partial class ColumnViewUserControl : UserControl, IUserControl
    {
        private EFCoreObjectSpace _objectSpace;
        protected ColumnView ColumnView;
        protected IList DataSource;
        private string _criteria;
        public ColumnViewUserControl() => Load += (_, _) => Refresh();
        public event EventHandler CurrentObjectChanged;
        public event EventHandler SelectionChanged;
        public event EventHandler SelectionTypeChanged;
        public event EventHandler ProcessObject;
        
        public void SetCriteria<T>(Expression<Func<T, bool>> lambda) 
            => SetCriteria((LambdaExpression)lambda);
        
        public void SetCriteria(LambdaExpression lambda) 
            => SetCriteria(lambda.ToCriteria(ObjectType).ToString());

        public void SetCriteria(string criteria){
            _criteria = criteria;
            Refresh();
        }
        
        public virtual void Refresh(object currentObject) => Refresh();

        public void Setup(IObjectSpace objectSpace, XafApplication application){
            _objectSpace = (EFCoreObjectSpace)objectSpace;
            ColumnView = this.ColumnView();
            ColumnView.SelectionChanged += (_, _) => {
                SelectionChanged?.Invoke(this, EventArgs.Empty);
                CurrentObjectChanged?.Invoke(this, EventArgs.Empty);
            };
            ColumnView.DoubleClick += (_, _) => {
                if (ColumnView.IsNotGroupedRow())
                    ProcessObject?.Invoke(this, EventArgs.Empty);
            };
            ColumnView.ColumnFilterChanged += (_, _) => OnDataSourceOfFilterChanged();
            ColumnView.DataSourceChanged += (_, _) => OnDataSourceOfFilterChanged();
            ColumnView.DataError+=(_, e) => throw new AggregateException(e.DataException.Message,e.DataException);
        }

        public override void Refresh(){
            ColumnView.GridControl.DataSource =
                (object)DataSource ?? _objectSpace.NewEntityServerModeSource(ObjectType, _criteria);
        }

        public virtual Type ObjectType => throw new NotImplementedException();

        public object CurrentObject => ColumnView.FocusedRowObject( _objectSpace,ObjectType);

        public IList SelectedObjects => ColumnView.GetSelectedRows().Select(i => ColumnView.GetRow(i)).ToArray();
        public SelectionType SelectionType => SelectionType.Full;
        public bool IsRoot => false;

        protected virtual void OnDataSourceOfFilterChanged(){
        }

        protected virtual void OnSelectionTypeChanged() => SelectionTypeChanged?.Invoke(this, EventArgs.Empty);
    }
}
