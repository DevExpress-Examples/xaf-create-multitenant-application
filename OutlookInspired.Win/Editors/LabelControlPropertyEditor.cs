using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using OutlookInspired.Module.Attributes;
using EditorAliases = OutlookInspired.Module.Services.EditorAliases;

namespace OutlookInspired.Win.Editors{
    // [PropertyEditor(typeof(string),EditorAliases.CustomStringPropertyEditor,false)]
    // public class CustomStringPropertyEditor:StringPropertyEditor{
    //     public CustomStringPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model){
    //     }
    //
    //     protected override void SetupRepositoryItem(RepositoryItem item){
    //         base.SetupRepositoryItem(item);
    //         var repositoryItemStringEdit = ((RepositoryItemStringEdit)item);
    //         repositoryItemStringEdit.ReadOnly = true;
    //         var appearanceObject = repositoryItemStringEdit.Appearance;
    //         appearanceObject.BackColor=Color.Red;
    //         appearanceObject.FontSizeDelta = 8;
    //     }
    //
    //     protected override object CreateControlCore(){
    //         var controlCore = (StringEdit)base.CreateControlCore();
    //         controlCore.AutoSize=true;
    //         controlCore.Properties.Appearance.ForeColor=Color.Red;
    //         return controlCore;
    //     }
    //
    //     protected override void SetRepositoryItemReadOnly(RepositoryItem item, bool readOnly){
    //         item.ReadOnly = true;
    //     }
    // }
    [PropertyEditor(typeof(object),EditorAliases.LabelPropertyEditor,false)]
    public class LabelControlPropertyEditor : WinPropertyEditor{

        public LabelControlPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) => ControlBindingProperty = "Text";

        public override bool CanFormatPropertyValue => true;

        protected override object CreateControlCore(){
            var labelControl = new LabelControl{
                BorderStyle = BorderStyles.NoBorder,
                AutoSizeMode = LabelAutoSizeMode.None,
                ShowLineShadow = false
            };
            var deltaAttribute = MemberInfo.FindAttribute<FontSizeDeltaAttribute>();
            if (deltaAttribute != null){
                labelControl.Appearance.FontSizeDelta = MemberInfo.FindAttribute<FontSizeDeltaAttribute>()?.Delta??0;    
            }
            return labelControl;
        }

        protected override void ReadValueCore(){
            base.ReadValueCore();
            Control.Text =DisplayFormat!=String.Empty? string.Format(DisplayFormat,PropertyValue):$"{PropertyValue}";
        }
    }
}