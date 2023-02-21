using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;

namespace SAASExtension.Controllers {
    public class SelectTenantController : ViewController<DetailView> {
        protected const string LogonActionActiveKey = "TenantSelected";
        private LogonController lc;
        public SimpleAction ConfirmSelectedTenantAction { get; private set; }
        public SelectTenantController() {
            string defaltCategory = PredefinedCategory.PopupActions.ToString();
            ConfirmSelectedTenantAction = new SimpleAction(this, "ConfirmSelectedTenant", defaltCategory, (s, e) => {
                if (lc != null) {
                    lc.AcceptAction.Active[LogonActionActiveKey] = true;
                    ConfirmSelectedTenantAction.Active[LogonActionActiveKey] = false;
                    ((IAppearanceVisibility)View.FindItem("UserName")).Visibility = ViewItemVisibility.Show;
                    ((IAppearanceVisibility)View.FindItem("Password")).Visibility = ViewItemVisibility.Show;
                    ((IAppearanceVisibility)View.FindItem("Tenant")).Visibility = ViewItemVisibility.Hide;
                }
            });
            ConfirmSelectedTenantAction.Caption = "Confirm";
        }
        protected override void OnViewChanging(View view) {
            base.OnViewChanging(view);
            Active[ControllerActiveKey] = !SecuritySystem.IsAuthenticated;
        }
        protected override void OnFrameAssigned() {
            base.OnFrameAssigned();
            lc = Frame.GetController<LogonController>();
        }
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            lc = Frame.GetController<LogonController>();
            if (lc != null) {
                lc.AcceptAction.Active[LogonActionActiveKey] = false;
                ((IAppearanceVisibility)View.FindItem("UserName")).Visibility = ViewItemVisibility.Hide;
                ((IAppearanceVisibility)View.FindItem("Password")).Visibility = ViewItemVisibility.Hide;
            }
        }
    }
}
