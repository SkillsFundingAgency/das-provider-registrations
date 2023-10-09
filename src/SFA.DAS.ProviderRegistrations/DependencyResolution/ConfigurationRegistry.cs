using Microsoft.Extensions.Configuration;
using SFA.DAS.AutoConfiguration.DependencyResolution;
using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.Provider.Shared.UI.Models;
using StructureMap;
using ProviderSharedUIConfiguration = SFA.DAS.Provider.Shared.UI.Models.ProviderSharedUIConfiguration;

namespace SFA.DAS.ProviderRegistrations.DependencyResolution
{
    public class ConfigurationRegistry : Registry
    {
        public ConfigurationRegistry()
        {
            IncludeRegistry<AutoConfigurationRegistry>();
            AddConfiguration<AuthenticationSettings>(ProviderRegistrationsConfigurationKeys.AuthenticationSettings);
            AddConfiguration<ActiveDirectorySettings>(ProviderRegistrationsConfigurationKeys.ActiveDirectorySettings);
            AddConfiguration<ProviderRegistrationsSettings>(ProviderRegistrationsConfigurationKeys.ProviderRegistrationsSettings);
            AddConfiguration<NServiceBusSettings>(ProviderRegistrationsConfigurationKeys.NServiceBusSettings);
            AddConfiguration<EmployerApprenticeshipApiClientSettings>(ProviderRegistrationsConfigurationKeys.EmployerApprenticeshipApiClientSettings);
            AddConfiguration<ZenDeskConfiguration>(ProviderRegistrationsConfigurationKeys.ZenDeskSettings);
            AddConfiguration<RoatpApiClientSettings>(ProviderRegistrationsConfigurationKeys.RoatpApiClientSettings);
            AddConfiguration<ProviderSharedUIConfiguration>(ProviderRegistrationsConfigurationKeys.ProviderSharedUIConfigurationSettings);
            AddConfiguration<TrainingProviderApiClientConfiguration>(ProviderRegistrationsConfigurationKeys.TrainingProviderApiClientSettings);
        }

        private void AddConfiguration<T>(string key) where T : class
        {
            For<T>().Use(c => GetConfiguration<T>(c, key)).Singleton();
        }

        private T GetConfiguration<T>(IContext context, string name)
        {
            var configuration = context.GetInstance<IConfiguration>();
            var section = configuration.GetSection(name);
            var value = section.Get<T>();

            return value;
        }
    }
}