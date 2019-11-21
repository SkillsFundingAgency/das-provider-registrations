using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Moq;
using NServiceBus.Testing;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.MessageHandlers.CommandHandlers;
using SFA.DAS.ProviderRegistrations.MessageHandlers.UnitTests.AutoFixture;
using SFA.DAS.ProviderRegistrations.Messages.Commands;

namespace SFA.DAS.ProviderRegistrations.MessageHandlers.UnitTests.CommandHandlers
{
    [TestFixture]
    [Parallelizable]
    public class RunHealthCheckCommandHandlerTests
    {
        [Test, DomainAutoData]
        public async Task Handle_WhenHandlingCommand_ThenShouldLogInformation(
            [Frozen] Mock<ILogger<RunHealthCheckCommandHandler>> logger,
            RunHealthCheckCommandHandler handler,
            RunHealthCheckCommand message,
            TestableMessageHandlerContext context)
        {
            //arrange

            //act
            await handler.Handle(message, context);

            //assert
            logger.Verify(l => l.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<FormattedLogValues>(v => v.ToString().Equals($"Handled {nameof(RunHealthCheckCommand)} with MessageId '{context.MessageId}'")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()));
        }

        [Test, DomainAutoData]
        public async Task Handle_WhenHandlingCommand_ThenShouldAddMessageIdToDistributedCache(
            [Frozen] Mock<IDistributedCache> distributedCache,
            RunHealthCheckCommandHandler handler,
            RunHealthCheckCommand message,
            TestableMessageHandlerContext context)
        {
            //arrange
            var stringValue = System.Text.Encoding.UTF8.GetBytes("OK");

            //act
            await handler.Handle(message, context);

            //assert
            distributedCache.Verify(c => c.SetAsync(context.MessageId, stringValue, It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Once, "Cache Method not Called");
        }
    }
}