using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using NServiceBus;
using SFA.DAS.NServiceBus.Configuration;
using SFA.DAS.NServiceBus.Configuration.AzureServiceBus;
using SFA.DAS.NServiceBus.Configuration.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.SqlServer.Configuration;
using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.ProviderRegistrations.Extensions;
using SFA.DAS.UnitOfWork.NServiceBus.Configuration;

namespace SFA.DAS.ProviderRegistrations.Web.ServiceRegistrations;

[ExcludeFromCodeCoverage]
public static class NServiceBusServiceRegistration
{
    public static IServiceCollection StartNServiceBus(this IServiceCollection services, IConfiguration configuration)
    {
        const string endPointName = "SFA.DAS.ProviderRegistrations.Web";

        return services.AddSingleton<IMessageSession>(provider =>
        {
            var providerRegistrationsSettings = configuration.GetSection(ProviderRegistrationsConfigurationKeys.ProviderRegistrationsSettings).Get<ProviderRegistrationsSettings>();
            var nServiceBusSettings = configuration.GetSection(ProviderRegistrationsConfigurationKeys.NServiceBusSettings).Get<NServiceBusSettings>();
            var hostingEnvironment = provider.GetService<IHostEnvironment>();
            var isDevelopment = hostingEnvironment.IsDevelopment();

            var endpointConfiguration = new EndpointConfiguration(endPointName)
                .UseErrorQueue($"{endPointName}-errors")
                .UseInstallers()
                .UseLicense(nServiceBusSettings.NServiceBusLicense)
                .UseMessageConventions()
                .UseNewtonsoftJsonSerializer()
                .UseOutbox(true)
                .UseSqlServerPersistence(() => DatabaseExtensions.GetSqlConnection(providerRegistrationsSettings.DatabaseConnectionString))
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
        });
    }
}