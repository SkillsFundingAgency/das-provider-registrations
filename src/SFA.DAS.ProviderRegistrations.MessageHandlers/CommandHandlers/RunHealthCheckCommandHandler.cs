﻿using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.ProviderRegistrations.Messages.Commands;

namespace SFA.DAS.ProviderRegistrations.MessageHandlers.CommandHandlers;

public class RunHealthCheckCommandHandler : IHandleMessages<RunHealthCheckCommand>
{
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<RunHealthCheckCommandHandler> _logger;

    public RunHealthCheckCommandHandler(IDistributedCache distributedCache, ILogger<RunHealthCheckCommandHandler> logger)
    {
        _distributedCache = distributedCache;
        _logger = logger;
    }

    public async Task Handle(RunHealthCheckCommand message, IMessageHandlerContext context)
    {
        _logger.LogInformation($"Handled {nameof(RunHealthCheckCommand)} with MessageId '{context.MessageId}'");

        await _distributedCache.SetStringAsync(context.MessageId, "OK");
    }
}