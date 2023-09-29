using System.Net.Http;

namespace SFA.DAS.ProviderRegistrations.Services
{
    /// <summary>
    /// Interface defines the contracts for the interactions with RoATPv2/APAR API Services.
    /// </summary>
    public interface ITrainingProviderApiClientFactory
    {
        /// <summary>
        /// Method is part of ITrainingProviderApiClientFactory Interface.
        /// </summary>
        /// Contract responsible creating the HttpClient.
        /// <returns>The value of <paramref name='HttpClient' /></returns>
        HttpClient CreateHttpClient();
    }
}
