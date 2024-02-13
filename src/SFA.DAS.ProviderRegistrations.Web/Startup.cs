using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NServiceBus.ObjectBuilder.MSDependencyInjection;
using SFA.DAS.Authorization.DependencyResolution.Microsoft;
using SFA.DAS.Authorization.Mvc.Extensions;
using SFA.DAS.NServiceBus.Features.ClientOutbox.Data;
using SFA.DAS.Provider.Shared.UI.Startup;
using SFA.DAS.ProviderRegistrations.Application.Commands.UnsubscribeByIdCommand;
using SFA.DAS.ProviderRegistrations.AppStart;
using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Extensions;
using SFA.DAS.ProviderRegistrations.Startup;
using SFA.DAS.ProviderRegistrations.Web.Authentication;
using SFA.DAS.ProviderRegistrations.Web.Authorization;
using SFA.DAS.ProviderRegistrations.Web.Extensions;
using SFA.DAS.ProviderRegistrations.Web.Mappings;
using SFA.DAS.ProviderRegistrations.Web.ServiceRegistrations;
using SFA.DAS.UnitOfWork.DependencyResolution.Microsoft;
using SFA.DAS.UnitOfWork.EntityFrameworkCore.DependencyResolution.Microsoft;
using SFA.DAS.UnitOfWork.Mvc.Extensions;
using SFA.DAS.UnitOfWork.NServiceBus.Features.ClientOutbox.DependencyResolution.Microsoft;

namespace SFA.DAS.ProviderRegistrations.Web;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration, bool buildConfig = true)
    {
        _configuration = buildConfig ? configuration.BuildDasConfiguration() : configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(builder =>
        {
            builder.AddFilter<ApplicationInsightsLoggerProvider>(string.Empty, LogLevel.Information);
            builder.AddFilter<ApplicationInsightsLoggerProvider>("Microsoft", LogLevel.Information);
        });

        services.AddWebConfigurationSections(_configuration);

        services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            })
            .AddProviderIdamsAuthentication(_configuration);
        
        services.AddDasDistributedMemoryCache(_configuration, _configuration.IsDevOrLocal());
        services.AddMemoryCache();
        services.AddTrainingProviderApi(_configuration);
        services.AddApplicationServices();
        services.AddDataProtection(_configuration);

        services.AddDasMvc(_configuration);

        services.AddAuthorization<AuthorizationContextProvider>();

        var providerRegistrationsSettings = _configuration.GetSection(ProviderRegistrationsConfigurationKeys.ProviderRegistrationsSettings).Get<ProviderRegistrationsSettings>();

        services
            .AddUnitOfWork()
            .AddEntityFramework(providerRegistrationsSettings)
            .AddEntityFrameworkUnitOfWork<ProviderRegistrationsDbContext>()
            .AddNServiceBusClientUnitOfWork();

        services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(UnsubscribeByIdCommandHandler).Assembly));
        services.AddAutoMapper(typeof(InvitationMappings), typeof(ProviderRegistrations.Mappings.InvitationMappings));
        services.AddProviderUiServiceRegistration(_configuration);
        services.AddHealthChecks();
        services.AddApplicationInsightsTelemetry();
        services.AddAuthorizationService();
    }

    public void ConfigureContainer(UpdateableServiceProvider serviceProvider)
    {
        serviceProvider.StartNServiceBus(_configuration);

        // Replacing ClientOutboxPersisterV2 with a local version to fix unit of work issue due to propogating Task up the chain rather than awaiting on DB Command.
        // not clear why this fixes the issue. Attempted to make the change in SFA.DAS.Nservicebus.SqlServer however it conflicts when upgraded with SFA.DAS.UnitOfWork.Nservicebus
        // which would require upgrading to NET6 to resolve.
        var serviceDescriptor = serviceProvider.FirstOrDefault(serv => serv.ServiceType == typeof(IClientOutboxStorageV2));
        serviceProvider.Remove(serviceDescriptor);
        serviceProvider.AddScoped<IClientOutboxStorageV2, ClientOutboxPersisterV2>();
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env, ILoggerFactory loggerFactory)
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
            .UseUnitOfWork()
            .UseStaticFiles()
            .UseCookiePolicy()
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints(builder => builder.MapDefaultControllerRoute())
            .UseHealthChecks("/health");
    }
}