using AutoFixture;
using SFA.DAS.ProviderRegistrations.Data;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using AutoFixture.NUnit3;
using AutoMapper;
using SFA.DAS.ProviderRegistrations.Mappings;
using AutoFixture.AutoMoq;

namespace SFA.DAS.ProviderRegistrations.UnitTests
{
    public class ProviderAutoDataAttribute : AutoDataAttribute
    {
        public ProviderAutoDataAttribute() : base(() => new Fixture().Customize(new DomainCustomizations()))
        {
        }
    }

    public class DomainCustomizations : CompositeCustomization
    {
        public DomainCustomizations() : base(
            new AutoMoqCustomization { ConfigureMembers = true },
            new ProviderDbCustomizations())
        {
        }
    }

    public class ProviderDbCustomizations : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register(CreateInMemoryProviderDb);
            fixture.Register(CreateMappings);
        }

        private IConfigurationProvider CreateMappings()
        {
            return new MapperConfiguration(c =>
            {
                c.AddProfile(typeof(InvitationMappings));
                c.AddProfile(typeof(InvitationMappings));
            });
        }

        private ProviderRegistrationsDbContext CreateInMemoryProviderDb()
        {
            return new ProviderRegistrationsDbContext(
                    new DbContextOptionsBuilder<ProviderRegistrationsDbContext>().
                    UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)).Options);
        }
    }
}
