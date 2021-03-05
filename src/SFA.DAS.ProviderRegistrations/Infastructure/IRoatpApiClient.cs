using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ProviderRegistrations.Models;

namespace SFA.DAS.ProviderRegistrations.Infastructure
{
    public interface IRoatpApiClient
    {
        Task<OrganisationSearchResult> GetOrganisationByUkprn(long ukprn);
    }
}