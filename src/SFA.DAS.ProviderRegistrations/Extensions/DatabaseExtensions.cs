using System;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Azure.Core;
using Azure.Identity;

namespace SFA.DAS.ProviderRegistrations.Extensions;

public static class DatabaseExtensions
{
    private const string AzureResource = "https://database.windows.net/";

    public static DbConnection GetSqlConnection(string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString));
        }

        var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
        var useManagedIdentity = !connectionStringBuilder.IntegratedSecurity && string.IsNullOrEmpty(connectionStringBuilder.UserID);
        
        if (!useManagedIdentity)
        {
            return new SqlConnection(connectionString);
        }

        var azureServiceTokenProvider = new ChainedTokenCredential(
            new ManagedIdentityCredential(),
            new AzureCliCredential(),
            new VisualStudioCodeCredential(),
            new VisualStudioCredential());

        return new SqlConnection
        {
            ConnectionString = connectionString,
            AccessToken = azureServiceTokenProvider.GetToken(new TokenRequestContext(scopes: new[] { AzureResource })).Token
        };
    }
}