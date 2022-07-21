using System;
using MediatR;

namespace SFA.DAS.ProviderRegistrations.Application.Commands.UpsertUserCommand
{
    public class UpsertUserCommand : IRequest
    {
        public string UserRef { get; }

        public string CorrelationId { get; }

        public DateTime EventDateTime { get; }

        public UpsertUserCommand(string userRef, DateTime created, string correlationId)
        {
            UserRef = userRef;
            EventDateTime = created;
            CorrelationId = correlationId;
        }
    }
}