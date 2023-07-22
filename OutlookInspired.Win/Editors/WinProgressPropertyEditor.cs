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
        protected override object CreateControlCore() => new TaskProgressBarControl();
        protected override RepositoryItem CreateRepositoryItem() => new RepositoryItemTaskProgressBarControl();

        protected override void SetupRepositoryItem(RepositoryItem item) {
            var repositoryItem = (RepositoryItemTaskProgressBarControl)item;
            repositoryItem.Maximum = 100;
            repositoryItem.Minimum = 0;
            base.SetupRepositoryItem(item);
        }
    }
    public class TaskProgressBarControl : ProgressBarControl {
        static TaskProgressBarControl() => RepositoryItemTaskProgressBarControl.Register();
        public override string EditorTypeName => RepositoryItemTaskProgressBarControl.EditorName;
        protected override object ConvertCheckValue(object val) => val;
    }
    public class RepositoryItemTaskProgressBarControl : RepositoryItemProgressBar {
        protected internal const string EditorName = "TaskProgressBarControl";
        protected internal static void Register() {
            if (!EditorRegistrationInfo.Default.Editors.Contains(EditorName)) {
                EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(EditorName, typeof(TaskProgressBarControl),
                    typeof(RepositoryItemTaskProgressBarControl), typeof(ProgressBarViewInfo),
                    new ProgressBarPainter(), true, EditImageIndexes.ProgressBarControl, typeof(ProgressBarAccessible)));
            }
        }
        static RepositoryItemTaskProgressBarControl() => Register();
        public RepositoryItemTaskProgressBarControl() {
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
