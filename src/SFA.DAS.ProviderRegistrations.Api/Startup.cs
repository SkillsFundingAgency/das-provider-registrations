using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SFA.DAS.ProviderRegistrations.Api.DependencyResolution;
using SFA.DAS.ProviderRegistrations.Api.Extensions;
using SFA.DAS.ProviderRegistrations.Api.ServiceRegistrations;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationByIdQuery;
using SFA.DAS.ProviderRegistrations.Extensions;
using SFA.DAS.ProviderRegistrations.Mappings;
using SFA.DAS.ProviderRegistrations.ServiceRegistrations;
using StructureMap;

namespace SFA.DAS.ProviderRegistrations.Api;

public class Startup
{
    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAdAuthentication(_configuration);
        services.AddMvc(options =>
        {
            if (!_environment.IsDevelopment())
            {
                options.Filters.Add(new AuthorizeFilter("default"));
            }
        });

        services.AddApiConfigurationSections(_configuration);
        services.AddMediatR(x=> x.RegisterServicesFromAssembly(typeof(GetInvitationByIdQuery).Assembly));
        services.AddDasDistributedMemoryCache(_configuration, _environment.IsDevelopment());
        services.AddDatabaseRegistration();
        services.AddMemoryCache();
        services.AddHealthChecks();
        services.AddAutoMapper(typeof(InvitationMappings));
        services.AddApplicationInsightsTelemetry();
        
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

    public void ConfigureContainer(Registry registry)
    {
        IoC.Initialize(registry);
    }
}