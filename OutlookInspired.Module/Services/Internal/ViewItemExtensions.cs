using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Templates;

namespace OutlookInspired.Module.Services.Internal{
    static class ViewItemExtensions{
        public static object DisplayableMemberValue(this PropertyEditor editor,object currentObject=null,object propertyValue=null) {
            currentObject ??= editor.CurrentObject;
            propertyValue ??= editor.PropertyValue;
            var defaultMember = editor.MemberInfo.FindDisplayableMember();
            return defaultMember != null ? defaultMember.GetValue(currentObject) : propertyValue;
        }
        public static T HideToolBar<T>(this T frameContainer) where T:IFrameContainer{
            ((ISupportActionsToolbarVisibility)frameContainer.Frame.Template).SetVisible(false);
            return frameContainer;
        }
    }
}