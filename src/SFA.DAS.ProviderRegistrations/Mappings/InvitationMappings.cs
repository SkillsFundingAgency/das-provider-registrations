using AutoMapper;
using SFA.DAS.ProviderRegistrations.Extensions;
using SFA.DAS.ProviderRegistrations.Models;
using SFA.DAS.ProviderRegistrations.Types;

namespace SFA.DAS.ProviderRegistrations.Mappings
{
    public class InvitationMappings : Profile
    {
        public InvitationMappings()
        {
            CreateMap<Invitation, InvitationDto>()
                .ForMember(d => d.State, opt => opt.MapFrom(EnumerableExpressionHelper.CreateEnumToStringExpression((Invitation d) => ((InvitationStatus) d.Status))))
                .ForMember(d => d.SentDate, opt => opt.MapFrom(s => s.UpdatedDate));

            CreateMap<InvitationEvents, InvitationEventsDto>();
        }
    }
}