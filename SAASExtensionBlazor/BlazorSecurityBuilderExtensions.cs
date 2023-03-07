using DevExpress.ExpressApp.Blazor.ApplicationBuilder;
using DevExpress.ExpressApp.MiddleTier;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Tests.TestObjects;
using Microsoft.Extensions.DependencyInjection;
using SAASExtension.BusinessObjects;
using System;

namespace SAASExtensionBlazor {
    public static class BlazorSecurityBuilderExtensions {
        private static Action<AuthenticationStandardProviderOptions> SAASConfigureOptions = options => {
            options.LogonParametersType = typeof(CustomLogonParametersForStandardAuthentication);
            options.IsSupportChangePassword = true;
        };
        public static IBlazorSecurityBuilder AddSAASPasswordAuthentication(
                this IBlazorSecurityBuilder builder,
                Action<AuthenticationStandardProviderOptions>? configureOptions = null) {
            builder.Context.Get().Configure(SAASConfigureOptions);

            AuthenticationStandardProviderExtensions.AddPasswordAuthentication(builder.Context.ServerConfiguration.Services, configureOptions);
            builder.RegisteredProviders.Add(SecurityDefaults.PasswordAuthentication);
            return builder;
        }
        public static IBlazorSecurityBuilder AddSAASPasswordAuthentication<TAuthenticationStandardProvider>(
                this IBlazorSecurityBuilder builder,
                Action<AuthenticationStandardProviderOptions>? configureOptions = null)
                where TAuthenticationStandardProvider : class, IAuthenticationProviderV2 {
            builder.Context.Get().Configure(SAASConfigureOptions);
            builder.AddAuthenticationProvider<AuthenticationStandardProviderOptions, TAuthenticationStandardProvider>(configureOptions);
            return builder;
        }
    }
}
