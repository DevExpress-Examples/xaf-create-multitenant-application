using System.Text.Json;
using Aqua.EnumerableExtensions;

namespace OutlookInspired.Blazor.Server.Components.DevExtreme{
    public abstract class DevExtremeModel<TComponent>:ComponentModelBase<TComponent> where TComponent:Microsoft.AspNetCore.Components.ComponentBase{
        public override void ShowMessage(JsonElement element){
            if (!element.EnumerateArray().Select(e => e.GetString()).StringJoin(", ").Contains("js.devexpress.com")) return;
            base.ShowMessage(element);
        }
    }
}