using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SFA.DAS.Http;
using SFA.DAS.Http.Configuration;
using SFA.DAS.Http.TokenGenerators;
using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.ProviderRegistrations.Extensions;
using SFA.DAS.ProviderRegistrations.Services;

namespace SFA.DAS.ProviderRegistrations.Web.ServiceRegistrations;

public static class TrainingProviderApiClientRegistration
{
    public static IServiceCollection AddTrainingProviderApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TrainingProviderApiClientConfiguration>(c => configuration.GetSection("TrainingProviderApiClient").Bind(c));
        services.AddSingleton(cfg => cfg.GetService<IOptions<TrainingProviderApiClientConfiguration>>().Value);

        services.AddSingleton<ITrainingProviderApiClient>(s =>
        {
            var trainingProviderApiClientConfiguration = s.GetService<TrainingProviderApiClientConfiguration>();
            var httpClient = GetHttpClient(trainingProviderApiClientConfiguration, configuration);
            return new TrainingProviderApiClient(httpClient, trainingProviderApiClientConfiguration);
        });
        return services;
    }

    private static HttpClient GetHttpClient(IManagedIdentityClientConfiguration apiClientConfiguration, IConfiguration config)
    {
        var httpClientBuilder = !config.IsDevOrLocal()
            ? new HttpClientBuilder()
            : new HttpClientBuilder().WithBearerAuthorisationHeader(new ManagedIdentityTokenGenerator(apiClientConfiguration));
        return httpClientBuilder.Build();
    }
}