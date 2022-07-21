using System;
using MediatR;

namespace SFA.DAS.ProviderRegistrations.Application.Commands.AddedAccountProviderCommand
{
    public class AddedAccountProviderCommand : IRequest
    {
        public long Ukprn { get; }

        public Guid UserRef { get; }

        public string CorrelationId { get; }

        public DateTime EventDateTime { get; }

        public AddedAccountProviderCommand(long ukprn, Guid userRef, string correlationId, DateTime eventDateTime)
        {
            Ukprn = ukprn;
            UserRef = userRef;
            CorrelationId = correlationId;
            EventDateTime = eventDateTime;
        }
    }
}