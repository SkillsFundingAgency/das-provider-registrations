using System;
using System.Collections.Generic;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Api.ServiceRegistrations;
using SFA.DAS.ProviderRegistrations.Application.Commands.UnsubscribeByIdCommand;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationByIdQuery;
using SFA.DAS.ProviderRegistrations.Mappings;
using SFA.DAS.ProviderRegistrations.ServiceRegistrations;
using IConfigurationProvider = Microsoft.Extensions.Configuration.IConfigurationProvider;

namespace SFA.DAS.ProviderRegistrations.Api.UnitTests;

public class WhenAddingServicesToTheContainer
{
    [TestCase(typeof(IRequestHandler<GetInvitationByIdQuery, GetInvitationByIdQueryResult>))]
    [TestCase(typeof(IRequestHandler<UnsubscribeByIdCommand>))]
    public void Then_The_Dependencies_Are_Correctly_Resolved_For_Handlers(Type toResolve)
    {
        var mockHostingEnvironment = new Mock<IWebHostEnvironment>();
        mockHostingEnvironment.Setup(x => x.EnvironmentName).Returns("Test");

        var config = GenerateConfiguration();
        var serviceCollection = new ServiceCollection();
        
        serviceCollection.AddApiConfigurationSections(config);
        serviceCollection.AddMediatR(x=> x.RegisterServicesFromAssembly(typeof(GetInvitationByIdQuery).Assembly));
        serviceCollection.AddDatabaseRegistration();
        serviceCollection.AddAutoMapper(typeof(InvitationMappings));
        
        var provider = serviceCollection.BuildServiceProvider();
        var type = provider.GetService(toResolve);

        Assert.That(type, Is.Not.Null);
    }
    
    private static IConfigurationRoot GenerateConfiguration()
    {
        var configSource = new MemoryConfigurationSource
        {
            InitialData = new List<KeyValuePair<string, string>>
            {
                 new("DatabaseConnectionString", "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;"),
            }
        };

        var provider = new MemoryConfigurationProvider(configSource);

        return new ConfigurationRoot(new List<IConfigurationProvider> { provider });
    }
}