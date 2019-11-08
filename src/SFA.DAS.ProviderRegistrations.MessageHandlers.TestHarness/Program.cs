using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.NServiceBus.Configuration;
using SFA.DAS.NServiceBus.Configuration.AzureServiceBus;
using SFA.DAS.NServiceBus.Configuration.NewtonsoftJsonSerializer;
using SFA.DAS.ProviderRegistrations.Configuration;

namespace SFA.DAS.ProviderRegistrations.MessageHandlers.TestHarness
{
    internal class Program
    {
        public static async Task Main()
        {
            var builder = new ConfigurationBuilder()
                .AddAzureTableStorage(ProviderRegistrationsConfigurationKeys.ProviderRegistrations);

            IConfigurationRoot configuration = builder.Build();

            var provider = new ServiceCollection()
                .AddOptions()
                .Configure<ProviderRegistrationsSettings>(configuration.GetSection(ProviderRegistrationsConfigurationKeys.ProviderRegistrations)).BuildServiceProvider();

            var config = configuration.GetSection(ProviderRegistrationsConfigurationKeys.NServiceBusSettings).Get<NServiceBusSettings>();
            var isDevelopment = Environment.GetEnvironmentVariable(EnvironmentVariableNames.EnvironmentName) == "LOCAL";

            var endpointConfiguration = new EndpointConfiguration("SFA.DAS.ProviderRegistrations.MessageHandlers.TestHarness")
                .UseInstallers()
                .UseLicense(config.NServiceBusLicense)
                .UseMessageConventions()
                .UseNewtonsoftJsonSerializer();

            if (isDevelopment)
            {
                endpointConfiguration.UseLearningTransport();
            }
            else
            {
                endpointConfiguration.UseAzureServiceBusTransport(config.ServiceBusConnectionString);
            }

            var endpoint = await Endpoint.Start(endpointConfiguration);

            var testHarness = new TestHarness(endpoint);

            await testHarness.Run();
            await endpoint.Stop();
        }
    }
}
