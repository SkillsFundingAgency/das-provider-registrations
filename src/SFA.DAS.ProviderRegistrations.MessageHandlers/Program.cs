using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Logging;
using SFA.DAS.ProviderRegistrations.Application.Commands.AddedPayeSchemeCommand;
using SFA.DAS.ProviderRegistrations.MessageHandlers.DependencyResolution;
using SFA.DAS.ProviderRegistrations.MessageHandlers.Extensions;
using StructureMap;

namespace SFA.DAS.ProviderRegistrations.MessageHandlers
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var hostBuilder = new HostBuilder();

            try
            {
                hostBuilder
                    .UseDasEnvironment()
                    .ConfigureDasAppConfiguration(args)
                    .ConfigureLogging(b => b.AddNLog())
                    .UseConsoleLifetime()
                    .UseStructureMap()
                    .ConfigureServices((c, s) => s
                        .AddMemoryCache()
                        .AddMediatR(x=> x.RegisterServicesFromAssembly(typeof(AddedPayeSchemeCommand).Assembly))
                        .AddNServiceBus(c.Configuration, c.HostingEnvironment.IsDevelopment()))
                    .ConfigureContainer<Registry>(IoC.Initialize);

                using (var host = hostBuilder.Build())
                {
                    await host.RunAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
