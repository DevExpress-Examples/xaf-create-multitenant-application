using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAASExample1.Module.Controllers {
    public class SelectCompanyController : ViewController<DetailView> {
        protected const string LogonActionActiveKey = "CompanySelected";
        LogonController lc;
        public SimpleAction ConfirmSelectedCompanyAction { get; private set; }
        public SelectCompanyController() {
            string defaltCategory = PredefinedCategory.PopupActions.ToString();
            ConfirmSelectedCompanyAction = new SimpleAction(this, "ConfirmSelectedCompany", defaltCategory, (s, e) => {
                if (lc != null) {
                    lc.AcceptAction.Active[LogonActionActiveKey] = true;
                    ConfirmSelectedCompanyAction.Active[LogonActionActiveKey] = false;
                    ((IAppearanceVisibility)View.FindItem("UserName")).Visibility = ViewItemVisibility.Show;
                    ((IAppearanceVisibility)View.FindItem("Password")).Visibility = ViewItemVisibility.Show;
                    ((IAppearanceVisibility)View.FindItem("CompanyName")).Visibility = ViewItemVisibility.Hide;
                }

            }) { Caption = "Confirm" };
        }
        protected override void OnViewChanging(View view) {
            base.OnViewChanging(view);
            Active[ControllerActiveKey] = !SecuritySystem.IsAuthenticated;
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
