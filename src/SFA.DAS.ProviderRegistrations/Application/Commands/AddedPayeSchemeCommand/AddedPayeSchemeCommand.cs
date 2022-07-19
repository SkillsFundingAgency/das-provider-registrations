using System;
using MediatR;

namespace SFA.DAS.ProviderRegistrations.Application.Commands.AddedPayeSchemeCommand
{
    public class AddedPayeSchemeCommand : IRequest
    {
        public long AccountId { get; }

        public string UserName { get; }

        public Guid UserRef { get; }

        public string PayeRef { get; }

        public string Aorn { get; }

        public string SchemeName { get; }

        public string CorrelationId { get; }

        public DateTime EventDateTime { get; }

        public AddedPayeSchemeCommand(long accountId, string userName, Guid userRef, string payeRef, string aorn, string schemeName, string correlationId, DateTime eventDateTime)
        {
            AccountId = accountId;
            UserName = userName;
            UserRef = userRef;
            PayeRef = payeRef;
            Aorn = aorn;
            SchemeName = schemeName;
            CorrelationId = correlationId;
            EventDateTime = eventDateTime;
        }
    }
}