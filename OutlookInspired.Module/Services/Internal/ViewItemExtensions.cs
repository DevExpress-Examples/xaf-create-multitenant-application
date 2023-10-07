using DevExpress.ExpressApp.Editors;

namespace OutlookInspired.Module.Services.Internal{
    static class ViewItemExtensions{
        public static object DisplayableMemberValue(this PropertyEditor editor,object currentObject=null,object propertyValue=null) {
            currentObject ??= editor.CurrentObject;
            propertyValue ??= editor.PropertyValue;
            var defaultMember = editor.MemberInfo.FindDisplayableMember();
            return defaultMember != null ? defaultMember.GetValue(currentObject) : propertyValue;
        }
    }
}