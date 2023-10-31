using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using OutlookInspired.Blazor.Server.Components;
using OutlookInspired.Blazor.Server.Services;
using OutlookInspired.Blazor.Server.Services.Internal;
using OutlookInspired.Module.Services.Internal;
using EditorAliases = OutlookInspired.Module.Services.EditorAliases;

namespace OutlookInspired.Blazor.Server.Editors {
    [PropertyEditor(typeof(object), EditorAliases.LabelPropertyEditor,false)]
    public class LabelPropertyEditor:ComponentPropertyEditor<LabelModel,LabelModelAdapter>{
        public LabelPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model){
        }

        protected override LabelModelAdapter CreateComponentAdapter(){
            var adapter = base.CreateComponentAdapter();
            adapter.Model.Style=Model.ModelMember.MemberInfo.FontSize();
            return adapter;
        }
    }
    
    public class LabelModelAdapter:ComponentModelAdapter<Label,LabelModel>{
        public override void SetPropertyValue(object value) 
            => Model.Text = value is byte[] bytes ? bytes.ToDocument(server => server.Text) : $"{value}";

        public override object GetPropertyValue() => Model.Text;
    }

}