using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Templates;

namespace OutlookInspired.Module.Services.Internal{
    static class ViewItemExtensions{
        public static T HideToolBar<T>(this T frameContainer) where T:IFrameContainer{
            ((ISupportActionsToolbarVisibility)frameContainer.Frame.Template).SetVisible(false);
            return frameContainer;
        }
    }
}