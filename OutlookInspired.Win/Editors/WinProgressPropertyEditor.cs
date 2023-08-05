using DevExpress.Accessibility;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using EditorAliases = OutlookInspired.Module.Services.EditorAliases;

namespace OutlookInspired.Win.Editors {
    [PropertyEditor(typeof(int), EditorAliases.ProgressEditor, false)]
    public class WinProgressPropertyEditor : DXPropertyEditor {
        public WinProgressPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) { }
        protected override object CreateControlCore() => new ProgressBarControl();
        protected override RepositoryItem CreateRepositoryItem() => new RepositoryItemProgressBarControl();

        protected override void SetupRepositoryItem(RepositoryItem item) {
            var repositoryItem = (RepositoryItemProgressBarControl)item;
            repositoryItem.Maximum = 100;
            repositoryItem.Minimum = 0;
            base.SetupRepositoryItem(item);
        }
    }
    public class ProgressBarControl : DevExpress.XtraEditors.ProgressBarControl {
        static ProgressBarControl() => RepositoryItemProgressBarControl.Register();
        public override string EditorTypeName => RepositoryItemProgressBarControl.EditorName;
        protected override object ConvertCheckValue(object val) => val;
    }
    public class RepositoryItemProgressBarControl : RepositoryItemProgressBar {
        protected internal const string EditorName = "TaskProgressBarControl";
        protected internal static void Register() {
            if (!EditorRegistrationInfo.Default.Editors.Contains(EditorName)) {
                EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(EditorName, typeof(ProgressBarControl),
                    typeof(RepositoryItemProgressBarControl), typeof(ProgressBarViewInfo),
                    new ProgressBarPainter(), true, EditImageIndexes.ProgressBarControl, typeof(ProgressBarAccessible)));
            }
        }
        static RepositoryItemProgressBarControl() => Register();
        public RepositoryItemProgressBarControl() {
            Maximum = 100;
            Minimum = 0;
            ShowTitle = true;
            Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        }
        protected override int ConvertValue(object val) {
            try {
                return (int)(Minimum + Convert.ToUInt32(val));
            }
            catch{
                // ignored
            }
            return Minimum;
        }
        public override string EditorTypeName => EditorName;
    }
}
