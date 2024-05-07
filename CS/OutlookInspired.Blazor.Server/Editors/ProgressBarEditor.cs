using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.Components;
using EditorAliases = OutlookInspired.Module.Services.EditorAliases;

namespace OutlookInspired.Blazor.Server.Editors{
    [PropertyEditor(typeof(int), EditorAliases.ProgressEditor, false)]
    [PropertyEditor(typeof(double), EditorAliases.ProgressEditor, false)]
    public class ProgressPropertyEditor:BlazorPropertyEditor<ProgressBar,ProgressBarModel>{
        public ProgressPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model){
        }

        protected override void ReadValueCore(){
            base.ReadValueCore();
            ComponentModel.Width = Convert.ToInt32(PropertyValue is double doubleValue ? doubleValue * 100 : PropertyValue ?? 0);
        }

        protected override object GetControlValueCore() => ComponentModel.Width;

        protected override RenderFragment CreateViewComponentCore(object dataContext) 
            => CreateEditComponentCore(dataContext);
    }
}