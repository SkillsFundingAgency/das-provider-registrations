using System.ComponentModel.DataAnnotations;
using MediatR;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetEmailAddressInUseQuery;

namespace SFA.DAS.ProviderRegistrations.Web.Validation
{
    public class InUseAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var mediator = (IMediator) validationContext.GetService(typeof(IMediator));
          
            return mediator.Send(new GetEmailAddressInUseQuery((value as string)?.Trim().ToLower())).Result
                ? new ValidationResult("This email address is already being used on an existing account.")
                : ValidationResult.Success;
        }
    }
}