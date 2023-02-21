using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Updating;
using SAASExtension.Options;
using SAASExtension.Security;
using System;

namespace SAASExtension.Modules {
    public class ExtensionModule : ModuleBase {
        InternalExtensionModuleOptions internalOptions;
        PublicExtensionModuleOptions publicOptions;
        public ExtensionModule() : base() {
        }
        public ExtensionModule(InternalExtensionModuleOptions internalOptions, PublicExtensionModuleOptions publicOptions) : base() {
            this.internalOptions = internalOptions;
            this.publicOptions = publicOptions;
        }
        public override void CustomizeTypesInfo(ITypesInfo typesInfo) {
            base.CustomizeTypesInfo(typesInfo);
            foreach(Action<ITypesInfo> action in internalOptions.CustomizeTypesInfo) {
                action.Invoke(typesInfo);
            }
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            DatabaseUpdate.Updater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB, internalOptions);
            return new ModuleUpdater[] { updater };
        }
        protected override IEnumerable<Type> GetDeclaredControllerTypes() {
            List<Type> result = new List<Type>();
            foreach (Type type in internalOptions.RunTimeControllers) {
                result.Add(type);
            }
            return result;
        }
        protected override IEnumerable<Type> GetDeclaredExportedTypes() {
            List<Type> result = new List<Type>();
            foreach(Type type in internalOptions.DeclaredExportedTypes) {
                result.Add(type);
            }
            return result;
        }
        public override IList<PopupWindowShowAction> GetStartupActions() {
            List<PopupWindowShowAction> actions = new List<PopupWindowShowAction>(base.GetStartupActions());
            if (internalOptions.AddSelectTenantStartupAction != null) {
                internalOptions.AddSelectTenantStartupAction.Invoke(this, actions);
            }
            return actions;
        }
    }
}
