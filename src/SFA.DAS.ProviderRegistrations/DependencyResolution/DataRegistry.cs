﻿using System.Data.Common;
using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.ProviderRegistrations.Data;
using StructureMap;
using System;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using SFA.DAS.Configuration;

namespace SFA.DAS.ProviderRegistrations.DependencyResolution
{
    public class DataRegistry : Registry
    {
        private const string AzureResource = "https://database.windows.net/";
        
        public DataRegistry()
        {
            var environmentName = Environment.GetEnvironmentVariable(EnvironmentVariableNames.EnvironmentName);
            For<DbConnection>().Use($"Build DbConnection", c => {
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                return environmentName.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase)
                    ? new SqlConnection(GetConnectionString(c))
                    : new SqlConnection
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