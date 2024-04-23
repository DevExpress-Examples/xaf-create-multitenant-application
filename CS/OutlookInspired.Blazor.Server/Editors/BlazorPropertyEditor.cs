using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Model;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.Services.Internal;
using ComponentBase = Microsoft.AspNetCore.Components.ComponentBase;

namespace OutlookInspired.Blazor.Server.Editors{
    public abstract class BlazorPropertyEditor<TComponent,TModel>:BlazorPropertyEditorBase where TComponent:ComponentBase where TModel:DevExpress.ExpressApp.Blazor.Components.Models.ComponentModelBase, new(){
        protected BlazorPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model){
        }
        
        protected override RenderFragment GetComponentModelContent() 
            => ComponentModel.Create<TComponent>();

        public override TModel ComponentModel 
            => (TModel) base.ComponentModel;
        protected override TModel CreateComponentModel() => new();
        
        RenderFragment CreateComponent(Action<TModel> configure){
            var componentModel = CreateComponentModel();
            configure(componentModel);
            return componentModel.Create( model => model.Create<TComponent>());
        }
        
        protected override RenderFragment CreateViewComponentCore(object dataContext) 
            => CreateComponent(componentModelBase => ConfigureViewComponent(componentModelBase,dataContext));

        protected virtual void ConfigureViewComponent(TModel model, object dataContext){
            
        }
    }
}