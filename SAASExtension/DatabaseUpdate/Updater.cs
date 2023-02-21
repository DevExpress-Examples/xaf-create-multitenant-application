using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using SAASExtension.BusinessObjects;
using SAASExtension.Security;
using System.Security.Permissions;
using SAASExtension.Options;

namespace SAASExtension.DatabaseUpdate;
public class Updater : ModuleUpdater {
    InternalExtensionModuleOptions internalOptions;
    public Updater(IObjectSpace objectSpace, Version currentDBVersion, InternalExtensionModuleOptions internalOptions) :
        base(objectSpace, currentDBVersion) {
        this.internalOptions = internalOptions;
    }
    public override void UpdateDatabaseAfterUpdateSchema() {
        base.UpdateDatabaseAfterUpdateSchema();
        if (ObjectSpace.IsKnownType(typeof(PermissionPolicyRole))) {
            UpdateDefaultRoles();
        }
        ObjectSpace.CommitChanges();
    }
    private void UpdateDefaultRoles() {
        foreach (PermissionPolicyRole defaultRole in ObjectSpace.CreateCollection(typeof(PermissionPolicyRole), CriteriaOperator.Parse("[IsAdministrative] == 'false'"))) {
            foreach(var action in internalOptions.CustomizeDefaultRole) {
                action.Invoke(defaultRole);
            }
        }
    }
}
