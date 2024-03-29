﻿using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.ObjectBuilder.MSDependencyInjection;
using SFA.DAS.NServiceBus.Configuration;
using SFA.DAS.NServiceBus.Configuration.MicrosoftDependencyInjection;
using SFA.DAS.NServiceBus.Configuration.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.Hosting;
using SFA.DAS.NServiceBus.SqlServer.Configuration;
using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.ProviderRegistrations.Extensions;
using SFA.DAS.UnitOfWork.NServiceBus.Configuration;

namespace SFA.DAS.ProviderRegistrations.MessageHandlers.Extensions;

public static class ServiceCollectionExtensions
{
    private const string EndpointName = "SFA.DAS.ProviderRegistrations.MessageHandlers";

    public static IServiceCollection AddNServiceBus(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddSingleton(provider =>
            {
                var hostingEnvironment = provider.GetService<IHostEnvironment>();
                var isDevelopment = hostingEnvironment.IsDevelopment();

                var nServiceBusSettings = configuration
                    .GetSection(ProviderRegistrationsConfigurationKeys.NServiceBusSettings)
                    .Get<NServiceBusSettings>();

                var providerRegistrationsConfig = configuration
                    .GetSection(ProviderRegistrationsConfigurationKeys.ProviderRegistrationsSettings)
                    .Get<ProviderRegistrationsSettings>();

                var endpointConfiguration = new EndpointConfiguration(EndpointName)
                    .ConfigureServiceBusTransport(() => nServiceBusSettings.ServiceBusConnectionString, isDevelopment)
                    .UseErrorQueue($"{EndpointName}-errors")
                    .UseInstallers()
                    .UseSqlServerPersistence(() => DatabaseExtensions.GetSqlConnection(providerRegistrationsConfig.DatabaseConnectionString))
                    .UseNewtonsoftJsonSerializer()
                    .UseOutbox()
                    .UseUnitOfWork()
                    .UseServicesBuilder(new UpdateableServiceProvider(services));

                if (!string.IsNullOrEmpty(nServiceBusSettings.NServiceBusLicense))
                {
                    var decodedLicence = WebUtility.HtmlDecode(nServiceBusSettings.NServiceBusLicense);
                    endpointConfiguration.UseLicense(decodedLicence);
                }
                
                return Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();
            })
            .AddSingleton<IMessageSession>(s => s.GetService<IEndpointInstance>())
            .AddHostedService<NServiceBusHostedService>();
    }
}