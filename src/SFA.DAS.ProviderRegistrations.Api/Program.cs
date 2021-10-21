﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NLog.Web;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.ProviderRegistrations.Configuration;
using StructureMap.AspNetCore;

namespace SFA.DAS.ProviderRegistrations.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            logger.Info("Starting up host");

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddAzureTableStorage(options =>
                    {
                        options.ConfigurationKeys = ProviderRegistrationsConfigurationKeys.ProviderRegistrations.Split(',');
                        options.PreFixConfigurationKeys = false;
                    });
                })
                .UseNLog()
                .UseStructureMap()
                .UseStartup<Startup>();
    }
}
