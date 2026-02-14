using CarMarketApp.Application.DTOs.Users;
using CarMarketApp.Application.Features.Commands;
using CarMarketApp.Application.Models.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarMarketApp.WebAPI.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto, CancellationToken cancellationToken)
    {
        Result? result = await _mediator.Send(new RegisterUserCommand(registerUserDto), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }
}
