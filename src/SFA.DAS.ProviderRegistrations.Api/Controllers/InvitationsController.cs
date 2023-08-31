using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ProviderRegistrations.Application.Queries.GetInvitationByIdQuery;

namespace SFA.DAS.ProviderRegistrations.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvitationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public InvitationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{correlationId}")]
    public async Task<ActionResult> Get(string correlationId, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(correlationId, out var correlationGuid))
        {
            ModelState.AddModelError(nameof(correlationId), "An invalid correlation id was supplied");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(new GetInvitationByIdQuery(correlationGuid), cancellationToken);

        return Ok(result.Invitation);
    }
}