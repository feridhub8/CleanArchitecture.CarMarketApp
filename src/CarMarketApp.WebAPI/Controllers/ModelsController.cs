using CarMarketApp.Application.DTOs.Models;
using CarMarketApp.Application.Features.Commands.Models;
using CarMarketApp.Application.Features.Queries.Brands;
using CarMarketApp.Application.Features.Queries.Models;
using CarMarketApp.Application.Models;
using CarMarketApp.Application.Models.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarMarketApp.WebAPI.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ModelsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ModelsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> Create([FromBody] CreateModelDto createModelDto, CancellationToken cancellationToken)
    {
        Result result = await _mediator.Send(new CreateModelCommand(createModelDto), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> Update([FromBody] UpdateModelDto updateModelDto, CancellationToken cancellationToken)
    {
        Result result = await _mediator.Send(new UpdateModelCommand(updateModelDto), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> Restore([FromQuery] Guid modelId, CancellationToken cancellationToken)
    {
        Result result = await _mediator.Send(new RestoreModelCommand(modelId), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> Delete([FromQuery] Guid modelId, CancellationToken cancellationToken)
    {
        Result result = await _mediator.Send(new DeleteModelCommand(modelId), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] ModelFilterDto modelFilterDto, CancellationToken cancellationToken)
    {
        Result<PagedList<ModelDto>> result = await _mediator.Send(new GetAllModelsQuery(modelFilterDto), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }
}
