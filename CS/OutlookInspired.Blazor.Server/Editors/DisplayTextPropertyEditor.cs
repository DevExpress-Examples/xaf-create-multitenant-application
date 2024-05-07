using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using OutlookInspired.Blazor.Server.Components;
using OutlookInspired.Blazor.Server.Services.Internal;
using OutlookInspired.Module.Services.Internal;
using EditorAliases = OutlookInspired.Module.Services.EditorAliases;

namespace OutlookInspired.Blazor.Server.Editors {
    [PropertyEditor(typeof(object), EditorAliases.LabelPropertyEditor,false)]
    public class LabelPropertyEditor:BlazorPropertyEditor<Label,LabelModel>{
        public LabelPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model){
        }

        protected override void ReadValueCore(){
            base.ReadValueCore();
            ComponentModel.Text = PropertyValue is byte[] bytes ? bytes.ToDocumentText() : $"{PropertyValue}";
        }

        protected override object GetControlValueCore() 
            => ComponentModel.Text;

        protected override LabelModel CreateComponentModel(){
            var labelModel = base.CreateComponentModel();
            labelModel.Style = MemberInfo.FontSize();
            return labelModel;
        }
    }
}