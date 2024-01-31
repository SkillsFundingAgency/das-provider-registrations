using System;
using System.Collections.Generic;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Application.Commands.AddedAccountProviderCommand;
using SFA.DAS.ProviderRegistrations.Application.Commands.AddedPayeSchemeCommand;
using SFA.DAS.ProviderRegistrations.Application.Commands.SignedAgreementCommand;
using SFA.DAS.ProviderRegistrations.Application.Commands.UpsertUserCommand;
using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.ProviderRegistrations.ServiceRegistrations;
using SFA.DAS.ProviderRegistrations.Startup;
using SFA.DAS.UnitOfWork.DependencyResolution.Microsoft;
using SFA.DAS.UnitOfWork.NServiceBus.DependencyResolution.Microsoft;

namespace SFA.DAS.ProviderRegistrations.MessageHandlers.UnitTests;

public class WhenAddingServicesToTheContainer
{
    [TestCase(typeof(IRequestHandler<AddedPayeSchemeCommand>))]
    [TestCase(typeof(IRequestHandler<SignedAgreementCommand>))]
    [TestCase(typeof(IRequestHandler<UpsertUserCommand>))]
    [TestCase(typeof(IRequestHandler<AddedAccountProviderCommand>))]
    public void Then_The_Dependencies_Are_Correctly_Resolved_For_Handlers(Type toResolve)
    {
        var services = new ServiceCollection();
        SetupServiceCollection(services);
        var provider = services.BuildServiceProvider();

        var type = provider.GetService(toResolve);
        type.Should().NotBeNull();
    }
    
    private static void SetupServiceCollection(IServiceCollection services)
    {
        var configuration = GenerateStubConfiguration();
        var providerRegistrationsConfig = configuration
            .GetSection(ProviderRegistrationsConfigurationKeys.ProviderRegistrationsSettings)
            .Get<ProviderRegistrationsSettings>();

        services.AddLogging();
        services.AddEntityFramework(providerRegistrationsConfig);
        services.AddUnitOfWork();
        services.AddNServiceBusUnitOfWork();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddSingleton(providerRegistrationsConfig);
        services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(AddedPayeSchemeCommand).Assembly));
    }

    private static IConfigurationRoot GenerateStubConfiguration()
    {
        var configSource = new MemoryConfigurationSource
        {
            InitialData = new List<KeyValuePair<string, string>>
            {
                new($"{ProviderRegistrationsConfigurationKeys.ProviderRegistrationsSettings}:DatabaseConnectionString", "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true"),
            }
        };

        var provider = new MemoryConfigurationProvider(configSource);

        return new ConfigurationRoot(new List<IConfigurationProvider> { provider });
    }
}