using NServiceBus;
using SFA.DAS.ProviderRegistrations.Messages.Commands;

namespace SFA.DAS.ProviderRegistrations.MessageHandlers.Extensions;

public static class RoutingSettingsExtensions
{
    public static void AddRouting(this RoutingSettings routingSettings)
    {
        routingSettings.RouteToEndpoint(typeof(RunHealthCheckCommand), "SFA.DAS.ProviderRegistrations.MessageHandlers");
    }
}