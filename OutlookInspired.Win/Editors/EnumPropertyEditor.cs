using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using EditorAliases = OutlookInspired.Module.Services.Internal.EditorAliases;

namespace OutlookInspired.Win.Editors{
    [PropertyEditor(typeof(Enum),EditorAliases.EnumImageOnlyEditor,false)]
    public class EnumPropertyEditor:DevExpress.ExpressApp.Win.Editors.EnumPropertyEditor{
        public EnumPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model){
        }

        protected override void SetupRepositoryItem(RepositoryItem item){
            base.SetupRepositoryItem(item);
            ((RepositoryItemEnumEdit)item).GlyphAlignment=HorzAlignment.Center;
        }
    }
}