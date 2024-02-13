using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.Extensions.Configuration;
using NServiceBus;
using NServiceBus.ObjectBuilder.MSDependencyInjection;
using SFA.DAS.NServiceBus.Configuration;
using SFA.DAS.NServiceBus.Configuration.AzureServiceBus;
using SFA.DAS.NServiceBus.Configuration.MicrosoftDependencyInjection;
using SFA.DAS.NServiceBus.Configuration.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.Hosting;
using SFA.DAS.NServiceBus.SqlServer.Configuration;
using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.ProviderRegistrations.Extensions;
using SFA.DAS.ProviderRegistrations.Web.Extensions;
using SFA.DAS.UnitOfWork.NServiceBus.Configuration;

namespace SFA.DAS.ProviderRegistrations.Web.ServiceRegistrations;

[ExcludeFromCodeCoverage]
public static class NServiceBusServiceRegistration
{
    private const string EndpointName = "SFA.DAS.ProviderRegistrations.Web";
    
    public static IServiceCollection StartNServiceBus(this UpdateableServiceProvider services, IConfiguration configuration)
    {
        var providerRegistrationsSettings = configuration.GetSection(ProviderRegistrationsConfigurationKeys.ProviderRegistrationsSettings).Get<ProviderRegistrationsSettings>();
        var nServiceBusSettings = configuration.GetSection(ProviderRegistrationsConfigurationKeys.NServiceBusSettings).Get<NServiceBusSettings>();
        
        var endpointConfiguration = new EndpointConfiguration(EndpointName)
            .UseErrorQueue($"{EndpointName}-errors")
            .UseInstallers()
            .UseMessageConventions()
            .UseServicesBuilder(services)
            .UseNewtonsoftJsonSerializer()
            .UseOutbox(true)                
            .UseSqlServerPersistence(() => DatabaseExtensions.GetSqlConnection(providerRegistrationsSettings.DatabaseConnectionString))                
            .UseUnitOfWork()
            .UseSendOnly();
        
        if (!string.IsNullOrEmpty(nServiceBusSettings.NServiceBusLicense))
        {
            var decodedLicence = WebUtility.HtmlDecode(nServiceBusSettings.NServiceBusLicense);
            endpointConfiguration.License(decodedLicence);
        }

        if (configuration.IsDevOrLocal())
        {
            endpointConfiguration.UseLearningTransport(r => r.AddRouting());
        }
        else
        {
            endpointConfiguration.UseAzureServiceBusTransport(nServiceBusSettings.ServiceBusConnectionString, r => r.AddRouting());
        }

        var endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

        services.AddSingleton(p => endpoint)
            .AddSingleton<IMessageSession>(p => p.GetService<IEndpointInstance>())
            .AddHostedService<NServiceBusHostedService>();

        return services;
    }
}