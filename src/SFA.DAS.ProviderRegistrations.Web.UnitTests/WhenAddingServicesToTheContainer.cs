using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Commands.AddInvitationCommand;
using SFA.DAS.ProviderRegistrations.Application.Commands.AddResendInvitationCommand;
using SFA.DAS.ProviderRegistrations.Application.Commands.SendInvitationEmailCommand;
using SFA.DAS.ProviderRegistrations.Application.Commands.UpdateInvitationCommand;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetEmailAddressInUseQuery;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationByIdQuery;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationEventByIdQuery;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationQuery;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetProviderByUkprnQuery;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetUnsubscribedQuery;
using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.ProviderRegistrations.Web.Authorization;
using SFA.DAS.ProviderRegistrations.Web.Controllers;

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
   // [TestCase(typeof(IRequestHandler<SendInvitationEmailCommand>))]
    [TestCase(typeof(IRequestHandler<AddInvitationCommand, string>))]
    public void Then_The_Dependencies_Are_Correctly_Resolved_For_Handlers(Type toResolve)
    {
        var provider = SetupServiceProvider();
        var type = provider.GetService(toResolve);

        Assert.That(type, Is.Not.Null);
    } 
    
    [TestCase(typeof(HomeController))]
    [TestCase(typeof(RegistrationController))]
    [TestCase(typeof(ITrainingProviderAuthorizationHandler))]
    public void Then_The_Dependencies_Are_Correctly_Resolved_For_Services(Type toResolve)
    {
        var provider = SetupServiceProvider();
        var type = provider.GetService(toResolve);

        Assert.That(type, Is.Not.Null);
    }
    
    private static ServiceProvider SetupServiceProvider()
    {
        var stubConfiguration = GenerateStubConfiguration();
        var startup = new Startup(stubConfiguration, false);
        var services = new ServiceCollection();
        startup.ConfigureServices(services);

        services.AddSingleton<IConfiguration>(stubConfiguration);
        services.AddTransient<HomeController>();
        services.AddTransient<RegistrationController>();
        services.AddTransient(_ =>Mock.Of<IMessageSession>());
        
        return services.BuildServiceProvider();
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