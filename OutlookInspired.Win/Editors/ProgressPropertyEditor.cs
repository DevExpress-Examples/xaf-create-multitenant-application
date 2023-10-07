using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using EditorAliases = OutlookInspired.Module.Services.Internal.EditorAliases;

namespace OutlookInspired.Win.Editors {
    [PropertyEditor(typeof(int), EditorAliases.ProgressEditor, false)]
    [PropertyEditor(typeof(double), EditorAliases.ProgressEditor, false)]
    public class ProgressPropertyEditor : DXPropertyEditor {
        public ProgressPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) { }
        protected override object CreateControlCore() => new ProgressBarControl();

        class RepositoryItemProgressBar:DevExpress.XtraEditors.Repository.RepositoryItemProgressBar,IValueCalculator{
            public object Calculate(object value) => Convert.ToDecimal(value) * 100;
        }
        protected override RepositoryItem CreateRepositoryItem()
            => new RepositoryItemProgressBar(){
                PercentView = true, ShowTitle = true, DisplayFormat ={ FormatType = FormatType.Numeric, FormatString = "{0}%" },
                Appearance ={
                    Options = { UseTextOptions = true},TextOptions = { HAlignment = HorzAlignment.Center}
                }
            };
    }
    public interface IValueCalculator{
        object Calculate(object eValue);
    }

}
