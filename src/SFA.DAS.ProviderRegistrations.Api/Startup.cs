using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
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