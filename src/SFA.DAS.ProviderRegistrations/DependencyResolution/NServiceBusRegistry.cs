using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using NServiceBus;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.NServiceBus.Configuration;
using SFA.DAS.NServiceBus.Configuration.AzureServiceBus;
using SFA.DAS.NServiceBus.Configuration.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.Configuration.NLog;
using SFA.DAS.NServiceBus.SqlServer.Configuration;
using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.ProviderRegistrations.Extensions;
using SFA.DAS.UnitOfWork.NServiceBus.Configuration;
using StructureMap;
using System.Data.Common;

namespace SFA.DAS.ProviderRegistrations.DependencyResolution
{
    public class NServiceBusRegistry : Registry
    {
        private const string AzureResource = "https://database.windows.net/";

        public NServiceBusRegistry()
        {
            For<IMessageSession>().Singleton().Use(s => StartNServiceBus(s));
        }

        private IMessageSession StartNServiceBus(IContext context)
        {
            var nServiceBusSettings = context.GetInstance<NServiceBusSettings>();
            var providerSettings = context.GetInstance<ProviderRegistrationsSettings>();
            var environmentService = context.GetInstance<IEnvironmentService>();          

            var endpointName = "SFA.DAS.ProviderRegistrations.Web";
            var endpointConfiguration = new EndpointConfiguration(endpointName)
                .UseErrorQueue($"{endpointName}-errors")
                .UseInstallers()
                .UseLicense(nServiceBusSettings.NServiceBusLicense)
                .UseMessageConventions()
                .UseNLogFactory()
                .UseNewtonsoftJsonSerializer()
                .UseOutbox(true)                
                .UseSqlServerPersistence(() => GetConnectionString(environmentService, providerSettings))                
                .UseUnitOfWork()
                .UseSendOnly();

            if (environmentService.IsCurrent(DasEnv.LOCAL))
            {
                endpointConfiguration.UseLearningTransport(r => r.AddRouting());
            }
            else
            {
                endpointConfiguration.UseAzureServiceBusTransport(nServiceBusSettings.ServiceBusConnectionString, r => r.AddRouting());
            }
            
            var endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();
            return endpoint;
        }

        private DbConnection GetConnectionString(IEnvironmentService environmentService, ProviderRegistrationsSettings providerSettings)
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            return environmentService.IsCurrent(DasEnv.LOCAL)
                ? new SqlConnection(providerSettings.DatabaseConnectionString)
                : new SqlConnection
                {
                    ConnectionString = providerSettings.DatabaseConnectionString,
                    AccessToken = azureServiceTokenProvider.GetAccessTokenAsync(AzureResource).Result
                };
        }
    }
}
