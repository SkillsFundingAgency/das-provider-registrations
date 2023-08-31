using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SFA.DAS.NServiceBus.Configuration.MicrosoftDependencyInjection;
using SFA.DAS.ProviderRegistrations.MessageHandlers.Extensions;

namespace SFA.DAS.ProviderRegistrations.MessageHandlers;

public class Program
{
    public static async Task Main(string[] args)
    {
        using var host = CreateHost(args);
        
        await host.RunAsync();
    }

    private static IHost CreateHost(string[] args)
    {
        return new HostBuilder()
            .ConfigureDasAppConfiguration(args)
            .UseDasEnvironment()
            .UseConsoleLifetime()
            .ConfigureDasLogging()
            .ConfigureDasServices()
            .UseNServiceBusContainer()
            .Build();
    }
}