using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraPdfViewer;
using EditorAliases = OutlookInspired.Module.Services.EditorAliases;

namespace OutlookInspired.Win.Editors{
    [PropertyEditor(typeof(byte[]), EditorAliases.PdfViewerEditor, false)]  
    public class PdfViewerEditor : WinPropertyEditor {
        public PdfViewerEditor(Type objectType, IModelMemberViewItem info)  
            : base(objectType, info) {  
        }

        public new PdfViewer Control => (PdfViewer)base.Control;
        protected override object CreateControlCore() 
            => new PdfViewer{ Dock = DockStyle.Fill, DetachStreamAfterLoadComplete = true, ZoomMode = PdfZoomMode.FitToVisible };
        
        [DllImport("USER32.dll", CharSet = CharSet.Auto)]  
        static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

        [SecuritySafeCritical]
        public void LockRedrew(Action action){
            SendMessage(Control.Handle, 0x000B, 0, IntPtr.Zero);
            action();
            SendMessage(Control.Handle, 0x000B, 1, IntPtr.Zero);
        }
        
        protected override void ReadValueCore(){
            if (PropertyValue != null){
                LockRedrew(() => {
                    using var memoryStream = new MemoryStream((byte[])PropertyValue);
                    Control.LoadDocument(memoryStream);
                    Control.NavigationPaneVisibility=PdfNavigationPaneVisibility.Hidden;
                });
            }
        }
    }
}