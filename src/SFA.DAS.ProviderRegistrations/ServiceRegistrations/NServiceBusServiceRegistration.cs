using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using SFA.DAS.NServiceBus.Configuration;
using SFA.DAS.NServiceBus.Configuration.AzureServiceBus;
using SFA.DAS.NServiceBus.Configuration.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.Hosting;
using SFA.DAS.NServiceBus.SqlServer.Configuration;
using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.ProviderRegistrations.Extensions;
using SFA.DAS.UnitOfWork.NServiceBus.Configuration;

namespace SFA.DAS.ProviderRegistrations.ServiceRegistrations;

public enum ServiceBusEndpointType
{
    Api,
    Web
}

[ExcludeFromCodeCoverage]
public static class NServiceBusServiceRegistration
{
    public static IServiceCollection StartNServiceBus(this IServiceCollection services, IConfiguration configuration, ServiceBusEndpointType endpointType)
    {
        var endPointName = $"SFA.DAS.ProviderRegistrations.{endpointType}";
        
        return services
            .AddSingleton(provider =>
            {
                var providerRegistrationsSettings = configuration.GetSection(ProviderRegistrationsConfigurationKeys.ProviderRegistrationsSettings).Get<ProviderRegistrationsSettings>();
                var nServiceBusSettings = configuration.GetSection(ProviderRegistrationsConfigurationKeys.NServiceBusSettings).Get<NServiceBusSettings>();
                var hostingEnvironment = provider.GetService<IHostEnvironment>();
                var isDevelopment = hostingEnvironment.IsDevelopment();
                
                var allowOutboxCleanup = endpointType == ServiceBusEndpointType.Api;

                var endpointConfiguration = new EndpointConfiguration(endPointName)
                    .UseErrorQueue($"{endPointName}-errors")
                    .UseInstallers()
                    .UseLicense(nServiceBusSettings.NServiceBusLicense)
                    .UseMessageConventions()
                    .UseNewtonsoftJsonSerializer()
                    .UseOutbox(allowOutboxCleanup)                
                    .UseSqlServerPersistence(() => DatabaseExtensions.GetSqlConnection(providerRegistrationsSettings.DatabaseConnectionString))                
                    .UseUnitOfWork();

                if (isDevelopment)
                {
                    endpointConfiguration.UseLearningTransport(r => r.AddRouting());
                }
                else
                {
                    endpointConfiguration.UseAzureServiceBusTransport(nServiceBusSettings.ServiceBusConnectionString, r => r.AddRouting());
                }
            
                return Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();
            })
            .AddSingleton<IMessageSession>(s => s.GetService<IEndpointInstance>())
            .AddHostedService<NServiceBusHostedService>();
    }
}