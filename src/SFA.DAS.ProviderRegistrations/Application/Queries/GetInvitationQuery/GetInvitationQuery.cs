using MediatR;

namespace SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationQuery
{
    public class GetInvitationQuery : IRequest<GetInvitationQueryResult>
    {
        public GetInvitationQuery(long ukprn, string userRef, string sortColumn, string sortDirection, string secondarySortColumn)
        {
            Ukprn = ukprn;
            UserRef = userRef;
            SortColumn = sortColumn;
            SortDirection = sortDirection;
            SecondarySortColumn = secondarySortColumn;
        }

        public long Ukprn { get; }

        public string UserRef { get; }

        public string SortColumn { get; }

        public string SortDirection { get; }

        public string SecondarySortColumn { get; }
    }
}