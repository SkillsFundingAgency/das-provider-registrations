using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ProviderRegistrations.Application.Commands.UnsubscribeByIdCommand;

namespace SFA.DAS.ProviderRegistrations.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UnsubscribeController : ControllerBase
{
    private readonly IMediator _mediator;

    public UnsubscribeController(IMediator mediator)
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

        await _mediator.Send(new UnsubscribeByIdCommand(correlationGuid), cancellationToken);

        return Ok();
    }
}