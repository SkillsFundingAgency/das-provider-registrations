using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.ProviderRegistrations.Configuration;
using StructureMap.AspNetCore;

namespace SFA.DAS.ProviderRegistrations.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(c => c.AddAzureTableStorage(ProviderRegistrationsConfigurationKeys.ProviderRegistrations))
                .UseStructureMap()
                .UseStartup<Startup>();
    }
}
