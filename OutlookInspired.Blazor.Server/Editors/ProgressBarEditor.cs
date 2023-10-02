using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using OutlookInspired.Blazor.Server.Components;
using EditorAliases = OutlookInspired.Module.Services.Internal.EditorAliases;

namespace OutlookInspired.Blazor.Server.Editors{
    [PropertyEditor(typeof(int), EditorAliases.ProgressEditor, false)]
    [PropertyEditor(typeof(double), EditorAliases.ProgressEditor, false)]
    public class ProgressPropertyEditor:ComponentPropertyEditor<ProgressBarModel,ProgressBarModelAdapter>{
        public ProgressPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model){
        }
    }
    public class ProgressBarModelAdapter:ComponentModelAdapter<ProgressBar,ProgressBarModel>{
        public override void SetPropertyValue(object value) => Model.Width = $"{value ?? 0}";
    }
}