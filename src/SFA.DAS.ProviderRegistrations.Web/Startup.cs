using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SFA.DAS.Authorization.Mvc.Extensions;
using SFA.DAS.ProviderRegistrations.Extensions;
using SFA.DAS.ProviderRegistrations.Web.Authentication;
using SFA.DAS.ProviderRegistrations.Web.DependencyResolution;
using SFA.DAS.ProviderRegistrations.Web.Extensions;
using SFA.DAS.ProviderRegistrations.Web.Filters;
using StructureMap;

namespace SFA.DAS.ProviderRegistrations.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .Configure<CookiePolicyOptions>(options =>
                {
                    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                })
                .AddProviderIdamsAuthentication(Configuration)
                .AddDasAuthorization()
                .AddDasDistributedMemoryCache(Configuration, Environment.IsDevelopment())
                .AddMemoryCache()
                .AddDataProtection(Configuration, Environment)
                .AddMvc(options =>
                {
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    options.Filters.Add(new GoogleAnalyticsFilter());
                    ConfigureAuthorization(options);
                })
                .AddNavigationBarSettings(Configuration)
                .AddZenDeskSettings(Configuration)
                .AddGoogleAnalyticsSettings(Configuration)
                .AddCookieBannerSettings(Configuration)
                .AddControllersAsServices()
                .AddSessionStateTempDataProvider()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHealthChecks();
            services.AddApplicationInsightsTelemetry();
        }

        protected virtual void ConfigureAuthorization(MvcOptions options)
        {
            var policy = new AuthorizationPolicyBuilder()
                .RequireProviderInRouteMatchesProviderInClaims()
                .Build();

            options.Filters.Add(new AuthorizeFilter(policy));
            options.AddAuthorization();
        }

        public void ConfigureContainer(Registry registry)
        {
            IoC.Initialize(registry);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/error", "?statuscode={0}")
                .UseUnauthorizedAccessExceptionHandler()
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseCookiePolicy()
                .UseAuthentication()
                .UseMvc()
                .UseHealthChecks("/health");

            var logger = loggerFactory.CreateLogger(nameof(Startup));
            logger.Log(LogLevel.Information, "Application start up configure is complete");
        }
    }
}
