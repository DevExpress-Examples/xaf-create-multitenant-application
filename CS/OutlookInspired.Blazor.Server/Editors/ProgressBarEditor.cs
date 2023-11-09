using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.Components;
using EditorAliases = OutlookInspired.Module.Services.EditorAliases;

namespace OutlookInspired.Blazor.Server.Editors{
    [PropertyEditor(typeof(int), EditorAliases.ProgressEditor, false)]
    [PropertyEditor(typeof(double), EditorAliases.ProgressEditor, false)]
    public class ProgressPropertyEditor:ComponentPropertyEditor<ProgressBarModel,ProgressBarModelAdapter>{
        public ProgressPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model){
        }

        protected override RenderFragment CreateViewComponentCore(object dataContext) 
            => CreateEditComponentCore(dataContext);
    }
    public class ProgressBarModelAdapter:ComponentModelAdapter<ProgressBar,ProgressBarModel>{
        public override void SetPropertyValue(object value) 
            => Model.Width = value is double doubleValue ? Convert.ToInt32(doubleValue * 100) : Convert.ToInt32(value ?? 0);
    }
}