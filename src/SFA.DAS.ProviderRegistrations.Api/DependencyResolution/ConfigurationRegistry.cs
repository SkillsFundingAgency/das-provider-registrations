using Microsoft.Extensions.Configuration;
using SFA.DAS.ProviderRegistrations.Configuration;
using StructureMap;

namespace SFA.DAS.ProviderRegistrations.Api.DependencyResolution
{
    public class ConfigurationRegistry : Registry
    {
        public ConfigurationRegistry()
        {
            AddConfiguration<ActiveDirectorySettings>(ProviderRegistrationsConfigurationKeys.ActiveDirectorySettings);
            AddConfiguration<ProviderRegistrationsSettings>(ProviderRegistrationsConfigurationKeys.ProviderRegistrations);
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