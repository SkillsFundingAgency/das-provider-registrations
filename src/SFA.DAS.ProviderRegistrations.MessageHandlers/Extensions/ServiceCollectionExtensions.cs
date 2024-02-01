﻿using System.Reflection;
using Microsoft.Azure.ServiceBus.Primitives;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
using SFA.DAS.UnitOfWork.NServiceBus.Configuration;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SFA.DAS.ProviderRegistrations.MessageHandlers.Extensions;

public static class ServiceCollectionExtensions
{
    private const string EndpointName = "SFA.DAS.ProviderRegistrations.MessageHandlers";
    
    public static IServiceCollection StartNServiceBus(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddSingleton(provider =>
            {
                var logger = provider.GetService<ILogger<Program>>();
                
                var hostingEnvironment = provider.GetService<IHostEnvironment>();
                var isDevelopment = hostingEnvironment.IsDevelopment();
                
                var nServiceBusSettings = configuration
                    .GetSection(ProviderRegistrationsConfigurationKeys.NServiceBusSettings)
                    .Get<NServiceBusSettings>();
                
                var providerRegistrationsConfig = configuration
                    .GetSection(ProviderRegistrationsConfigurationKeys.ProviderRegistrationsSettings)
                    .Get<ProviderRegistrationsSettings>();
                
                logger.LogInformation("ProviderRegistrationsSettings config value: {Value}", JsonSerializer.Serialize(providerRegistrationsConfig));

                var endpointConfiguration = new EndpointConfiguration(EndpointName)
                    .UseErrorQueue($"{EndpointName}-errors")
                    .UseInstallers()
                    .UseLicense(nServiceBusSettings.NServiceBusLicense)
                    .UseMessageConventions()
                    .UseNewtonsoftJsonSerializer()
                    .UseOutbox()
                    .UseSqlServerPersistence(() => DatabaseExtensions.GetSqlConnection(providerRegistrationsConfig.DatabaseConnectionString))
                    .UseServicesBuilder(new UpdateableServiceProvider(services))
                    .UseUnitOfWork();

                if (isDevelopment)
                {
                    endpointConfiguration.UseLearningTransport(s => s.AddRouting());
                }
                else
                {
                    var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
                    var ruleNameShortener = new RuleNameShortener();

                    var tokenProvider = TokenProvider.CreateManagedIdentityTokenProvider();
                    transport.CustomTokenProvider(tokenProvider);
                    transport.ConnectionString(nServiceBusSettings.ServiceBusConnectionString);
                    transport.RuleNameShortener(ruleNameShortener.Shorten);
                    transport.Routing().AddRouting();
                }

                return Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();
                
            })
            .AddSingleton<IMessageSession>(s => s.GetService<IEndpointInstance>())
            .AddHostedService<NServiceBusHostedService>();
    }
}