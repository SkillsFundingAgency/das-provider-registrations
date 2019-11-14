using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetUnsubscribedQuery;
using SFA.DAS.ProviderRegistrations.Web.Authentication;

namespace SFA.DAS.ProviderRegistrations.Web.Validation
{
    public class UnsubscribedAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var mediator = (IMediator) validationContext.GetService(typeof(IMediator));
            var authenticationService = (IAuthenticationService) validationContext.GetService(typeof(IAuthenticationService));

            return mediator.Send(new GetUnsubscribedQuery(authenticationService.Ukprn.Value, (value as string)?.Trim().ToLower())).Result 
                ? new ValidationResult("This employer has unsubscribed from invitation emails") 
                : ValidationResult.Success;
        }
    }
}
