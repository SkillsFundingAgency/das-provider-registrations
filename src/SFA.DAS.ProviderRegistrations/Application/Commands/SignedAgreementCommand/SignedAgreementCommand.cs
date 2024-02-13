namespace SFA.DAS.ProviderRegistrations.Application.Commands.SignedAgreementCommand;

public class SignedAgreementCommand : IRequest
{
    public long AccountId { get; }
    public DateTime EventDateTime { get; }
    public string OrganisationName { get; }

    public long AgreementId { get; }

    public long LegalEntityId { get; }

    public bool CohortCreated { get; }

    public string UserName { get; }

    public Guid UserRef { get; }

    public string CorrelationId { get; }

    public SignedAgreementCommand(long accountId, DateTime eventDateTime, long agreementId, string organisationName, long legalEntityId, bool cohortCreated, string userName, Guid userRef, string correlationId)
    {
        AccountId = accountId;
        EventDateTime = eventDateTime;
        AgreementId = agreementId;
        OrganisationName = organisationName;
        UserName = userName;
        UserRef = userRef;
        CohortCreated = cohortCreated;
        LegalEntityId = legalEntityId;
        CorrelationId = correlationId;
    }
}