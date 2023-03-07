using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using SAASExtension.BusinessObjects;
using SAASExtension.Interfaces;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using DevExpress.Data.Filtering;

namespace SAASExtension.Controllers {
    public class SelectTenantAfterLogInController : ObjectViewController<DetailView, CustomLogonParametersForStandardAuthentication> {
        protected const string LogonActionActiveKey = "TenantSelected";
        LogonController lc;

        private bool IsAdministrator(string name) {
            PermissionPolicyUser user = ObjectSpace.FindObject<PermissionPolicyUser>(CriteriaOperator.Parse($"[UserName] = '{name}'"));
            return (user != null) && (user.Roles.FirstOrDefault(r => r.IsAdministrative) != null);
        }
        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e) {
            CustomLogonParametersForStandardAuthentication obj = (CustomLogonParametersForStandardAuthentication)e.Object;
            if (e.PropertyName == "UserName") {
                obj.Tenant = null;
                View.FindItem("Tenant").Refresh();
            }
            if (e.PropertyName == "Tenant") {
                lc.AcceptAction.Enabled[LogonActionActiveKey] = IsAdministrator(ViewCurrentObject.UserName) || (ViewCurrentObject?.TenantName != null);
            }
        }
        protected override void OnViewChanging(View view) {
            base.OnViewChanging(view);
            Active[ControllerActiveKey] = !SecuritySystem.IsAuthenticated;
        }
        protected override void OnActivated() {
            base.OnActivated();
            lc = Frame.GetController<LogonController>();
            lc.AcceptAction.Enabled[LogonActionActiveKey] = IsAdministrator(ViewCurrentObject.UserName) || (ViewCurrentObject?.TenantName != null);
            ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
        }
        protected override void OnDeactivated() {
            ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
            base.OnDeactivated();
        }
    }
    public class SelectTenantRunTimeController : WindowController {
        SingleChoiceAction selectTenant;
        private void SelectTenant_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
            ReloadTenant(e.SelectedChoiceActionItem.Id);
        }
        protected virtual void ReloadTenant(string tenantName) {
            var logonParameters = ((ILogonParameterProvider)Application.ServiceProvider.GetService(typeof(ILogonParameterProvider))).GetLogonParameters<ITenantName>();
            logonParameters.TenantName = tenantName;
            Application.ChangeModel(true);
        }
        protected override void OnActivated() {
            base.OnActivated();
            ITenantNamesHelper tenantNamesHelper = Application.ServiceProvider.GetService(typeof(ITenantNamesHelper)) as ITenantNamesHelper;
            foreach (var name in tenantNamesHelper.GetTenantNamesMap().Keys) {
                selectTenant.Items.Add(new ChoiceActionItem(name, name));
            }
        }
        public SelectTenantRunTimeController() {
            this.TargetWindowType = WindowType.Main;
            selectTenant = new SingleChoiceAction(this, "ChangeTenant", "Edit");
            selectTenant.Execute += SelectTenant_Execute;
        }
    }
}
