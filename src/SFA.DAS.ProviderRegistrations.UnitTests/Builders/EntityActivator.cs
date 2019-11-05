using System;
using SFA.DAS.ProviderRegistrations.Models;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Builders
{
    public static class EntityActivator
    {
        public static T CreateInstance<T>() where T : Entity
        {
            return (T) Activator.CreateInstance(typeof(T), true);
        }
    }
}