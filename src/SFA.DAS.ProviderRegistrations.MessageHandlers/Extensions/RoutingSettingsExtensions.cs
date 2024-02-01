using NServiceBus;
using SFA.DAS.ProviderRegistrations.Messages.Commands;

namespace SFA.DAS.ProviderRegistrations.MessageHandlers.Extensions;

public static class RoutingSettingsExtensions
{
    public static void AddRouting(this RoutingSettings routing)
    {
        routing.RouteToEndpoint(
            typeof(RunHealthCheckCommand).Assembly, 
            typeof(RunHealthCheckCommand).Namespace, 
            "SFA.DAS.ProviderRegistrations.MessageHandlers");
    }
}