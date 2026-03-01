using CarMarketApp.Application.DTOs.Brands;
using CarMarketApp.Application.Features.Commands.Brands;
using CarMarketApp.Application.Features.Queries.Brands;
using CarMarketApp.Application.Models;
using CarMarketApp.Application.Models.ResultPattern;
using CarMarketApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarMarketApp.WebAPI.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class BrandsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BrandsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> Create([FromBody] CreateBrandDto createBrandDto, CancellationToken cancellationToken)
    {
        Result result = await _mediator.Send(new CreateBrandCommand(createBrandDto), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> Update([FromBody] UpdateBrandDto updateBrandDto, CancellationToken cancellationToken)
    {
        Result result = await _mediator.Send(new UpdateBrandCommand(updateBrandDto), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> Restore([FromQuery] Guid brandId, CancellationToken cancellationToken)
    {
        Result result = await _mediator.Send(new RestoreBrandCommand(brandId), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> Delete([FromQuery] Guid brandId, CancellationToken cancellationToken)
    {
        Result result = await _mediator.Send(new DeleteBrandCommand(brandId), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] BrandFilterDto brandFilterDto, CancellationToken cancellationToken)
    {
        Result<PagedList<BrandDto>> result = await _mediator.Send(new GetAllBrandsQuery(brandFilterDto), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }
}
