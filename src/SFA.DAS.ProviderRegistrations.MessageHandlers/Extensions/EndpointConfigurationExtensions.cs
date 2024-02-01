using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Azure.ServiceBus.Primitives;
using NServiceBus;
using SFA.DAS.NServiceBus.Configuration;
using SFA.DAS.NServiceBus.Configuration.AzureServiceBus;

namespace SFA.DAS.ProviderRegistrations.MessageHandlers.Extensions;

[ExcludeFromCodeCoverage]
public static class EndpointConfigurationExtensions
{
    public static EndpointConfiguration ConfigureServiceBusTransport(this EndpointConfiguration config, Func<string> connectionStringBuilder, bool isLocal)
    {
        if (isLocal)
        {
            config.UseLearningTransport();
        }
        else
        {
            //config.UseAzureServiceBusTransport(connectionStringBuilder(), s => s.AddRouting());
            var transport = config.UseTransport<AzureServiceBusTransport>();
            var ruleNameShortener = new RuleNameShortener();
            
            var tokenProvider = TokenProvider.CreateManagedIdentityTokenProvider();
            transport.CustomTokenProvider(tokenProvider);
            transport.ConnectionString(connectionStringBuilder());
            transport.RuleNameShortener(ruleNameShortener.Shorten);
            transport.Routing().AddRouting();
        }

        config.UseMessageConventions();

        return config;
    }
}