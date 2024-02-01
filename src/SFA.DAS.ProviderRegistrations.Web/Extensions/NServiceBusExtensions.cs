using NServiceBus;
using SFA.DAS.Notifications.Messages.Commands;

namespace SFA.DAS.ProviderRegistrations.Web.Extensions;

public static class NServiceBusExtensions
{
    private const string NotificationsMessageHandler = "SFA.DAS.Notifications.MessageHandlers";
    public static void AddRouting(this RoutingSettings routingSettings)
    {
        routingSettings.RouteToEndpoint(typeof(SendEmailCommand), NotificationsMessageHandler);
        routingSettings.RouteToEndpoint(typeof(SendSmsCommand), NotificationsMessageHandler);
    }
}