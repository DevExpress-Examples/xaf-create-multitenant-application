using DevExpress.ExpressApp;
using SAASExtension.BusinessObjects;
using SAASExtension.Security;

namespace SAASExtension.Controllers {
    public class AddAdditionalObjectSpaceController : ObjectViewController<DetailView, SAASPermissionPolicyUser> {
        protected override void OnActivated() {
            base.OnActivated();
            if (!ObjectSpace.IsKnownType(typeof(TenantObject)) && ((CompositeObjectSpace)ObjectSpace).AdditionalObjectSpaces.Count() == 0) {
                IObjectSpace additionalObjectSpace = Application.CreateObjectSpace(typeof(TenantObject));
                ((CompositeObjectSpace)ObjectSpace).AdditionalObjectSpaces.Add(additionalObjectSpace);
                ObjectSpace.Disposed += (s2, e2) => {
                    additionalObjectSpace.Dispose();
                };
            }
        }
    }
}
