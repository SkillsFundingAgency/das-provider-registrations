using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using SFA.DAS.ProviderRegistrations.Configuration;

namespace SFA.DAS.ProviderRegistrations.Api.Extensions
{
    public static class SecurityServicesCollectionExtension
    {
        public static void AddAdAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var activeDirectorySettings = configuration.GetSection(ProviderRegistrationsConfigurationKeys.ActiveDirectorySettings).Get<ActiveDirectorySettings>();

            services.AddAuthorization(o =>
            {
                o.AddPolicy("default", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Default");
                });
            });

            services.AddAuthentication(auth =>
            {
                auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(auth =>
            {
                auth.Authority = $"https://login.microsoftonline.com/{activeDirectorySettings.Tenant}";
                auth.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidAudiences = activeDirectorySettings.IdentifierUri.Split(",")
                };
            });
        }
    }
}
