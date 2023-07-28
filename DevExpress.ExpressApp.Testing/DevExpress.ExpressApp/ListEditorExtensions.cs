using System.Reactive.Linq;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Testing.RXExtensions;

namespace DevExpress.ExpressApp.Testing.DevExpress.ExpressApp{
    public static class ListEditorExtensions{
        public static IObservable<TListEditor> WhenControlsCreated<TListEditor>(this TListEditor listEditor) where TListEditor:ListEditor 
            => listEditor.WhenEvent(nameof(listEditor.ControlsCreated)).StartWith(listEditor.Control).WhenNotDefault().To(listEditor);
    }
}