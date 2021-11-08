using System.Data.Common;
using System.Data.SqlClient;
using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.ProviderRegistrations.Data;
using StructureMap;
using System;
using Microsoft.Azure.Services.AppAuthentication;
using SFA.DAS.AutoConfiguration;

namespace SFA.DAS.ProviderRegistrations.DependencyResolution
{
    public class DataRegistry : Registry
    {
        private const string AzureResource = "https://database.windows.net/";
        
        public DataRegistry()
        {
            For<DbConnection>().Use($"Build DbConnection", c => {
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                return new SqlConnection
                    {
                        ConnectionString = GetConnectionString(c),
                        AccessToken = azureServiceTokenProvider.GetAccessTokenAsync(AzureResource).Result
                    };
            });

            For<ProviderRegistrationsDbContext>().Use(c => c.GetInstance<IProviderRegistrationsDbContextFactory>().CreateDbContext());
        }

        private string GetConnectionString(IContext context)
        {
            return context.GetInstance<ProviderRegistrationsSettings>().DatabaseConnectionString;            
        }
    }
}