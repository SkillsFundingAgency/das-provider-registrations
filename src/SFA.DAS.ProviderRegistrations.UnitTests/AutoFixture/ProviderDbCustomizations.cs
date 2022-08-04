using AutoFixture;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SFA.DAS.ProviderRegistrations.Data;
using SFA.DAS.ProviderRegistrations.Mappings;
using SFA.DAS.ProviderRegistrations.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ProviderRegistrations.UnitTests.AutoFixture
{
    public class ProviderDbCustomizations : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var perTestDatabaseName = Guid.NewGuid();
            fixture.Register(() => CreateInMemoryProviderDb(perTestDatabaseName));
            fixture.Register(CreateMappings);

            fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            fixture.Customize<Invitation>(i =>
                i.With(i => i.InvitationEvents, new List<InvitationEvent>()));
        }

        private IConfigurationProvider CreateMappings()
        {
            return new MapperConfiguration(c =>
            {
                c.AddProfile(typeof(InvitationMappings));
            });
        }

        private ProviderRegistrationsDbContext CreateInMemoryProviderDb(Guid databaseGuid)
        {
            return new ProviderRegistrationsDbContext(
                    new DbContextOptionsBuilder<ProviderRegistrationsDbContext>().
                    UseInMemoryDatabase(databaseGuid.ToString()).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)).Options);
        }
    }
}
