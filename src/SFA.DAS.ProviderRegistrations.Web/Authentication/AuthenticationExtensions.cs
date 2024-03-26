using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SFA.DAS.DfESignIn.Auth.AppStart;
using SFA.DAS.ProviderRegistrations.Configuration;

namespace SFA.DAS.ProviderRegistrations.Web.Authentication;

public static class AuthenticationExtensions
{
    private const string CookieAuthName = "SFA.DAS.ProviderApprenticeshipService";

    public static IServiceCollection AddProviderIdamsAuthentication(this IServiceCollection services, IConfiguration config)
    {
        if (config["UseAuthenticationStub"] != null && bool.Parse(config["UseAuthenticationStub"]))
        {
            services.AddProviderStubAuthentication();
        }
        else
        {
            // if the DfESignIn feature toggle is turned on then use DfESignIn OpenIdConnect as the authentication. 
            var useDfeSignIn = config.GetSection(ProviderRegistrationsConfigurationKeys.UseDfESignIn).Get<bool>();
            if (useDfeSignIn)
            {
                services.AddAndConfigureDfESignInAuthentication(
                    config,
                    CookieAuthName,
                    typeof(CustomServiceRole),
                    DfESignIn.Auth.Enums.ClientName.ProviderRoatp,
                    "/signout");
            }
            else
            {
                var authenticationSettings = config.GetSection(ProviderRegistrationsConfigurationKeys.AuthenticationSettings).Get<AuthenticationSettings>();
                services.AddIdamsAuthentication(authenticationSettings);
            }
        }

        return services;
    }

    private static void AddIdamsAuthentication(this IServiceCollection services, AuthenticationSettings authenticationSettings)
    {
        services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = WsFederationDefaults.AuthenticationScheme;
            })
            .AddWsFederation(options =>
            {
                options.MetadataAddress = authenticationSettings.MetadataAddress;
                options.Wtrealm = authenticationSettings.Wtrealm;
                options.Events.OnSecurityTokenValidated = OnSecurityTokenValidated;
            }).AddCookie(options =>
            {
                options.CookieManager = new ChunkingCookieManager { ChunkSize = 3000 };
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.ReturnUrlParameter = "/Home/Index";
                options.AccessDeniedPath = "/Error/403";
            });
    }

    public static IServiceCollection AddProviderStubAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication("Provider-stub").AddScheme<AuthenticationSchemeOptions, ProviderStubAuthHandler>(
            "Provider-stub",
            options => { }).AddCookie(options =>
        {
            options.AccessDeniedPath = "/Error/403";
            options.CookieManager = new ChunkingCookieManager { ChunkSize = 3000 };
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.ReturnUrlParameter = "/Home/Index";
        });
        return services;
    }

    private static Task OnSecurityTokenValidated(SecurityTokenValidatedContext context)
    {
        var claims = context.Principal.Claims;
        var ukprn = claims.FirstOrDefault(claim => claim.Type == (ProviderClaims.Ukprn))?.Value;

        return Task.CompletedTask;
    }

    public class ProviderStubAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProviderStubAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IHttpContextAccessor httpContextAccessor) : base(options, logger, encoder, clock)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[]
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, "10005310"),
                new Claim(ProviderClaims.DisplayName, "Test-U-Good Corporation"),
                new Claim(ProviderClaims.Service, "DAA"),
                new Claim(ProviderClaims.Ukprn, "10005310"),
                new Claim(ProviderClaims.Upn, "10005310"),
                new Claim(ProviderClaims.Email, "test+10005310@test.com"),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "Provider")
            };
            var identity = new ClaimsIdentity(claims, "Provider-stub");

            var principal = new ClaimsPrincipal(identity);

            var ticket = new AuthenticationTicket(principal, "Provider-stub");

            var result = AuthenticateResult.Success(ticket);

            _httpContextAccessor.HttpContext.Items.Add(ClaimsIdentity.DefaultNameClaimType, "10005310");
            _httpContextAccessor.HttpContext.Items.Add(ClaimsIdentity.DefaultRoleClaimType, "Provider");
            _httpContextAccessor.HttpContext.Items.Add(ProviderClaims.DisplayName, "Test-U-Good Corporation");

            return Task.FromResult(result);
        }
    }
}