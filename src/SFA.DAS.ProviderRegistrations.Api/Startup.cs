using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using NServiceBus.ObjectBuilder.MSDependencyInjection;
using SFA.DAS.NServiceBus.Features.ClientOutbox.Data;
using SFA.DAS.ProviderRegistrations.Api.Extensions;
using SFA.DAS.ProviderRegistrations.Api.ServiceRegistrations;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationByIdQuery;
using SFA.DAS.ProviderRegistrations.Extensions;
using SFA.DAS.ProviderRegistrations.Mappings;
using SFA.DAS.ProviderRegistrations.ServiceRegistrations;

namespace SFA.DAS.ProviderRegistrations.Api;

public class Startup
{
    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _environment = environment;
        _configuration = configuration.BuildDasConfiguration();
    }

    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(builder =>
        {
            builder.AddFilter<ApplicationInsightsLoggerProvider>(string.Empty, LogLevel.Information);
            builder.AddFilter<ApplicationInsightsLoggerProvider>("Microsoft", LogLevel.Information);
        });
        
        services.AddAdAuthentication(_configuration);
        services.AddMvc(options =>
        {
            if (!_environment.IsDevelopment())
            {
                options.Filters.Add(new AuthorizeFilter("default"));
            }
        });

        services.AddApiConfigurationSections(_configuration);
        services.AddMediatR(configuration=> configuration.RegisterServicesFromAssembly(typeof(GetInvitationByIdQuery).Assembly));
        services.AddDasDistributedMemoryCache(_configuration, _configuration.IsDevOrLocal());
        services.AddDatabaseRegistration();
        services.AddMemoryCache();
        services.AddHealthChecks();
        services.AddAutoMapper(typeof(InvitationMappings));
        services.AddApplicationInsightsTelemetry();
    }
    
    public void ConfigureContainer(UpdateableServiceProvider serviceProvider)
    {
        serviceProvider.StartNServiceBus(_configuration, ServiceBusEndpointType.Api);

        // Replacing ClientOutboxPersisterV2 with a local version to fix unit of work issue due to propogating Task up the chain rather than awaiting on DB Command.
        // not clear why this fixes the issue. Attempted to make the change in SFA.DAS.Nservicebus.SqlServer however it conflicts when upgraded with SFA.DAS.UnitOfWork.Nservicebus
        // which would require upgrading to NET6 to resolve.
        var serviceDescriptor = serviceProvider.FirstOrDefault(serv => serv.ServiceType == typeof(IClientOutboxStorageV2));
        serviceProvider.Remove(serviceDescriptor);
        serviceProvider.AddScoped<IClientOutboxStorageV2, ClientOutboxPersisterV2>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
            app.UseAuthentication();
        }

        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseEndpoints(builder =>
        {
            builder.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
        
        app.UseHealthChecks("/health");
    }
}