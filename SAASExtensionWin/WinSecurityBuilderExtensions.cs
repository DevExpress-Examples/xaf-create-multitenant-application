using DevExpress.ExpressApp.MiddleTier;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win.ApplicationBuilder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAASExtension.BusinessObjects;

namespace SAASExtensionWin {
    public static class WinSecurityBuilderExtensions {
        private static Action<AuthenticationStandardProviderOptions> SAASConfigureOptions = options => {
            options.LogonParametersType = typeof(CustomLogonParametersForStandardAuthentication);
        };
        public static IWinAuthenticationBuilder AddSAASPasswordAuthentication(
                this IWinAuthenticationBuilder builder,
                Action<AuthenticationStandardProviderOptions>? configureOptions = null) {
            ((IWinSecurityBuilder)builder).Context.Get().Configure(SAASConfigureOptions);
            builder.UsePasswordAuthentication(configureOptions);
            return builder;
        }
        public static IWinAuthenticationBuilder AddSAASPasswordAuthentication<TAuthenticationStandardProvider>(
        this IWinAuthenticationBuilder builder,
        Action<AuthenticationStandardProviderOptions>? configureOptions = null)
        where TAuthenticationStandardProvider : class, IAuthenticationProviderV2 {
            IServiceCollection serviceCollection = ((IWinSecurityBuilder)builder).Context.Get();
            serviceCollection.Configure(SAASConfigureOptions);
            serviceCollection.Configure(configureOptions);
            serviceCollection.TryAddEnumerable(ServiceDescriptor.Scoped(typeof(IAuthenticationProviderV2), typeof(TAuthenticationStandardProvider)));
            serviceCollection.AddScoped<IPrincipalProvider, SAASExtensionWin.Authentication.DummyPrincipalProvider>();
            return builder;
        }
    }
}
