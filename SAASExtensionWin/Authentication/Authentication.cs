using DevExpress.ExpressApp.Security;

namespace SAASExtensionWin.Authentication {
    internal class DummyIdentity : System.Security.Principal.IIdentity {
        public string AuthenticationType => throw new NotImplementedException();
        public bool IsAuthenticated => false;
        public string Name => throw new NotImplementedException();
    }
    internal class DummyPrincipal : System.Security.Principal.IPrincipal {
        DummyIdentity identity = new DummyIdentity();
        public System.Security.Principal.IIdentity Identity => identity;
        public bool IsInRole(string role) => throw new NotImplementedException();
    }
    internal class DummyPrincipalProvider : IPrincipalProvider {
        DummyPrincipal principal = new DummyPrincipal();
        public System.Security.Principal.IPrincipal User => principal;
    }
}
