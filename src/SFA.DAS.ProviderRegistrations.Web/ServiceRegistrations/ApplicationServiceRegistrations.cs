﻿using SFA.DAS.NServiceBus.Services;
using SFA.DAS.ProviderRegistrations.Services;

namespace SFA.DAS.ProviderRegistrations.Web.ServiceRegistrations;

public static class ApplicationServiceRegistrations
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IProviderService, ProviderService>();
        services.AddTransient<IEmployerUsersApiHttpClientFactory, EmployerApprenticeshipApiHttpClientFactory>();
        services.AddTransient<IRoatpApiHttpClientFactory, RoatpApiHttpClientFactory>();
        services.AddTransient<IEmployerApprenticeshipService, EmployerApprenticeshipService>();
        services.AddTransient<ILinkGenerator, LinkGenerator>();
        services.AddTransient<IDateTimeService, DateTimeService>();

        return services;
    }
}