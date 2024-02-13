namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetEmailAddressInUseQuery;

public class GetEmailAddressInUseQuery : IRequest<bool>
{
    public GetEmailAddressInUseQuery(string emailAddress)
    {
        EmailAddress = emailAddress;
    }

    public string EmailAddress { get; }
}