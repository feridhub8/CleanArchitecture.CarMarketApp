using CarMarketApp.Application.DTOs.Adverts;
using CarMarketApp.Application.Features.Commands.Adverts;
using CarMarketApp.Application.Features.Queries.Adverts;
using CarMarketApp.Application.Models;
using CarMarketApp.Application.Models.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarMarketApp.WebAPI.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AdvertsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdvertsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateAdvertDto createAdvertDto, CancellationToken cancellationToken)
    {
        Claim? userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim is null)
            return Unauthorized("User not found");

        if (!Guid.TryParse(userIdClaim.Value, out var userId))
            return BadRequest("Invalid user id");

        Result result = await _mediator.Send(new CreateAdvertCommand(createAdvertDto, userId), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] UpdateAdvertDto updateAdvertDto, CancellationToken cancellationToken)
    {
        Claim? userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim is null)
            return Unauthorized("User not found");

        if (!Guid.TryParse(userIdClaim.Value, out var userId))
            return BadRequest("Invalid user id");

        Result result = await _mediator.Send(new UpdateAdvertCommand(updateAdvertDto, userId), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Restore([FromQuery] Guid advertId, CancellationToken cancellationToken)
    {
        Result result = await _mediator.Send(new RestoreAdvertCommand(advertId), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> Delete([FromQuery] Guid advertId, CancellationToken cancellationToken)
    {
        Claim? userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim is null)
            return Unauthorized("User not found");

        if (!Guid.TryParse(userIdClaim.Value, out var userId))
            return BadRequest("Invalid user id");

        Result result = await _mediator.Send(new DeleteAdvertCommand(advertId, userId), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] AdvertFilterDto advertFilterDto, CancellationToken cancellationToken)
    {
        Result<PagedList<AdvertDto>> result = await _mediator.Send(new GetAllAdvertsQuery(advertFilterDto), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }
}
