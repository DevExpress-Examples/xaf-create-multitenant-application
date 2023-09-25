using System.Reactive.Linq;
using DevExpress.ExpressApp.Editors;
using XAF.Testing.RX;

namespace XAF.Testing.XAF{
    public static class ListEditorExtensions{
        public static IObservable<TListEditor> WhenControlsCreated<TListEditor>(this TListEditor listEditor) where TListEditor:ListEditor 
            => listEditor.WhenEvent(nameof(listEditor.ControlsCreated)).StartWith(listEditor.Control).WhenNotDefault().To(listEditor);
    }
}