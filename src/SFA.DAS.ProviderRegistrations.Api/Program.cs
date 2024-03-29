﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SFA.DAS.NServiceBus.Configuration.MicrosoftDependencyInjection;

namespace SFA.DAS.ProviderRegistrations.Api;

public class Program
{
    public static void Main(string[] args)
    {
        CreateWebHostBuilder(args)
            .Build()
            .Run();
    }

    private static IHostBuilder CreateWebHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseNServiceBusContainer()
            .ConfigureWebHostDefaults(builder =>
            {
                builder.UseStartup<Startup>();
            });

}