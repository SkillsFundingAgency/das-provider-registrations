using SFA.DAS.ProviderRegistrations.Services;
using StructureMap;
using System.Net.Http;

namespace SFA.DAS.ProviderRegistrations.DependencyResolution
{
    public class ServiceRegistry : Registry
    {
        public ServiceRegistry()
        {
            For<HttpClient>().Singleton().Use(x => new HttpClient());
            For<IProviderService>().Use<ProviderService>();
            For<IEmployerUsersApiHttpClientFactory>().Use<EmployerApprenticeshipApiHttpClientFactory>();
            For<IRoatpApiHttpClientFactory>().Use<RoatpApiHttpClientFactory>();
            For<ITrainingProviderApiClient>().Use<TrainingProviderApiClient>();
            For<ITrainingProviderApiClientFactory>().Use<TrainingProviderApiClientFactory>();
            For<IEmployerApprenticeshipService>().Use<EmployerApprenticeshipService>();
        }
    }
}
