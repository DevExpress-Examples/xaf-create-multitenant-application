using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.Services;
using OutlookInspired.Blazor.Server.Services.Internal;
using ComponentModelBase = OutlookInspired.Blazor.Server.Components.ComponentModelBase;

namespace OutlookInspired.Blazor.Server.Editors {

    public abstract class ComponentPropertyEditor<TModel,TAdapter,TMemberType> : DevExpress.ExpressApp.Blazor.Editors.BlazorPropertyEditorBase where TModel : ComponentModelBase, new()
     where TAdapter:ComponentModelEditorAdapter<TModel,TMemberType>,new(){
        protected ComponentPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) {}
        public new ComponentModelEditorAdapter<TModel,TMemberType> Control => (ComponentModelEditorAdapter<TModel,TMemberType>)base.Control;
        public override bool CanFormatPropertyValue => true;
        protected override  TAdapter CreateComponentAdapter() => new();
    }
    public abstract class ComponentPropertyEditor<TModel,TAdapter> : ComponentPropertyEditor<TModel,TAdapter,object> where TModel : ComponentModelBase, new()
     where TAdapter:ComponentModelEditorAdapter<TModel,object>,new(){
        protected ComponentPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) {}
    }

    public abstract class ComponentModelAdapter<TComponent,TModel, TMemberType> : ComponentModelEditorAdapter<TModel,TMemberType>
        where TModel : ComponentModelBase, new() where TComponent:ComponentBase{
        
        protected override RenderFragment RenderFragment(TModel model) => model.Create<TComponent>();
    }
    public abstract class ComponentModelAdapter<TComponent,TModel> : ComponentModelAdapter<TComponent,TModel, object>
        where TModel : ComponentModelBase, new() where TComponent:ComponentBase{
    }

    public abstract class ComponentModelEditorAdapter<TModel,TMemberType> : ComponentAdapterBase where TModel:ComponentModelBase, new(){
        public override void SetAllowEdit(bool allowEdit){ }
        public override void SetAllowNull(bool allowNull){ }
        public override void SetDisplayFormat(string displayFormat){ }
        public override void SetEditMask(string editMask){ }
        public override void SetEditMaskType(EditMaskType editMaskType){ }
        public override void SetErrorIcon(ImageInfo errorIcon){ }
        public override void SetErrorMessage(string errorMessage){ }
        public override void SetIsPassword(bool isPassword){ }
        public override void SetMaxLength(int maxLength){ }
        public override void SetNullText(string nullText){ }
        public sealed override object GetValue() => GetPropertyValue();
        public sealed override void SetValue(object value) => SetPropertyValue((TMemberType)value);
        public abstract void SetPropertyValue(TMemberType value);
        public virtual TMemberType GetPropertyValue(){throw new NotImplementedException(); }
        public sealed override IComponentModel ComponentModel => Model;
        public virtual TModel Model{ get; } = new();
        protected override RenderFragment CreateComponent() => Model.Create(RenderFragment);
        
        protected abstract RenderFragment RenderFragment(TModel model);
    }
}