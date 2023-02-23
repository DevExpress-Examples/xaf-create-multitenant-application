using DevExpress.ExpressApp;

namespace SAASExtension.Interfaces {
    public interface IXAFApplicationBuilderWrapper {
        IXAFApplicationBuilderWrapper ConfigureOptions<TOptions>(Action<TOptions> configureOptions) where TOptions : class;
        IXAFApplicationBuilderWrapper AddService<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;
        IXAFApplicationBuilderWrapper AddLogonController<TLogonController>(Action<TLogonController> configure = null) where TLogonController : Controller, new();
        IXAFApplicationBuilderWrapper AddBuildStep(Action<XafApplication> configureApplication);
    }
}
