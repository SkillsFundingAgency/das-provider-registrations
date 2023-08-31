using Microsoft.Extensions.Configuration;
using NServiceBus;
using SFA.DAS.NServiceBus.Configuration;
using SFA.DAS.NServiceBus.Configuration.AzureServiceBus;
using SFA.DAS.NServiceBus.Configuration.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.Hosting;
using SFA.DAS.NServiceBus.SqlServer.Configuration;
using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.ProviderRegistrations.Extensions;
using SFA.DAS.UnitOfWork.NServiceBus.Configuration;

namespace SFA.DAS.ProviderRegistrations.Web.ServiceRegistrations;

public static class NServiceBusServiceRegistration
{
    private const string EndpointName = "SFA.DAS.ProviderRegistrations.Web";

    public static IServiceCollection StartNServiceBus(this IServiceCollection services)
    {
        return services
            .AddSingleton(provider =>
            {
                var config = provider.GetService<IConfiguration>();
                var nServiceBusSettings = config.GetSection(ProviderRegistrationsConfigurationKeys.NServiceBusSettings).Get<NServiceBusSettings>();
                var configuration = provider.GetService<ProviderRegistrationsSettings>();
                var hostingEnvironment = provider.GetService<IHostEnvironment>();
                var isDevelopment = hostingEnvironment.IsDevelopment();

                var endpointConfiguration = new EndpointConfiguration(EndpointName)
                    .UseErrorQueue($"{EndpointName}-errors")
                    .UseInstallers()
                    .UseLicense(nServiceBusSettings.NServiceBusLicense)
                    .UseMessageConventions()
                    .UseNewtonsoftJsonSerializer()
                    .UseOutbox(true)                
                    .UseSqlServerPersistence(() => DatabaseExtensions.GetSqlConnection(configuration.DatabaseConnectionString))                
                    .UseUnitOfWork()
                    .UseSendOnly();

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