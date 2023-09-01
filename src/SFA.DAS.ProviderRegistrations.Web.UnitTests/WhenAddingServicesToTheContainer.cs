using System;
using System.Collections.Generic;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.Authorization.DependencyResolution;
using SFA.DAS.ProviderRegistrations.Application.Commands.AddedPayeSchemeCommand;
using SFA.DAS.ProviderRegistrations.Application.Commands.AddInvitationCommand;
using SFA.DAS.ProviderRegistrations.Application.Commands.AddResendInvitationCommand;
using SFA.DAS.ProviderRegistrations.Application.Commands.SendInvitationEmailCommand;
using SFA.DAS.ProviderRegistrations.Application.Commands.SignedAgreementCommand;
using SFA.DAS.ProviderRegistrations.Application.Commands.UpdateInvitationCommand;
using SFA.DAS.ProviderRegistrations.Application.Commands.UpsertUserCommand;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetEmailAddressInUseQuery;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationByIdQuery;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationEventByIdQuery;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationQuery;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetProviderByUkprnQuery;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetUnsubscribedQuery;
using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.ProviderRegistrations.ServiceRegistrations;
using SFA.DAS.ProviderRegistrations.Web.Authorization;
using SFA.DAS.ProviderRegistrations.Web.Controllers;
using SFA.DAS.ProviderRegistrations.Web.Mappings;
using IConfigurationProvider = Microsoft.Extensions.Configuration.IConfigurationProvider;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace SFA.DAS.ProviderRegistrations.Web.UnitTests;

public class WhenAddingServicesToTheContainer
{
    [TestCase(typeof(IRequestHandler<GetInvitationQuery, GetInvitationQueryResult>))]
    [TestCase(typeof(IRequestHandler<GetProviderByUkprnQuery, GetProviderByUkprnQueryResult>))]
    [TestCase(typeof(IRequestHandler<GetInvitationEventByIdQuery, GetInvitationEventByIdQueryResult>))]
    [TestCase(typeof(IRequestHandler<GetInvitationByIdQuery, GetInvitationByIdQueryResult>))]
    [TestCase(typeof(IRequestHandler<GetEmailAddressInUseQuery, bool>))]
    [TestCase(typeof(IRequestHandler<GetUnsubscribedQuery, bool>))]
    [TestCase(typeof(IRequestHandler<GetProviderByUkprnQuery, GetProviderByUkprnQueryResult>))]
    [TestCase(typeof(IRequestHandler<AddResendInvitationCommand>))]
    [TestCase(typeof(IRequestHandler<UpdateInvitationCommand>))]
    [TestCase(typeof(IRequestHandler<SendInvitationEmailCommand>))]
    [TestCase(typeof(IRequestHandler<AddInvitationCommand, string>))]
    public void Then_The_Dependencies_Are_Correctly_Resolved_For_Handlers(Type toResolve)
    {
        var provider = SetupServiceProvider();
        var type = provider.GetService(toResolve);
 
        Assert.IsNotNull(type);
    } 
    
    [TestCase(typeof(RegistrationController))]
    public void Then_The_Dependencies_Are_Correctly_Resolved_For_Controllers(Type toResolve)
    {
        var provider = SetupServiceProvider();
        var type = provider.GetService(toResolve);
       
        Assert.IsNotNull(type);
    }

    private static ServiceProvider SetupServiceProvider()
    {
        var mockHostingEnvironment = new Mock<IHostingEnvironment>();
        mockHostingEnvironment.Setup(x => x.EnvironmentName).Returns(Environments.Development);

        var startup = new Startup(GenerateStubConfiguration(), false);
        var serviceCollection = new ServiceCollection();
        startup.ConfigureServices(serviceCollection);

        serviceCollection.AddSingleton(_ => mockHostingEnvironment.Object);
        serviceCollection.AddTransient<RegistrationController>();
        serviceCollection.AddTransient(_ =>Mock.Of<IMessageSession>());
        
        return serviceCollection.BuildServiceProvider();
    }
    
    private static IConfigurationRoot GenerateStubConfiguration()
    {
        var configSource = new MemoryConfigurationSource
        {
            InitialData = new List<KeyValuePair<string, string>>
            {
                new("EnvironmentName","LOCAL"),
                new("DatabaseConnectionString", "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true"),
                new("RedisConnectionSettings:RedisConnectionString","Test"),
                new("ProviderSharedUIConfiguration:DashboardUrl","https://test.com"),
                new("RoatpApiClientSettings:ApiBaseUrl","https://test.com"),
                new("EmployerApprenticeshipApiClientSettings:ApiBaseUrl","https://test.com"),
            }
        };

        var provider = new MemoryConfigurationProvider(configSource);

        return new ConfigurationRoot(new List<IConfigurationProvider> { provider });
    }
}